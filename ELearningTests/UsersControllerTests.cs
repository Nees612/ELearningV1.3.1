using System;
using ELearningV1._3._1.Controllers;
using ELearningV1._3._1.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Mvc;

namespace ELearningTests
{

    [TestFixture]
    public class UsersControllerTests
    {
        [Test]
        public void Index_Called_ReturnsOk()
        {
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var cookieManager = Substitute.For<ICookieManager>();

            var usersController = new UsersController(unitOfWork, new UserManager<User>(null, null, null, null, null, null, null, null, null), cookieManager);
            var result = usersController.Index();

        }

    }
}
