using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ELearningV1._3._1.Context;
using ELearningV1._3._1.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ELearningV1._3._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuestionsController : ControllerBase
    {
        private readonly ApiContext _context;

        public QuestionsController(ApiContext apiContext)
        {
            _context = apiContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var QuestionsDb = await _context.Questions.Include("User").ToListAsync();
            var questions = QuestionsDb.Select(q => new Question
            {
                Id = q.Id,
                Title = q.Title,
                Body = q.Body,
                User = new User { UserName = q.User.UserName }
            });

            return new JsonResult(questions);

        }
    }
}