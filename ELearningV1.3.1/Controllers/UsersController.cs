using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Models;
using ELearningV1._3._1.ViewModels;
using ELearningV1._3._1.Managers;
using ELearningV1._3._1.Enums;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Units;

namespace ELearningV1._3._1.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private UserManager<User> _userManager;
        private CookieManager _cookieManager;
        public UsersController(UnitOfWork repository, UserManager<User> userManager, CookieManager cookieOptionsManager)
        {
            _repository = repository;
            _userManager = userManager;
            _cookieManager = cookieOptionsManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AllUsers()
        {
            var Users = await _repository.Users.GetAll();

            return Ok(new { users = Users });
        }

        [HttpGet("Users_by_role/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var Users = await _repository.Users.GetUsersByRole(role);

            return Ok(new { users = Users });
        }

        [HttpGet("{userName}")]
        public IActionResult GetUser(string userName)
        {
            var User = _repository.Users.GetUserByUserName(userName);
            User.PasswordHash = null;

            return Ok(new { user = User });
        }

        [HttpGet("Role/{userName}")]
        public IActionResult GetRole(string userName)
        {
            var userRole = _repository.Users.GetUserRoleByUserName(userName);

            return Ok(new { role = userRole });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateableUserinfo UserInfo)
        {
            var Errors = await ChangeUserInfo(UserInfo);
            if (Errors == null)
            {
                var User = _repository.Users.GetUserByUserName(UserInfo.NewUserName);
                var tokenString = _cookieManager.GenerateJSONWebToken(User);
                var cookieOption = _cookieManager.CreateCookieOption(1400);
                Response.Cookies.Append("tokenCookie", tokenString, cookieOption);
                return Ok();
            }
            return BadRequest(new { errors = Errors });
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistration newUserInfo)
        {
            var User = new User { UserName = newUserInfo.UserName, Email = newUserInfo.Email, PhoneNumber = newUserInfo.PhoneNumber, Role = "Student" };
            var result = await _userManager.CreateAsync(User, newUserInfo.Password);
            var Errors = new Dictionary<string, string>();
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Role.Student.ToString());
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors.ToArray())
                {
                    Errors.Add(error.Code, error.Description);
                }
            }
            return BadRequest(new { errors = Errors });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserLogin login)
        {
            var User = _repository.Users.GetUserByUserName(login.UserName);

            if (await _userManager.CheckPasswordAsync(User, login.Password))
            {
                var tokenString = _cookieManager.GenerateJSONWebToken(User);
                var cookieOption = _cookieManager.CreateCookieOption(1400);
                Response.Cookies.Append("tokenCookie", tokenString, cookieOption);
                return Ok();
            }
            var Errors = new Dictionary<string, string>() { ["errors"] = "Invalid username or password !" };
            return NotFound(new { errors = Errors });
        }

        [HttpDelete("{userName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var User = _repository.Users.GetUserByUserName(userName);
            if (User != null)
            {
                _repository.Users.Remove(User);
                await _repository.Complete();
                return Ok();
            }
            return NotFound();
        }

        private async Task<IDictionary<string, string>> ChangeUserInfo(UpdateableUserinfo UserInfo)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var User = _repository.Users.GetUserByUserName(UserInfo.OldUserName);

            if (UserInfo.NewUserName != UserInfo.OldUserName)
            {
                if (await _userManager.FindByNameAsync(UserInfo.NewUserName) == null)
                {
                    User.UserName = (UserInfo.NewUserName == User.UserName ? User.UserName : UserInfo.NewUserName);
                    User.NormalizedUserName = (UserInfo.NewUserName.ToUpper() == User.NormalizedUserName ? User.NormalizedUserName : UserInfo.NewUserName.ToUpper());
                }
                else
                {
                    errors.Add("NewUserName", "Username is already in use.");
                }
            }

            if (UserInfo.NewEmail != UserInfo.OldEmail)
            {
                if (await _userManager.FindByEmailAsync(UserInfo.NewEmail) == null)
                {
                    User.Email = (UserInfo.NewEmail == User.UserName ? User.UserName : UserInfo.NewEmail);
                    User.NormalizedEmail = (UserInfo.NewEmail.ToUpper() == User.NormalizedEmail ? User.NormalizedEmail : UserInfo.NewEmail.ToUpper());
                }
                else
                {
                    errors.Add("NewEmail", "Email is already in use.");
                }
            }

            if (UserInfo.NewPhoneNumber != UserInfo.OldPhoneNumber)
            {
                User.PhoneNumber = (UserInfo.NewPhoneNumber == User.PhoneNumber ? User.PhoneNumber : UserInfo.NewPhoneNumber);
            }

            if (errors.Count < 1)
            {
                _repository.Users.Update(User);
                await _repository.Complete();
                return null;
            }
            return errors;
        }
    }
}