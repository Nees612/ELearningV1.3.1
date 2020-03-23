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

            //await Seed();
            //return Ok();
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


        //private async Task Seed()
        //{
        //    var modulesT = await _unitOfWork.Modules.GetAll();
        //    var prgbasicM = modulesT.First(m => m.Title.Equals("Programing Basics"));
        //    var webM = modulesT.First(m => m.Title.Equals("Web technologies"));
        //    var oopM = modulesT.First(m => m.Title.Equals("OOP"));
        //    var advancedM = modulesT.First(m => m.Title.Equals("Advanced"));

        //    _context.Assigments.Add(new Assigment { Title = "Say 'Hello, World!' With Python", Description = "A simple 'Hello World'", Url = "https://www.hackerrank.com/challenges/py-hello-world/problem", Module = prgbasicM });
        //    _context.Assigments.Add(new Assigment { Title = "Python If-Else", Description = "If-Else... ", Url = "https://www.hackerrank.com/challenges/py-if-else/problem", Module = prgbasicM });
        //    _context.Assigments.Add(new Assigment { Title = "Arithmetic Operators", Description = "Operators are usefull", Url = "https://www.hackerrank.com/challenges/python-arithmetic-operators/problem", Module = prgbasicM });
        //    _context.Assigments.Add(new Assigment { Title = "String Split and Join", Description = "String manipulations", Url = "https://www.hackerrank.com/challenges/python-string-split-and-join/problem", Module = prgbasicM });
        //    _context.Assigments.Add(new Assigment { Title = "Print Function", Description = "Learn how to write", Url = "https://www.hackerrank.com/challenges/python-print/problem", Module = prgbasicM });
        //    _context.Assigments.Add(new Assigment { Title = "Loops", Description = "For, While make it easier", Url = "https://www.hackerrank.com/challenges/python-loops/problem", Module = prgbasicM });

        //    _context.Assigments.Add(new Assigment { Title = "Flask – (Creating first simple application)", Description = "Building an webpage using python.", Url = "https://www.geeksforgeeks.org/flask-creating-first-simple-application/", Module = webM });
        //    _context.Assigments.Add(new Assigment { Title = "Building a Todo App with Flask in Python", Description = "We are going to build an API, or a web service, for a todo app.", Url = "https://stackabuse.com/building-a-todo-app-with-flask-in-python/", Module = webM });
        //    _context.Assigments.Add(new Assigment { Title = "Building a simple REST API with Python and Flask", Description = "Building a simple web API", Url = "https://medium.com/@onejohi/building-a-simple-rest-api-with-python-and-flask-b404371dc699", Module = webM });
        //    _context.Assigments.Add(new Assigment { Title = "SELECT queries", Description = "Select queries introduction", Url = "https://sqlbolt.com/lesson/select_queries_introduction", Module = webM });
        //    _context.Assigments.Add(new Assigment { Title = "Queries with constraints (Pt. 1)", Description = "Select queries with constraints", Url = "https://sqlbolt.com/lesson/select_queries_with_constraints", Module = webM });
        //    _context.Assigments.Add(new Assigment { Title = "Queries with constraints (Pt. 2)", Description = "Select queries with constraints", Url = "https://sqlbolt.com/lesson/select_queries_with_constraints_pt_2", Module = webM });

        //    _context.Assigments.Add(new Assigment { Title = "Java Hello World", Description = "Your new friend.", Url = "https://practice.geeksforgeeks.org/problems/java-hello-world/0", Module = oopM });
        //    _context.Assigments.Add(new Assigment { Title = "Java Classes Introduction", Description = "Learn new concepts.", Url = "https://practice.geeksforgeeks.org/problems/java-classes-introduction/1", Module = oopM });
        //    _context.Assigments.Add(new Assigment { Title = "Java Inheritance", Description = "Inheritance is an important pillar of OOP", Url = "https://practice.geeksforgeeks.org/problems/java-inheritance/1", Module = oopM });
        //    _context.Assigments.Add(new Assigment { Title = "Java Abstract keyword", Description = "Abstraction.", Url = "https://practice.geeksforgeeks.org/problems/java-abstract-keyword/1", Module = oopM });
        //    _context.Assigments.Add(new Assigment { Title = "Java Override", Description = "The Override keyword.", Url = "https://practice.geeksforgeeks.org/problems/java-override/1", Module = oopM });
        //    _context.Assigments.Add(new Assigment { Title = "Java Input/Output", Description = "Learn how to Input/Output.", Url = "https://practice.geeksforgeeks.org/problems/java-inputoutput/0", Module = oopM });


        //    _context.SaveChanges();
        //}

        //private async void DeleteSeed()
        //{
        //    var AssigmentsT = await _context.Assigments.ToArrayAsync();
        //    foreach (var assigment in AssigmentsT)
        //    {
        //        _context.Assigments.Remove(assigment);
        //    }

        //    await _context.SaveChangesAsync();
        //}

    }
}