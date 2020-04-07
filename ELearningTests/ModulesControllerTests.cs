using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using ELearningV1._3._1.Controllers;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Mvc;

namespace ELearningTests
{
    public class ModulesControllerTests
    {
        [Fact]
        public async void GetAll_Called_RetunsOkWithListOfModules()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();

            var testModules = new List<Module>() { new Module(), new Module(), new Module(), new Module() };

            unitOfWork.Modules.GetAll().Returns(testModules);

            var modulesController = new ModulesController(unitOfWork);

            var result = await modulesController.GetAll();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Module>>(okObjectResult.Value);

            Assert.Equal(testModules.Count, returnValue.Count);
        }

    }
}
