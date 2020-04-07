using System.Collections.Generic;
using Xunit;
using NSubstitute;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Mvc;
using ELearningV1._3._1.Controllers;

namespace ELearningTests
{
    public class ModuleContentsTests
    {
        [Fact]
        public async void GetModuleContentByModuleId_CalledWithValidId_ReturnsOkWithListOfModuleContent()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();

            var testId = 1;
            var testModule = new Module { Id = testId };
            var testModuleContents = new List<ModuleContent>() { new ModuleContent { Module = testModule }, new ModuleContent { Module = testModule }, new ModuleContent { Module = testModule }, new ModuleContent { Module = testModule } };

            unitOfWork.ModuleContents.GetModuleContentsByModuleId(testId).Returns(testModuleContents);

            var moduleContentsController = new ModuleContentsController(unitOfWork);

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

            var testId = 1235;
            List<ModuleContent> nullModuleContents = null;

            unitOfWork.ModuleContents.GetModuleContentsByModuleId(testId).Returns(nullModuleContents);

            var moduleContentsController = new ModuleContentsController(unitOfWork);

            var result = await moduleContentsController.GetModuleContentByModuleId(testId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(testId, returnValue);

        }

        [Fact]
        public async void GetAllModuleContents_Called_ReturnsOkWithListOfModuleContents()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();

            var testModuleContents = new List<ModuleContent>() { new ModuleContent(), new ModuleContent(), new ModuleContent(), new ModuleContent() };

            unitOfWork.ModuleContents.GetAll().Returns(testModuleContents);

            var moduleContentsController = new ModuleContentsController(unitOfWork);

            var result = await moduleContentsController.GetAllModuleContents();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ModuleContent>>(okObjectResult.Value);

            Assert.Equal(testModuleContents.Count, returnValue.Count);
        }
    }
}
