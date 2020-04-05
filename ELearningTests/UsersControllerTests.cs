using System;
using ELearningV1._3._1.Controllers;
using ELearningV1._3._1.Interfaces;
using NSubstitute;
using Xunit;
using Microsoft.AspNetCore.Identity;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ELearningV1._3._1.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;

namespace ELearningTests
{

    public class UsersControllerTests
    {


        [Fact]
        public void Index_Called_ReturnsOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);
            var result = usersController.Index();

            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async void AllUsers_Called_ReturnsOkWithListOfUsers()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();


            List<User> users = new List<User>();

            for (var i = 0; i < 10; i++)
            {
                users.Add(new User());
            }

            _unitOfWork.Users.GetAll().Returns(users);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.AllUsers();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            var returnValue = Assert.IsType<List<User>>(okObjectResult.Value);

            Assert.Equal(10, returnValue.Count);

        }

        [Fact]
        public async void GetUsersByRole_Called_ReturnsOkWithListOfUsersByRole()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testRole = "Student";

            var users = new List<User>();

            for (int i = 0; i < 10; i++)
            {
                users.Add(new User { Role = testRole });
            }


            _unitOfWork.Users.GetUsersByRole(testRole).Returns(users);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.GetUsersByRole(testRole);
            var okObjetResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<User>>(okObjetResult.Value);

            Assert.Equal(testRole, returnValue.FirstOrDefault().Role);
        }

        [Fact]
        public async void GetUsersByRole_CalledWithInvalidRole_ReturnsNotFoundWithRole()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testRole = "";

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.GetUsersByRole(testRole);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<string>(notFoundObjectResult.Value);

            Assert.Equal(testRole, returnValue);

        }

        [Fact]
        public async void GetUser_CalledWithValidId_ReturnsOkWithUser()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var Id = "1";
            User user = new User { Id = Id };

            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(user);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.GetUser(Id);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<User>(okObjectResult.Value);

            Assert.Equal(Id, returnValue.Id);
        }

        [Fact]
        public async void GetUser_CalledWithInvalidId_ReturnsNotFoundWithId()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testId = "";
            User nullUser = null;

            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(nullUser);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.GetUser(testId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<string>(notFoundObjectResult.Value);

            Assert.Equal(testId, returnValue);

        }

        [Fact]
        public async void GetRole_CalledWithValidId_ReturnsOkWithUserRole()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testId = "1";
            var role = "Student";

            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(new User { Role = role });

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.GetRole(testId);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okObjectResult.Value);

            Assert.Equal(role, returnValue);


        }

        [Fact]
        public async void GetRole_CalledWithInvalidId_ReturnsNotFoundWithUserId()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testId = "";
            User nullUser = null;

            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(nullUser);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.GetRole(testId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<string>(notFoundObjectResult.Value);

            Assert.Equal(testId, returnValue);
        }

        [Fact]
        public async void UpdateUser_CalledWithValidParameters_ReturnsOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();
            var context = Substitute.For<HttpContext>();


            var testId = "1";
            UserUpdateViewModel userInfo = new UserUpdateViewModel() { UserName = "TestUser" };
            Dictionary<string, string> nullDictionary = null;
            User user = new User() { UserName = "TestUser" };
            CookieOptions option = new CookieOptions();

            _unitOfWork.Users.UpdateUser(userInfo, user).Returns(nullDictionary);
            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(user);
            _unitOfWork.Users.GetUserByUserName(userInfo.UserName).Returns(user);
            _unitOfWork.Complete().Returns(2);
            _cookieManager.GenerateJSONWebToken(user).Returns("Token");
            _cookieManager.CreateCookieOption().Returns(option);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);
            usersController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await usersController.UpdateUser(userInfo, testId);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async void UpdateUser_CalledWithInvalidUserInfo_ReturnsBadRequestWithUserInfoError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testId = "1";
            User user = new User();
            UserUpdateViewModel nullUserInfo = null;
            string error = "User info cannot be null.";
            Dictionary<string, string> nullDictionary = null;

            _unitOfWork.Users.UpdateUser(nullUserInfo, user).Returns(nullDictionary);
            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(user);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.UpdateUser(nullUserInfo, testId);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);

        }
        [Fact]
        public async void UpdateUser_CalledWithInvalidUserId_ReturnsNotFoundWithUserId()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testId = "";
            User nullUser = null;
            UserUpdateViewModel userInfo = new UserUpdateViewModel();
            Dictionary<string, string> nullDictionary = null;

            _unitOfWork.Users.UpdateUser(userInfo, nullUser).Returns(nullDictionary);
            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(nullUser);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.UpdateUser(userInfo, testId);
            var notFoundObjetResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<string>(notFoundObjetResult.Value);

            Assert.Equal(testId, returnValue);
        }

        [Fact]
        public async void UpdateUser_CalledAndUpdateIsFailed_ReturnsBadRequestWithErrorDictionary()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testId = "1";
            User user = new User();
            UserUpdateViewModel userInfo = new UserUpdateViewModel() { UserName = "Nees612" };
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("UserName", "Username is already in use.");

            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(user);
            _unitOfWork.Users.UpdateUser(userInfo, user).Returns(dictionary);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.UpdateUser(userInfo, testId);
            var badRequestobjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Dictionary<string, string>>(badRequestobjectResult.Value);

            Assert.Equal(dictionary, returnValue);
        }

        [Fact]
        public async void Registration_CalledWithValidModel_ReturnsOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();
            var manager = Substitute.For<UserManager<User>>(_usersStore, null, null, null, null, null, null, null, null);

            UserRegistrationViewModel model = new UserRegistrationViewModel();


            manager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(Task.FromResult(IdentityResult.Success));
            manager.AddToRoleAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(Task.FromResult(IdentityResult.Success));

            var usersController = new UsersController(_unitOfWork, manager, _cookieManager);

            var result = await usersController.Registration(model);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void Registration_CalledWithInvalidModel_ReturnsOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            UserRegistrationViewModel model = null;
            string error = "User info cannot be null.";

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.Registration(model);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void Registration_CalledAndFailedUserCreate_ReturnsBadRequestWithErrorDictionary()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();
            var manager = Substitute.For<UserManager<User>>(_usersStore, null, null, null, null, null, null, null, null);

            UserRegistrationViewModel model = new UserRegistrationViewModel();
            Dictionary<string, string> errors = new Dictionary<string, string>();
            manager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(Task.FromResult(IdentityResult.Failed()));

            var usersController = new UsersController(_unitOfWork, manager, _cookieManager);

            var result = await usersController.Registration(model);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Dictionary<string, string>>(badRequestObjectResult.Value);

            Assert.Equal(errors, returnValue);
        }

        [Fact]
        public async void Login_CalledWithValidModel_ReturnsOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();
            var context = Substitute.For<HttpContext>();
            var manager = Substitute.For<UserManager<User>>(_usersStore, null, null, null, null, null, null, null, null);

            UserLoginViewModel model = new UserLoginViewModel() { UserName = "Test" };
            User user = new User { UserName = "Test" };
            CookieOptions option = new CookieOptions();
            manager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(true);

            _cookieManager.GenerateJSONWebToken(user).Returns("Token");
            _cookieManager.CreateCookieOption().Returns(option);
            _unitOfWork.Users.GetUserByUserName(model.UserName).Returns(user);

            var usersController = new UsersController(_unitOfWork, manager, _cookieManager);
            usersController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await usersController.Login(model);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async void Login_CalledWithInvalidUserName_ReturnsNotFoundWithError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();
            var manager = Substitute.For<UserManager<User>>(_usersStore, null, null, null, null, null, null, null, null);

            UserLoginViewModel model = new UserLoginViewModel() { UserName = "Test" };
            User user = new User { UserName = "Test" };
            Dictionary<string, string> errors = new Dictionary<string, string>() { ["errors"] = "Invalid username or password !" };

            _unitOfWork.Users.GetUserByUserName(model.UserName).Returns(user);
            manager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(false);

            var usersController = new UsersController(_unitOfWork, manager, _cookieManager);

            var result = await usersController.Login(model);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Dictionary<string, string>>(notFoundObjectResult.Value);

            Assert.Equal(errors, returnValue);
        }

        [Fact]
        public async void Login_CalledWithNull_ReturnsBadRequestWithError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            UserLoginViewModel model = null;
            string error = "Login info cannot be null.";

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.Login(model);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestResult.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void DeleteUser_CalledWithValidParameters_ReturnsOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();
            var manager = Substitute.For<UserManager<User>>(_usersStore, null, null, null, null, null, null, null, null);

            var testId = "1";
            User user = new User();

            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(user);
            _unitOfWork.Complete().Returns(2);

            var usersController = new UsersController(_unitOfWork, manager, _cookieManager);

            var result = await usersController.DeleteUser(testId);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);

        }
        [Fact]
        public async void DeleteUser_CalledWithInvalidId_ReturnNotFoundWithId()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            var testId = "";
            User nullUser = null;

            _unitOfWork.Users.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(nullUser);

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.DeleteUser(testId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<string>(notFoundObjectResult.Value);

            Assert.Equal(testId, returnValue);

        }
        [Fact]
        public async void DeleteUser_CalledWithNull_ReturnBadRequestWithError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            IUserStore<User> _usersStore = Substitute.For<IUserStore<User>>();

            string testId = null;
            string error = "Id cannot be null.";

            var usersController = new UsersController(_unitOfWork, new UserManager<User>(_usersStore, null, null, null, null, null, null, null, null), _cookieManager);

            var result = await usersController.DeleteUser(testId);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);
        }
    }
}
