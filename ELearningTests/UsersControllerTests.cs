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

            Assert.IsType<OkResult>(result);

            var okResult = result as OkResult;

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
        public async void GetUser_CalledWithValid_ReturnsOkWithUser()
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




        //[Fact]
        //public void GetNote_ValidIdPassed_ReturnsOk()
        //{
        //    //Arrange
        //    var id = 1;

        //    // Act
        //    var noteObjectResult = _controller.GetNote(id);

        //    // Assert
        //    var okObjectResult = noteObjectResult as OkObjectResult;

        //    Assert.NotNull(okObjectResult);

        //    var model = okObjectResult.Value as NoteDto;

        //    Assert.NotNull(model);
        //    Assert.True(model.Id == id);
        //}

    }
}
