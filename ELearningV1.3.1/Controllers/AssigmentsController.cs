using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ELearningV1._3._1.Context;
using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ELearningV1._3._1.Controllers
{
    [Route("Assigments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssigmentsController : ControllerBase
    {
        private ApiContext _context;

        public AssigmentsController(ApiContext apiContext)
        {
            _context = apiContext;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAssigments()
        {
            //Seed();
            //return Ok();
            var AssigmentT = await _context.Assigments.ToArrayAsync();
            return Ok(new { assigments = AssigmentT });
        }


        private void Seed()
        {
            _context.Assigments.Add(new Assigment { Title = "Say 'Hello, World!' With Python", Description = "A simple 'Hello World'", Url = "https://www.hackerrank.com/challenges/py-hello-world/problem" });
            _context.Assigments.Add(new Assigment { Title = "Python If-Else", Description = "If-Else... ", Url = "https://www.hackerrank.com/challenges/py-if-else/problem" });
            _context.Assigments.Add(new Assigment { Title = "Arithmetic Operators", Description = "Operators are usefull", Url = "https://www.hackerrank.com/challenges/python-arithmetic-operators/problem" });
            _context.Assigments.Add(new Assigment { Title = "String Split and Join", Description = "String manipulations", Url = "https://www.hackerrank.com/challenges/python-string-split-and-join/problem" });
            _context.Assigments.Add(new Assigment { Title = "Print Function", Description = "Learn how to write", Url = "https://www.hackerrank.com/challenges/python-print/problem" });
            _context.Assigments.Add(new Assigment { Title = "Loops", Description = "For, While make it easier", Url = "https://www.hackerrank.com/challenges/python-loops/problem" });

            _context.SaveChangesAsync();
        }

    }
}