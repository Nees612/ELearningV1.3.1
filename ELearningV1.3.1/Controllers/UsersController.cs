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
        public async Task<IActionResult> GetUser(string Id)
        {
            var User = await _repository.Users.GetUserById(Id);
            User.PasswordHash = null;

            return Ok(new { user = User });
        }

        [HttpGet("Role/{Id}")]
        public async Task<IActionResult> GetRole(string Id)
        {
            var userRole = (await _repository.Users.GetUserById(Id)).Role;

            return Ok(new { role = userRole });
        }

        [HttpPut("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateViewModel UserInfo, string Id)
        {
            var Errors = await _repository.Users.UpdateUser(UserInfo, Id);
            if (Errors == null)
            {
                await _repository.Complete();
                var User = await _repository.Users.GetUserByUserName(UserInfo.UserName);
                var tokenString = _cookieManager.GenerateJSONWebToken(User);
                var cookieOption = _cookieManager.CreateCookieOption(1400);
                Response.Cookies.Append("tokenCookie", tokenString, cookieOption);
                return Ok();
            }
            return BadRequest(new { errors = Errors });
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationViewModel newUser)
        {
            var User = new User { UserName = newUser.UserName, Email = newUser.Email, PhoneNumber = newUser.PhoneNumber, Role = Role.Student.ToString() };
            var result = await _userManager.CreateAsync(User, newUser.Password);
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
            var User = await _repository.Users.GetUserByUserName(login.UserName);

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
            var User = await _repository.Users.GetUserById(Id);
            if (User != null)
            {
                _repository.Users.Remove(User);
                await _repository.Complete();
                return Ok();
            }
            return NotFound();
        }

    }
}