using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Context;
using ELearningV1._3._1.Models;
using ELearningV1._3._1.ViewModels;
using ELearningV1._3._1.Managers;

namespace ELearningV1._3._1.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiContext _context;
        private UserManager<User> _userManager;
        private CookieManager _cookieManager;
        public UsersController(ApiContext apiContext, UserManager<User> userManager, CookieManager cookieOptionsManager)
        {
            _context = apiContext;
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
            var UserT = await _context.UsersT.ToArrayAsync();
            return Ok(new { users = UserT });
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            var UserT = await _context.UsersT.ToArrayAsync();
            var User = UserT.First(u => u.UserName.Equals(userName));
            User.PasswordHash = null;

            return Ok(new { user = User });
        }

        [HttpGet("Role/{userName}")]
        public async Task<IActionResult> GetRole(string userName)
        {
            var UserT = await _context.UsersT.ToArrayAsync();
            var userRole = UserT.First(u => u.UserName.Equals(userName)).Role;

            return Ok(new { role = userRole });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateableUserinfo UserInfo)
        {
            var Errors = await ChangeUserInfo(UserInfo);
            if (Errors == null)
            {
                var UserT = await _context.UsersT.ToArrayAsync();
                var User = UserT.First(u => u.UserName.Equals(UserInfo.NewUserName));
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
                await _userManager.AddToRoleAsync(User, "Student");
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
            var UserT = await _context.UsersT.ToListAsync();
            var Errors = new Dictionary<string, string>() { ["errors"] = "Invalid username or password !" };
            User User;
            try
            {
                User = UserT.First(u => u.UserName.Equals(login.UserName));
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { errors = Errors });
            }
            var result = await _userManager.CheckPasswordAsync(User, login.Password);
            if (result)
            {
                var tokenString = _cookieManager.GenerateJSONWebToken(User);
                var cookieOption = _cookieManager.CreateCookieOption(1400);
                Response.Cookies.Append("tokenCookie", tokenString, cookieOption);
                return Ok();
            }
            return NotFound(new { errors = Errors });
        }

        [HttpDelete("{userName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var UserT = await _context.UsersT.ToArrayAsync();
            var user = UserT.First(u => u.UserName.Equals(userName));
            if (user != null)
            {
                _context.UsersT.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        private async Task<IDictionary<string, string>> ChangeUserInfo(UpdateableUserinfo UserInfo)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var UserT = await _context.UsersT.ToArrayAsync();
            var User = UserT.First(u => u.UserName.Equals(UserInfo.OldUserName));

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
                _context.UsersT.Update(User);
                await _context.SaveChangesAsync();
                return null;
            }
            return errors;
        }
    }
}