using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using ELearningV1._3._1.Enums;

namespace ELearningV1._3._1.Controllers
{
    [Route("Modules")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ModulesController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private readonly ICookieManager _cookieManager;

        public ModulesController(IUnitOfWork repository, ICookieManager cookieManager)
        {
            _repository = repository;
            _cookieManager = cookieManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Modules = await _repository.Modules.GetAll();

            return Ok(Modules);
        }

        [HttpPost]
        public async Task<IActionResult> AddModule([FromBody] Module module)
        {
            if (module == null)
            {
                return BadRequest("Module cannot be null.");
            }

            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                _repository.Modules.Add(module);
                await _repository.Complete();
                return Ok();
            }

            return BadRequest("Only admins can add modules.");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteModule(long Id)
        {
            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var Module = await _repository.Modules.GetById(Id);

                if (Module != null)
                {
                    var ModuleContents = await _repository.ModuleContents.GetModuleContentsByModuleId(Id);

                    var ModuleAssigments = await _repository.Assigments.GetAssigmentsByModuleName(Module.Title);
                    _repository.Assigments.RemoveRange(ModuleAssigments);

                    foreach (var ModuleContent in ModuleContents)
                    {
                        var ContentVideos = await _repository.Videos.GetVideosByModuleContentId(ModuleContent.Id);


                        if (ContentVideos != null)
                        {
                            _repository.Videos.RemoveRange(ContentVideos);
                        }
                    }

                    _repository.ModuleContents.RemoveRange(ModuleContents);
                    _repository.Modules.Remove(Module);

                    await _repository.Complete();

                    return Ok();
                }
                return NotFound(Id);
            }
            return BadRequest("Only Admins can delete modules.");
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateModule([FromBody] Module module, long Id)
        {
            if (module == null)
            {
                return BadRequest("Module cannot be null.");
            }

            var UserId = _cookieManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var userRole = (await _repository.Users.Get(u => u.Id.Equals(UserId))).Role;

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var Module = await _repository.Modules.GetById(Id);

                if (Module == null)
                {
                    return NotFound(Id);
                }

                Module.Title = module.Title;
                Module.Description = module.Description;

                _repository.Modules.Update(Module);
                await _repository.Complete();

                return Ok();
            }

            return BadRequest("Only Admins can update modules");
        }
    }
}