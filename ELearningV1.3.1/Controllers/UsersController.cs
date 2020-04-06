using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Models;
using ELearningV1._3._1.ViewModels;
using ELearningV1._3._1.Enums;
using ELearningV1._3._1.Interfaces;
using System;

namespace ELearningV1._3._1.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private UserManager<User> _userManager;
        private ICookieManager _cookieManager;

        public UsersController(IUnitOfWork repository, UserManager<User> userManager, ICookieManager cookieOptionsManager)
        {
            _repository = repository;
            _userManager = userManager;
            _cookieManager = cookieOptionsManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AllUsers()
        {
            var Users = await _repository.Users.GetAll();

            return Ok(Users);
        }

        [HttpGet("Users_by_role/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            if (role == null)
            {
                return BadRequest("Role cannot be null.");
            }

            if (Enum.IsDefined(typeof(Role), role))
            {
                var Users = await _repository.Users.GetUsersByRole(role);

                return Ok(Users);
            }
            return NotFound(role);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUser(string Id)
        {
            if (Id == null)
            {
                return BadRequest("Id cannot be null.");
            }

            var User = await _repository.Users.Get(u => u.Id.Equals(Id));

            if (User != null)
            {
                User.PasswordHash = null;
                return Ok(User);
            }

            return NotFound(Id);

        }

        [HttpGet("Role/{Id}")]
        public async Task<IActionResult> GetRole(string Id)
        {
            if (Id == null)
            {
                return BadRequest("Id cannot be null.");
            }

            var User = await _repository.Users.Get(u => u.Id.Equals(Id));

            if (User != null)
            {
                return Ok(User.Role);
            }

            return NotFound(Id);
        }

        [HttpPut("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateViewModel UserInfo, string Id)
        {

            if (UserInfo == null)
            {
                return BadRequest("User info cannot be null.");
            }

            var User = await _repository.Users.Get(u => u.Id.Equals(Id));

            if (User == null)
            {
                return NotFound(Id);
            }

            var Errors = await _repository.Users.UpdateUser(UserInfo, User);

            if (Errors == null)
            {
                await _repository.Complete();
                var updatedUser = await _repository.Users.GetUserByUserName(UserInfo.UserName);
                var tokenString = _cookieManager.GenerateJSONWebToken(updatedUser);
                var cookieOption = _cookieManager.CreateCookieOption();
                Response.Cookies.Append("tokenCookie", tokenString, cookieOption);
                return Ok();
            }

            return BadRequest(Errors);
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationViewModel newUser)
        {
            if (newUser == null)
            {
                return BadRequest("User info cannot be null.");
            }

            var User = new User { UserName = newUser.UserName, Email = newUser.Email, PhoneNumber = newUser.PhoneNumber, Role = Role.Student.ToString() };
            var result = await _userManager.CreateAsync(User, newUser.Password);
            var ErrorContext = new ErrorContext();

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Role.Student.ToString());
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors.ToArray())
                {
                    ErrorContext.Errors.Add(error.Description);
                }
            }
            return BadRequest(ErrorContext);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginViewModel login)
        {

            if (login == null)
            {
                return BadRequest("Login info cannot be null.");
            }

            var ErrorContext = new ErrorContext();
            var User = await _repository.Users.GetUserByUserName(login.UserName);

            if (await _userManager.CheckPasswordAsync(User, login.Password))
            {
                var tokenString = _cookieManager.GenerateJSONWebToken(User);
                var cookieOption = _cookieManager.CreateCookieOption();
                Response.Cookies.Append("tokenCookie", tokenString, cookieOption);
                return Ok();
            }
            ErrorContext.Errors.Add("Invalid Username or Password !");
            return BadRequest(ErrorContext);
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            if (Id == null)
            {
                return BadRequest("Id cannot be null.");
            }

            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var User = await _repository.Users.Get(u => u.Id.Equals(Id));
                if (User != null)
                {
                    _repository.Users.Remove(User);
                    await _repository.Complete();
                    return Ok();
                }
                return NotFound(Id);
            }

            return BadRequest("Only Admins can delete profiles.");

        }

    }
}