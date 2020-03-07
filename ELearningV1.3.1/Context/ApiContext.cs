using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ELearningV1._3._1.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ELearningV1._3._1.Context
{
    public class ApiContext : IdentityDbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
        public DbSet<User> UsersT { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Assigment> Assigments { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<ModuleContent> ModuleContents { get; set; }

    }
}
