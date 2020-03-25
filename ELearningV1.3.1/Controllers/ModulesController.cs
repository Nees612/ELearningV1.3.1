using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Units;

namespace ELearningV1._3._1.Controllers
{
    [Route("Modules")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ModulesController : ControllerBase
    {
        private readonly IUnitOfWork _repository;

        public ModulesController(IUnitOfWork repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Modules = await _repository.Modules.GetAll();

            return Ok(new { modules = Modules });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleContentByModuleId(int id)
        {
            var ModuleContents = await _repository.ModuleContents.GetModuleContentsByModuleId(id);

            return Ok(new { moduleContents = ModuleContents });
        }
        [HttpGet("AllModuleContents")]
        public async Task<IActionResult> GetAllModuleContents()
        {
            var ModuleContents = await _repository.ModuleContents.GetAll();

            return Ok(new { contents = ModuleContents });
        }


    }
}