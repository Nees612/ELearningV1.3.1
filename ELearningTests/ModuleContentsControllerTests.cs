using System.Collections.Generic;
using Xunit;
using NSubstitute;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Mvc;
using ELearningV1._3._1.Controllers;
using Microsoft.AspNetCore.Http;
using ELearningV1._3._1.Enums;
using ELearningV1._3._1.ViewModels;

namespace ELearningTests
{
    public class ModuleContentsControllerTests
    {
        [Fact]
        public async void GetModuleContentByModuleId_CalledWithValidId_ReturnsOkWithListOfModuleContent()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            var testId = 1;
            var testModule = new Module { Id = testId };
            var testModuleContents = new List<ModuleContent>() { new ModuleContent { Module = testModule }, new ModuleContent { Module = testModule }, new ModuleContent { Module = testModule }, new ModuleContent { Module = testModule } };

            unitOfWork.ModuleContents.GetModuleContentsByModuleId(testId).Returns(testModuleContents);

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);

            var result = await moduleContentsController.GetModuleContentByModuleId(testId);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var retrunValue = Assert.IsType<List<ModuleContent>>(okObjectResult.Value);

            Assert.Equal(testModuleContents.Count, retrunValue.Count);
            Assert.Equal(testId, retrunValue[0].Module.Id);
        }

        [Fact]
        public async void GetModuleContentByModuleId_CalledWithInvalidId_ReturnsNotFoundWithId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            var testId = 1235;
            List<ModuleContent> nullModuleContents = null;

            unitOfWork.ModuleContents.GetModuleContentsByModuleId(testId).Returns(nullModuleContents);

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);

            var result = await moduleContentsController.GetModuleContentByModuleId(testId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(testId, returnValue);

        }

        [Fact]
        public async void GetAllModuleContents_Called_ReturnsOkWithListOfModuleContents()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            var testModuleContents = new List<ModuleContent>() { new ModuleContent(), new ModuleContent(), new ModuleContent(), new ModuleContent() };

            unitOfWork.ModuleContents.GetAll().Returns(testModuleContents);

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);

            var result = await moduleContentsController.GetAllModuleContents();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ModuleContent>>(okObjectResult.Value);

            Assert.Equal(testModuleContents.Count, returnValue.Count);
        }

        [Fact]
        public async void AddModuleContent_CalledWithValidModuleContent_ReturnsOk()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            var testId = 1;
            var testModule = new Module();
            var testModuleContent = new ModuleContentViewModel() { ModuleId = testId };

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());
            unitOfWork.Modules.GetById(testId).Returns(testModule);
            unitOfWork.ModuleContents.GetLastModuleContentByModuleId(testModuleContent.ModuleId).Returns(1);

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.AddModuleContent(testModuleContent);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async void AddModuleContent_CalledWithInvalidValidModuleContent_ReturnsNotFoundWithModuleId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            var notValidId = 1;
            Module nullModule = null;
            var testModuleContent = new ModuleContentViewModel() { ModuleId = notValidId };

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());
            unitOfWork.Modules.GetById(notValidId).Returns(nullModule);
            unitOfWork.ModuleContents.GetLastModuleContentByModuleId(testModuleContent.ModuleId).Returns(0);

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.AddModuleContent(testModuleContent);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(notValidId, returnValue);
        }

        [Fact]
        public async void AddModuleContent_CalledWithNull_ReturnsBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            ModuleContentViewModel nullModuleContent = null;
            string error = "Module content cannot be null.";

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.AddModuleContent(nullModuleContent);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);

        }

        [Fact]
        public async void AddModuleContent_CalledAndRoleIsNotAdmin_ReturnsBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            var testModuleContent = new ModuleContentViewModel();
            string error = "Only Admins can add new module contents.";

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Student.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.AddModuleContent(testModuleContent);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void DeleteModuleContent_CalledWithValidId_ReturnsOk()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testModuleContent = new ModuleContent();
            var testVideos = new List<Video>() { new Video(), new Video(), new Video(), new Video() };

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());
            unitOfWork.ModuleContents.GetById(testId).Returns(testModuleContent);
            unitOfWork.Videos.GetVideosByModuleContentId(testId).Returns(testVideos);

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.DeleteModuleContent(testId);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);

        }
        [Fact]
        public async void DeleteModuleContent_CalledWithInvalidId_RetrunsNotFoundWithId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long notValidId = 1;
            ModuleContent nullModuleContent = null;
            List<Video> nullVideos = null;

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());
            unitOfWork.ModuleContents.GetById(notValidId).Returns(nullModuleContent);
            unitOfWork.Videos.GetVideosByModuleContentId(notValidId).Returns(nullVideos);

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.DeleteModuleContent(notValidId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(notValidId, returnValue);
        }

        [Fact]
        public async void DeleteModuleContent_CalledAndUserRoleIsNotAdmin_ReturnsBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            string error = "Only Admins can delete module contents.";

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Student.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.DeleteModuleContent(testId);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void UpdateModuleContent_CalledWithValidModuleContentAndId_RetrunsOk()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testModuleContentView = new ModuleContentUpdateViewModel();
            var testModuleContent = new ModuleContent();

            unitOfWork.ModuleContents.GetById(testId).Returns(testModuleContent);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.UpdateModuleContent(testModuleContentView, testId);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void UpdateModuleContent_CalledWithNullModuleContentView_ReturnsBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            ModuleContentUpdateViewModel nullModuleContentView = null;
            var testModuleContent = new ModuleContent();
            string error = "Module content cannot be null.";

            unitOfWork.ModuleContents.GetById(testId).Returns(testModuleContent);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.UpdateModuleContent(nullModuleContentView, testId);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void UpdateModuleContent_CalledWithInvalidId_ReturnsNotFoundWithId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long notValidId = 1;
            var testModuleContentView = new ModuleContentUpdateViewModel();
            ModuleContent nullModuleContent = null;
            string error = "Module content cannot be null.";

            unitOfWork.ModuleContents.GetById(notValidId).Returns(nullModuleContent);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.UpdateModuleContent(testModuleContentView, notValidId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(notValidId, returnValue);
        }

        [Fact]
        public async void UpdateModuleContent_CalledAndRoleIsNotAdmin_ReturnsBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testModuleContentView = new ModuleContentUpdateViewModel();
            ModuleContent nullModuleContent = null;
            string error = "Only Admins can update module content.";

            unitOfWork.ModuleContents.GetById(testId).Returns(nullModuleContent);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Student.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await moduleContentsController.UpdateModuleContent(testModuleContentView, testId);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void ChangeModuleContentOrder_CalledWithValidIds_ReturnsOk()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            long testcontentId = 1;
            ModuleContent nullModuleContent = null;
            string error = "Only Admins can update module content.";

            unitOfWork.ModuleContents.GetById(testId).Returns(nullModuleContent);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Student.ToString());

            var moduleContentsController = new ModuleContentsController(unitOfWork, cookieManager);
            moduleContentsController.ControllerContext = new ControllerContext() { HttpContext = context };
        }

    }
}

