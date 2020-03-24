using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        [HttpGet("{Id}")]
        public IActionResult GetUser(string Id)
        {
            var User = _repository.Users.GetUserById(Id);
            User.PasswordHash = null;

            return Ok(new { user = User });
        }

        [HttpGet("Role/{Id}")]
        public IActionResult GetRole(string Id)
        {
            var userRole = _repository.Users.GetUserById(Id).Role;

            return Ok(new { role = userRole });
        }

        [HttpPut("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateViewModel UserInfo, string Id)
        {
            var Errors = await UpdateUserInfo(UserInfo, Id);
            if (Errors == null)
            {
                var User = _repository.Users.GetUserByUserName(UserInfo.UserName);
                var tokenString = _cookieManager.GenerateJSONWebToken(User);
                var cookieOption = _cookieManager.CreateCookieOption(1400);
                Response.Cookies.Append("tokenCookie", tokenString, cookieOption);
                return Ok();
            }
            return BadRequest(new { errors = Errors });
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationViewModel newUserInfo)
        {
            var User = new User { UserName = newUserInfo.UserName, Email = newUserInfo.Email, PhoneNumber = newUserInfo.PhoneNumber, Role = Role.Student.ToString() };
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
        public async Task<IActionResult> Login([FromBody]UserLoginViewModel login)
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

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var User = _repository.Users.GetUserById(Id);
            if (User != null)
            {
                _repository.Users.Remove(User);
                await _repository.Complete();
                return Ok();
            }
            return NotFound();
        }

        private async Task<IDictionary<string, string>> UpdateUserInfo(UserUpdateViewModel UserInfo, string Id)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var User = _repository.Users.GetUserById(Id);

            if (UserInfo.UserName != User.UserName)
            {
                if (await _userManager.FindByNameAsync(UserInfo.UserName) == null)
                {
                    User.UserName = (UserInfo.UserName == User.UserName ? User.UserName : UserInfo.UserName);
                    User.NormalizedUserName = (UserInfo.UserName.ToUpper() == User.NormalizedUserName ? User.NormalizedUserName : UserInfo.UserName.ToUpper());
                }
                else
                {
                    errors.Add("NewUserName", "Username is already in use.");
                }
            }

            if (UserInfo.Email != User.Email)
            {
                if (await _userManager.FindByEmailAsync(UserInfo.Email) == null)
                {
                    User.Email = (UserInfo.Email == User.UserName ? User.UserName : UserInfo.Email);
                    User.NormalizedEmail = (UserInfo.Email.ToUpper() == User.NormalizedEmail ? User.NormalizedEmail : UserInfo.Email.ToUpper());
                }
                else
                {
                    errors.Add("NewEmail", "Email is already in use.");
                }
            }

            if (UserInfo.PhoneNumber != User.PhoneNumber)
            {
                User.PhoneNumber = (UserInfo.PhoneNumber == User.PhoneNumber ? User.PhoneNumber : UserInfo.PhoneNumber);
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