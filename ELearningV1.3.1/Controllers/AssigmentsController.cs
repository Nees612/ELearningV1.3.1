using System;
using System.Linq;
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
    [Route("Assigments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssigmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICookieManager _cookieManager;

        public AssigmentsController(IUnitOfWork unitOfWork, ICookieManager cookieManager)
        {
            _unitOfWork = unitOfWork;
            _cookieManager = cookieManager;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAssigments()
        {
            var Assigments = await _unitOfWork.Assigments.GetAll();

            return Ok(Assigments);
        }

        [HttpGet("{moduleName}")]
        public async Task<IActionResult> GetAssigmentsByModule(string moduleName)
        {
            if (moduleName == null)
            {
                return BadRequest("Module name cannot be null.");
            }

            var Assigments = await _unitOfWork.Assigments.GetAssigmentsByModuleName(moduleName);

            return Ok(Assigments);
        }

        [HttpGet("Random/{moduleName}")]
        public async Task<IActionResult> GetRandomAssigment(string moduleName)
        {

            if (moduleName == null)
            {
                return BadRequest("Module name cannot be null.");
            }

            Random rnd = new Random();

            var Assigments = await _unitOfWork.Assigments.GetAssigmentsByModuleName(moduleName);
            if (Assigments != null)
            {
                var RandomAssigment = Assigments.ElementAt(rnd.Next(0, Assigments.Count()));
                return Ok(RandomAssigment);
            }
            return NotFound(moduleName);
        }

        [HttpPost]
        public async Task<IActionResult> AddAssigment([FromBody] AssigmentViewModel assigment)
        {
            if (assigment == null)
            {
                return BadRequest("Assigment cannot be null.");
            }

            var Module = await _unitOfWork.Modules.GetById(assigment.ModuleId);
            if (Module != null)
            {
                var Assigment = new Assigment { Title = assigment.Title, Description = assigment.Description, Url = assigment.Url, Module = Module };
                _unitOfWork.Assigments.Add(Assigment);
                await _unitOfWork.Complete();
                return Ok();

            }
            return NotFound(assigment.ModuleId);


        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAssigment(long Id)
        {
            var userRole = _cookieManager.GetRoleFromToken(Request.Headers["Authorization"]);

            if (userRole.Equals(Role.Admin.ToString()))
            {
                var Assigment = await _unitOfWork.Assigments.GetById(Id);

                if (Assigment != null)
                {
                    _unitOfWork.Assigments.Remove(Assigment);
                    await _unitOfWork.Complete();
                    return Ok();
                }
                return NotFound(Id);
            }
            return BadRequest("Only Admins can delete assigments.");
        }
    }
}