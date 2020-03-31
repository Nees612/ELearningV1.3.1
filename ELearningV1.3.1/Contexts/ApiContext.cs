using ELearningV1._3._1.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearningV1._3._1.Contexts
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
        public DbSet<User> UsersT { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Assigment> Assigments { get; set; }

        public DbSet<ModuleContent> ModuleContents { get; set; }

        public DbSet<Video> Videos { get; set; }

    }
}
