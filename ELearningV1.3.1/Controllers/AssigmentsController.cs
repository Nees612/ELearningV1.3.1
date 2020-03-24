using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ELearningV1._3._1.Repositories;
using ELearningV1._3._1.Units;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.ViewModels;

namespace ELearningV1._3._1.Controllers
{
    [Route("Assigments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssigmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssigmentsController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAssigments()
        {

            var Assigments = await _unitOfWork.Assigments.GetAll();

            return Ok(new { assigments = Assigments });

        }

        [HttpGet("{moduleName}")]
        public async Task<IActionResult> GetAssigmentsByModule(string moduleName)
        {
            var Assigments = await _unitOfWork.Assigments.GetAssigmentsByModuleName(moduleName);

            return Ok(new { assigments = Assigments });
        }

        [HttpGet("Random/{moduleName}")]
        public async Task<IActionResult> GetRandomAssigment(string moduleName)
        {
            Random rnd = new Random();

            var Assigments = await _unitOfWork.Assigments.GetAssigmentsByModuleName(moduleName);
            var randomUrl = Assigments.ElementAt(rnd.Next(0, Assigments.Count())).Url;

            return Ok(new { randomAssigmentUrl = randomUrl });
        }

        [HttpPost]
        public async Task<IActionResult> AddAssigment([FromBody] AssigmentViewModel assigment)
        {
            var Module = _unitOfWork.Modules.Get(assigment.ModuleId);
            var Assigment = new Assigment { Title = assigment.Title, Description = assigment.Description, Url = assigment.Url, Module = Module };

            _unitOfWork.Assigments.Add(Assigment);
            await _unitOfWork.Complete();
            
            return Ok();
        }

    }
}