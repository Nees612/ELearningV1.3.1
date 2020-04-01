using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.ViewModels;

namespace ELearningV1._3._1.Controllers
{
    [Route("Videos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VideosController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private readonly IVideoManager _videoManager;

        public VideosController(IUnitOfWork repository, IVideoManager videoManager)
        {
            _repository = repository;
            _videoManager = videoManager;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllVideo()
        {
            var Videos = await _repository.Videos.GetAll();

            return Ok(new { videos = Videos });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetVideo(long Id)
        {
            var Video = await _repository.Videos.GetById(Id);

            return Ok(new { video = Video });
        }

        [HttpGet("Video_by_module_content/{ContentId}")]
        public async Task<IActionResult> GetVideosByModuleContentId(long ContentId)
        {
            var Videos = await _repository.Videos.GetVideosByModuleContentId(ContentId);

            return Ok(new { videos = Videos });
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo([FromBody] VideoViewModel video)
        {
            var ModuleContent = await _repository.ModuleContents.GetById(video.ModuleContentId);
            string embedLink = _videoManager.ConvertUrl(video.Url);
            string YoutubeId = _videoManager.GetYoutubeId(embedLink);
            var Video = new Video { Title = video.Title, Description = video.Description, Url = embedLink, YoutubeId = YoutubeId, ModuleContent = ModuleContent };

            _repository.Videos.Add(Video);
            await _repository.Complete();

            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleVideo(long Id)
        {
            var Video = await _repository.Videos.GetById(Id);

            _repository.Videos.Remove(Video);
            await _repository.Complete();

            return Ok();
        }
    }
}