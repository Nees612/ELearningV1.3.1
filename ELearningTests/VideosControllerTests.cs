using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using ELearningV1._3._1.Controllers;
using Microsoft.AspNetCore.Mvc;
using ELearningV1._3._1.ViewModels;
using ELearningV1._3._1.Enums;
using Microsoft.AspNetCore.Http;

namespace ELearningTests
{
    public class VideosControllerTests
    {
        [Fact]
        public async void GetAllVideos_Called_ReturnsOkWithListOfVideos()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            var testList = new List<Video>() { new Video(), new Video(), new Video(), new Video() };

            unitOfWork.Videos.GetAll().Returns(testList);

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);

            var result = await videosController.GetAllVideos();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Video>>(okObjectResult.Value);

            Assert.Equal(testList.Count, returnValue.Count);
        }

        [Fact]
        public async void GetVideo_CalledWithValidId_ReturnsOkWithVideo()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            long testId = 1;
            var video = new Video() { Id = testId };

            unitOfWork.Videos.GetById(testId).Returns(video);

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);

            var result = await videosController.GetVideo(testId);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Video>(okObjectResult.Value);

            Assert.Equal(testId, returnValue.Id);
        }

        [Fact]
        public async void GetVideo_CalledWithInvalidId_ReturnsNotFoundWithId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            long notValidId = 1;
            Video video = null;

            unitOfWork.Videos.GetById(notValidId).Returns(video);

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);

            var result = await videosController.GetVideo(notValidId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(notValidId, returnValue);
        }

        [Fact]
        public async void GetVideosByModuleContentId_CalledWithValidId_ReturnOkWithListOfVideo()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            long testId = 1;
            var testModuleContent = new ModuleContent() { Id = testId };
            var testVideos = new List<Video>() { new Video() { ModuleContent = testModuleContent }, new Video() { ModuleContent = testModuleContent }, new Video() { ModuleContent = testModuleContent } };

            unitOfWork.Videos.GetVideosByModuleContentId(testId).Returns(testVideos);

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);

            var result = await videosController.GetVideosByModuleContentId(testId);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Video>>(okObjectResult.Value);

            Assert.Equal(testVideos.Count, returnValue.Count);
            Assert.Equal(testId, returnValue[0].ModuleContent.Id);

        }

        [Fact]
        public async void GetVideosByModuleContentId_CalledWithInvalidId_ReturnsNotFoundWithId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();

            long testId = 1;
            List<Video> nullVideos = null;

            unitOfWork.Videos.GetVideosByModuleContentId(testId).Returns(nullVideos);

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);

            var result = await videosController.GetVideosByModuleContentId(testId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(testId, returnValue);
        }

        [Fact]
        public async void AddVideo_CalledWithValidVideo_ReturnsOk()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testVideo = new VideoViewModel() { ModuleContentId = testId };
            var testModuleContent = new ModuleContent();
            string testUrl = "testurl";
            string convertedUrl = "converted";
            string testyoutubeId = "youtubeid";

            unitOfWork.ModuleContents.GetById(testId).Returns(testModuleContent);
            videoManager.ConvertUrl(testUrl).Returns(convertedUrl);
            videoManager.GetYoutubeId(convertedUrl).Returns(testyoutubeId);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);
            videosController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await videosController.AddVideo(testVideo);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async void AddVideo_CalledWithInvalidContentId_RetrunsNotFoundWithId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testVideo = new VideoViewModel() { ModuleContentId = testId };
            ModuleContent nullModuleContent = null;
            string testUrl = "testurl";
            string convertedUrl = "converted";
            string testyoutubeId = "youtubeid";

            unitOfWork.ModuleContents.GetById(testId).Returns(nullModuleContent);
            videoManager.ConvertUrl(testUrl).Returns(convertedUrl);
            videoManager.GetYoutubeId(convertedUrl).Returns(testyoutubeId);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);
            videosController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await videosController.AddVideo(testVideo);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var retrunValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(testId, retrunValue);
        }

        [Fact]
        public async void AddVideo_CalledWithNull_ReturnBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            VideoViewModel nullVideo = null;
            ModuleContent nullModuleContent = null;
            string testUrl = "testurl";
            string convertedUrl = "converted";
            string testyoutubeId = "youtubeid";
            string error = "Video cannot be null.";

            unitOfWork.ModuleContents.GetById(testId).Returns(nullModuleContent);
            videoManager.ConvertUrl(testUrl).Returns(convertedUrl);
            videoManager.GetYoutubeId(convertedUrl).Returns(testyoutubeId);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);
            videosController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await videosController.AddVideo(nullVideo);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var retrunValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, retrunValue);
        }

        [Fact]
        public async void AddVideo_CalledAndRoleIsNotAdmin_ReturnsBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testVideo = new VideoViewModel();
            ModuleContent nullModuleContent = null;
            string testUrl = "testurl";
            string convertedUrl = "converted";
            string testyoutubeId = "youtubeid";
            string error = "Only Admins can add videos.";

            unitOfWork.ModuleContents.GetById(testId).Returns(nullModuleContent);
            videoManager.ConvertUrl(testUrl).Returns(convertedUrl);
            videoManager.GetYoutubeId(convertedUrl).Returns(testyoutubeId);
            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Student.ToString());

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);
            videosController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await videosController.AddVideo(testVideo);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, returnValue);
        }

        [Fact]
        public async void DeleteVideo_CalledWithValidId_ReturnsOk()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testVideo = new Video();

            unitOfWork.Videos.GetById(testId).Returns(testVideo);

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);
            videosController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await videosController.DeleteVideo(testId);
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void DeleteVideo_CalledWithInvalidId_ReturnsNotFoundWithId()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long notValidId = 1;
            Video nullVideo = null;

            unitOfWork.Videos.GetById(notValidId).Returns(nullVideo);

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Admin.ToString());

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);
            videosController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await videosController.DeleteVideo(notValidId);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<long>(notFoundObjectResult.Value);

            Assert.Equal(notValidId, returnValue);
        }

        [Fact]
        public async void DeleteVideo_CalledAndRoleIsNotAdmin_ReturnsBadRequestWithError()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            IVideoManager videoManager = Substitute.For<IVideoManager>();
            ICookieManager cookieManager = Substitute.For<ICookieManager>();
            var context = Substitute.For<HttpContext>();

            long testId = 1;
            var testVideo = new Video();
            string error = "Only Admins can delete videos.";

            unitOfWork.Videos.GetById(testId).Returns(testVideo);

            cookieManager.GetRoleFromToken(Arg.Any<string>()).Returns(Role.Student.ToString());

            var videosController = new VideosController(unitOfWork, videoManager, cookieManager);
            videosController.ControllerContext = new ControllerContext() { HttpContext = context };

            var result = await videosController.DeleteVideo(testId);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var retrunValue = Assert.IsType<string>(badRequestObjectResult.Value);

            Assert.Equal(error, retrunValue);
        }
    }
}
