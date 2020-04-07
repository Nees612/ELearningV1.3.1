using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Interfaces;

namespace ELearningV1._3._1.Controllers
{
    [Route("ModuleContents")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ModuleContentsController : ControllerBase
    {
        private readonly IUnitOfWork _repository;

        public ModuleContentsController(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetModuleContentByModuleId(long Id)
        {
            var ModuleContents = await _repository.ModuleContents.GetModuleContentsByModuleId(Id);

            if (ModuleContents == null)
            {
                return NotFound(Id);
            }

            return Ok(ModuleContents);
        }
        [HttpGet("AllModuleContents")]
        public async Task<IActionResult> GetAllModuleContents()
        {
            var ModuleContents = await _repository.ModuleContents.GetAll();

            return Ok(ModuleContents);
        }
    }
}