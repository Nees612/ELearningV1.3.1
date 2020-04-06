using ELearningV1._3._1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using ELearningV1._3._1.Controllers;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ELearningV1._3._1.ViewModels;
using Microsoft.AspNetCore.Http;
using ELearningV1._3._1.Enums;

namespace ELearningTests
{
    public class AssigmentsControllerTests
    {
        [Fact]
        public async void GetAllAssigments_Called_ReturnOkWithAssigmentsCollection()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            var AssigmentList = new List<Assigment>() { new Assigment(), new Assigment(), new Assigment(), new Assigment() };

            _unitOfWork.Assigments.GetAll().Returns(AssigmentList);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.GetAllAssigments();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Assigment>>(okObjectResult.Value);

            Assert.Equal(AssigmentList.Count, returnValue.Count);
        }
        [Fact]

        public async void GetAssigmentsByModule_CalledWithValidModuleName_ReturnOkWithAssigmentsCollection()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            var oopModule = new Module() { Title = "Oop" };
            var oopAssigmentList = new List<Assigment>() { new Assigment() { Module = oopModule }, new Assigment() { Module = oopModule } };
            string moduleName = "Oop";

            _unitOfWork.Assigments.GetAssigmentsByModuleName(moduleName).Returns(oopAssigmentList);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.GetAssigmentsByModule(moduleName);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Assigment>>(okObjectResult.Value);

            Assert.Equal(oopAssigmentList.Count, returnValue.Count);
            Assert.Equal(moduleName, returnValue[0].Module.Title);
        }
        [Fact]
        public async void GetAssigmentsByModule_CalledWithNull_ReturnsBadRequestWithError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            string nullModuleName = null;
            string error = "Module name cannot be null.";

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.GetAssigmentsByModule(nullModuleName);
            var badRequestobjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestobjectResult.Value);

            Assert.Equal(error, returnValue);

        }
        [Fact]
        public async void GetRandomAssigment_CalledWithValidModulename_ReturnsOkWithAssigmentUrl()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            string testModuleName = "Oop";
            string[] testUrls = new string[] { "test", "test2" };
            Module oopModule = new Module() { Title = "Oop" };
            var assigments = new List<Assigment>() { new Assigment { Url = "test", Module = oopModule }, new Assigment { Url = "test2", Module = oopModule } };

            _unitOfWork.Assigments.GetAssigmentsByModuleName(testModuleName).Returns(assigments);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.GetRandomAssigment(testModuleName);
            var okobjectReesult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Assigment>(okobjectReesult.Value);

            Assert.Equal(testModuleName, returnValue.Module.Title);
            Assert.Contains(returnValue.Url, testUrls);
        }
        [Fact]
        public async void GetRandomAssigment_CalledInvalidModuleName_ReturnsNotFountWithModuleName()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            string notValidModuleName = "CSarja";
            List<Assigment> nullAssigment = null;

            _unitOfWork.Assigments.GetAssigmentsByModuleName(notValidModuleName).Returns(nullAssigment);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.GetRandomAssigment(notValidModuleName);
            var notFountObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<string>(notFountObjectResult.Value);

            Assert.Equal(notValidModuleName, returnValue);
        }
        [Fact]
        public async void GetRandomAssigment_CalledWithNull_ReturnsBadRequestWithError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            string nullModuleName = null;
            string error = "Module name cannot be null.";

            var assigmentController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentController.GetRandomAssigment(nullModuleName);
            var badRequestobjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestobjectResult.Value);

            Assert.Equal(error, returnValue);
        }
        [Fact]
        public async void AddAssigment_CalledWithValidAssigment_ReturnOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            long testId = 1;
            var testAssigment = new AssigmentViewModel() { ModuleId = testId };
            var testModule = new Module() { Id = testId };

            _unitOfWork.Modules.GetById(testId).Returns(testModule);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.AddAssigment(testAssigment);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async void AddAssigment_CalledWithInvalidAssigment_ReturnsNotFoundWithModuleId()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            long notValidId = default;
            var testAssigment = new AssigmentViewModel() { ModuleId = notValidId };
            Module nullModule = null;

            _unitOfWork.Modules.GetById(Arg.Any<long>()).Returns(nullModule);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.AddAssigment(testAssigment);
            var notFoundObjectREsult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectREsult.Value);

            Assert.Equal(notValidId, returnValue);
        }

        [Fact]
        public async void AddAssigment_CalledWithNull_ReturnsBadRequestWithError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();

            AssigmentViewModel nullAssigment = null;
            string error = "Assigment cannot be null.";

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);

            var result = await assigmentsController.AddAssigment(nullAssigment);
            var badRequestObjectResut = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResut.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void DeleteAssigment_CalledWithValidId_ReturnsOk()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testAssgiment = new Assigment();

            _cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());
            _unitOfWork.Assigments.GetById(Arg.Any<long>()).Returns(testAssgiment);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);
            assigmentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await assigmentsController.DeleteAssigment(testId);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void DeleteAssigment_CalledWithinvalidId_ReturnsNotFoundWithId()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = default;
            Assigment nullAssigment = null;

            _cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());
            _unitOfWork.Assigments.GetById(Arg.Any<long>()).Returns(nullAssigment);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);
            assigmentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await assigmentsController.DeleteAssigment(testId);
            var notFoundObjetResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjetResult.Value);

            Assert.Equal(testId, returnValue);
        }

        [Fact]
        public async void DeleteAssigment_CalledAndRoleIsNotAdmin_ReturnsBadRequestWithError()
        {
            IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
            ICookieManager _cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testAssigment = new Assigment();
            string error = "Only Admins can delete assigments.";

            _cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Student.ToString());
            _unitOfWork.Assigments.GetById(Arg.Any<long>()).Returns(testAssigment);

            var assigmentsController = new AssigmentsController(_unitOfWork, _cookieManager);
            assigmentsController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await assigmentsController.DeleteAssigment(testId);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);

        }
    }
}
