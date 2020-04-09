using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.ViewModels;
using ELearningV1._3._1.Models;
using ELearningV1._3._1.Enums;
using System.Linq;

namespace ELearningV1._3._1.Controllers
{
    [Route("ModuleContents")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ModuleContentsController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private readonly ICookieManager _cookieManager;
        public ModuleContentsController(IUnitOfWork unitOfWork, ICookieManager cookieManager)
        {
            _repository = unitOfWork;
            _cookieManager = cookieManager;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetModuleContentByModuleId(long Id)
        {
            var ModuleContents = (await _repository.ModuleContents.GetModuleContentsByModuleId(Id)).OrderBy(mc => mc.ContentId);

            if (ModuleContents == null)
            {
                return NotFound(Id);
            }

            return Ok(ModuleContents);
        }
        [HttpGet("AllModuleContents")]
        public async Task<IActionResult> GetAllModuleContents()
        {
            var ModuleContents = (await _repository.ModuleContents.GetAll()).OrderBy(mc => mc.ContentId);

            return Ok(ModuleContents);
        }
        [HttpPost]
        public async Task<IActionResult> AddModuleContent([FromBody] ModuleContentViewModel ModuleContent)
        {
            if (ModuleContent == null)
            {
                return BadRequest("Module content cannot be null.");
            }

            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var Module = await _repository.Modules.GetById(ModuleContent.ModuleId);

                if (Module == null)
                {
                    return NotFound(ModuleContent.ModuleId);
                }

                var NextContentId = (await _repository.ModuleContents.GetLastModuleContentByModuleId(ModuleContent.ModuleId)) + 1;
                var NewModuleContent = new ModuleContent()
                {
                    Title = ModuleContent.Title,
                    Description = ModuleContent.Description,
                    Module = Module,
                    ContentId = NextContentId,
                    AssigmentUrl = ModuleContent.AssigmentUrl,
                    Lesson = ModuleContent.Lesson

                };

                _repository.ModuleContents.Add(NewModuleContent);
                await _repository.Complete();

                return Ok();
            }

            return BadRequest("Only Admins can add new module contents.");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteModuleContent(long Id)
        {
            var UserRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (UserRole.Equals(Role.Admin.ToString()))
            {
                var ModuleContent = await _repository.ModuleContents.GetById(Id);
                var Videos = await _repository.Videos.GetVideosByModuleContentId(Id);

                if (ModuleContent == null)
                {
                    return NotFound(Id);
                }

                if (Videos != null)
                {
                    _repository.Videos.RemoveRange(Videos);
                    await _repository.Complete();
                }

                _repository.ModuleContents.Remove(ModuleContent);
                await _repository.Complete();

                return Ok();
            }

            return BadRequest("Only Admins can delete module contents.");
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateModuleContent([FromBody]ModuleContentUpdateViewModel moduleContent, long Id)
        {
            if (moduleContent == null)
            {
                return BadRequest("Module content cannot be null.");
            }

            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var ModuleContent = await _repository.ModuleContents.GetById(Id);

                if (ModuleContent == null)
                {
                    return NotFound(Id);
                }

                ModuleContent.Title = moduleContent.Title;
                ModuleContent.Description = moduleContent.Description;
                ModuleContent.AssigmentUrl = moduleContent.AssigmentUrl;
                ModuleContent.Lesson = moduleContent.Lesson;

                _repository.ModuleContents.Update(ModuleContent);
                await _repository.Complete();

                return Ok();
            }

            return BadRequest("Only Admins can update module content.");
        }

        [HttpPut("ChangeOrder/{Id}")]
        public async Task<IActionResult> ChangeModuleContentOrder([FromBody] long contentId, long Id)
        {
            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);
            if (userRole.Equals(Role.Admin.ToString()))
            {
                var ModuleContent = await _repository.ModuleContents.GetById(Id);

                if (ModuleContent == null)
                {
                    return NotFound(Id);
                }

                ModuleContent.ContentId = contentId;

                _repository.ModuleContents.Update(ModuleContent);
                await _repository.Complete();

                return Ok();
            }
            return BadRequest("Only Admins can change content order");

        }
    }
}