using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.ViewModels;
using ELearningV1._3._1.Enums;

namespace ELearningV1._3._1.Controllers
{
    [Route("Videos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VideosController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private readonly IVideoManager _videoManager;
        private readonly ICookieManager _cookieManager;

        public VideosController(IUnitOfWork repository, IVideoManager videoManager, ICookieManager cookieManager)
        {
            _repository = repository;
            _videoManager = videoManager;
            _cookieManager = cookieManager;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllVideos()
        {
            var Videos = await _repository.Videos.GetAll();

            return Ok(Videos);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetVideo(long Id)
        {
            var Video = await _repository.Videos.GetById(Id);

            if (Video == null)
            {
                return NotFound(Id);
            }

            return Ok(Video);
        }

        [HttpGet("Video_by_module_content/{ContentId}")]
        public async Task<IActionResult> GetVideosByModuleContentId(long ContentId)
        {
            var Videos = await _repository.Videos.GetVideosByModuleContentId(ContentId);

            if (Videos == null)
            {
                return NotFound(ContentId);
            }

            return Ok(Videos);
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo([FromBody] VideoViewModel video)
        {
            if (video == null)
            {
                return BadRequest("Video cannot be null.");
            }

            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var ModuleContent = await _repository.ModuleContents.GetById(video.ModuleContentId);

                if (ModuleContent == null)
                {
                    return NotFound(video.ModuleContentId);
                }

                string embedLink = _videoManager.ConvertUrl(video.Url);
                string YoutubeId = _videoManager.GetYoutubeId(embedLink);
                var Video = new Video { Title = video.Title, Description = video.Description, Url = embedLink, YoutubeId = YoutubeId, ModuleContent = ModuleContent };

                _repository.Videos.Add(Video);
                await _repository.Complete();

                return Ok();
            }
            return BadRequest("Only Admins can add videos.");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteVideo(long Id)
        {
            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var Video = await _repository.Videos.GetById(Id);
                if (Video != null)
                {
                    _repository.Videos.Remove(Video);
                    await _repository.Complete();

                    return Ok();
                }
                return NotFound(Id);
            }
            return BadRequest("Only Admins can delete videos.");

        }
    }
}