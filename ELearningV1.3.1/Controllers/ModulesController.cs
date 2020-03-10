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
    [Route("Modules")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ModulesController : ControllerBase
    {
        private readonly ApiContext _context;

        public ModulesController(ApiContext apiContext)
        {
            _context = apiContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ModulesDb = await _context.Modules.ToListAsync();
            var modules = ModulesDb.Select(m => new Module { Id = m.Id, Title = m.Title, Description = m.Description });

            return new JsonResult(modules);
            //Seed();
            //return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleContent(int id)
        {
            var moduleContents = await _context.ModuleContents.Where(m => m.Module.Id.Equals(id)).Select(m => new ModuleContent
            {
                Id = m.Id,
                ContentId = m.ContentId,
                Title = m.Title,
                Description = m.Description,
                Lesson = m.Lesson,
                TutorialUrl = m.TutorialUrl,
                AssigmentUrl = m.AssigmentUrl
            }).ToArrayAsync();

            return new JsonResult(moduleContents);
        }
        private void Seed()
        {
            var module0 = new Module { Title = "Programing Basics", Description = "Learn the essential programming concepts of variables, operators, and data types." };
            var module1 = new Module { Title = "Web technologies", Description = "Understanding the web" };
            var module2 = new Module { Title = "OOP", Description = "Yet another language to master: Java" };
            var module3 = new Module { Title = "Advanced", Description = "Free your mind" };
            var moduleContent = new ModuleContent
            {
                Module = module0,
                ContentId = 1,
                Title = "What is Computer Programming ?",
                Description = "Basic of programming",
                Lesson = "English is the most popular and well-known Human Language. The English language has its own set of grammar rules," +
                " which has to be followed to write in the English language correctly." +
                "Likewise,any other Human Languages(German, Spanish, Russian, etc.) are made of several elements like nouns," +
                "adjective, adverbs, propositions, and conjunctions, etc.So, just like English, Spanish or other human languages," +
                "programming languages are also made of different elements. Just like human languages, programming languages also " +
                "follow grammar called syntax.There are certain basic program code elements which are common for all the programming languages.",
                TutorialUrl = "https://www.youtube.com/embed/bJzb-RuUcMU"
            };
            var moduleContent2 = new ModuleContent
            {
                Module = module0,
                ContentId = 2,
                Title = "What is a Variable in Python ?",
                Description = "A Python variable is a reserved memory location to store values.In other words...",
                Lesson = "A Python variable is a reserved memory location to store values. In other words, a variable" +
                " in a python program gives data to the computer for processing. " +
                "Every value in Python has a datatype.Different data types in Python are Numbers," + " List, Tuple, Strings, Dictionary," +
                " etc.Variables can be declared by any name or even alphabets like a, aa, abc, etc.",
                TutorialUrl = "https://www.youtube.com/embed/olH6T7iicQQ"
            };
            var moduleContent3 = new ModuleContent
            {
                Module = module0,
                ContentId = 3,
                Title = "What are functions ?",
                Description = "Functions in Python",
                Lesson = "A function is a set of statements that take inputs, do some specific computation and produces output." +
                " The idea is to put some commonly or repeatedly done task together and make a function, so that instead of writing" +
                " the same code again and again for different inputs, we can call the function. Python provides built -in functions like print()," +
                " etc.but we can also create your own functions.These functions are called user - defined functions.",
                TutorialUrl = "https://www.youtube.com/embed/9Os0o3wzS_I"
            };
            var moduleContent4 = new ModuleContent
            {
                Module = module0,
                ContentId = 4,
                Title = "HackerRank",
                Description = "HackerRank is one of the most popular coding challenge sites in the IT world. It has a huge, free repository of katas.",
                Lesson = "HackerRank is one of the most popular coding challenge sites in the IT world. It has a huge, free repository of katas similar " +
                "to CodeWars. The main difference is that the exercises are organized into learning tracks. Programmers both from Poland (3.) and Hungary " +
                "(5.) are top performers on HackerRank according to this 2016 study. The company also has a HackerRank for Work platform used by companies" +
                " for their hiring processes.We are using this for an automatic and instant feedback tool measuring your advancement.The main unit on this platform" +
                " is called test. A test is a collection of a few exercises with a URL and a maximum time frame to finish the test. In case you do not have an account" +
                " yet, please create it now.",
                TutorialUrl = "https://www.youtube.com/embed/cKrZCa95Uw0",
                AssigmentUrl = "https://www.hackerrank.com/"

            };
            var moduleContent5 = new ModuleContent
            {
                Module = module0,
                ContentId = 5,
                Title = "Install a Python interpreter",
                Description = "There is a chance that you already have one on your computer...",
                Lesson = "There is a chance that you already have one on your computer Linux machines and macOS have" +
                " it in the core system. However, there is a caveat: we need Python version 3. If you dont have an interpreter please Follow this tutorial.",
                TutorialUrl = "https://www.youtube.com/embed/LwORmcaI69w"
            };
            var moduleContent6 = new ModuleContent
            {
                Module = module0,
                ContentId = 6,
                Title = "Install Python",
                Description = "Installing Python",
                Lesson = "And finally, in case you'd need to install Python on your system, check for example this guide.",
                AssigmentUrl = "https://realpython.com/installing-python/"
            };
            var moduleContent7 = new ModuleContent
            {
                Module = module0,
                ContentId = 7,
                Title = "Install a package manager for Python",
                Description = "Packet Manager",
                Lesson = "If you have Python, most probably you also have pip installed as the default package manager." +
                " With this you can install Python packages (which is a collections of modules, also called as libraries or" +
                " third-party extensions) on your computer from a central repository called PyPI. The syntax for this is pip" +
                " install SomePackage. Again, you should be aware of the possible version conflict mentioned above; check pip" +
                " --version and pip3 --version, and use the appropriate v3 version every time you install a package! On unix" +
                " - based systems packages are probably stored somewhere under / usr / lib / which means that you need root privileges" +
                " or use sudo to add a package.One solution to avoid this to install packages into the user space by using the --user" +
                " flag: pip install --user SomePackage, although generally the best practice is to use virtual environments."

            };
            var moduleContent8 = new ModuleContent
            {
                Module = module0,
                ContentId = 8,
                Title = "Assignment: Hello World",
                Description = "Create a script file that welcomes the user given to it. If no name was given, then it welcomes the whole world.",
                Lesson = "Python is a very simple language, and has a very straightforward syntax. It encourages programmers to program without" +
                " boilerplate (prepared) code. The simplest directive in Python is the 'print' directive - it simply prints out a line" +
                " (and also includes a newline, unlike in C). There are two major Python versions, Python 2 and Python 3.Python 2 and 3" +
                " are quite different.This tutorial uses Python 3, because it more semantically correct and supports newer features." +
                " For example, one difference between Python 2 and 3 is the print statement.In Python 2, the 'print' statement is not" +
                " a function, and therefore it is invoked without parentheses.However, in Python 3, it is a function, and must be invoked with parentheses." +
                " To print a string in Python 3, just write: print(\"This line will be printed.\")",
            };

            _context.Modules.Add(module0);
            _context.Modules.Add(module1);
            _context.Modules.Add(module2);
            _context.Modules.Add(module3);
            _context.ModuleContents.Add(moduleContent);
            _context.ModuleContents.Add(moduleContent2);
            _context.ModuleContents.Add(moduleContent3);
            _context.ModuleContents.Add(moduleContent4);
            _context.ModuleContents.Add(moduleContent5);
            _context.ModuleContents.Add(moduleContent6);
            _context.ModuleContents.Add(moduleContent7);
            _context.ModuleContents.Add(moduleContent8);
            _context.SaveChanges();
        }

    }
}