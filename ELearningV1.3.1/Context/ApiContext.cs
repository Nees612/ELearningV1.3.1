using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ELearningV1._3._1.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearningV1._3._1.Context
{
    public class ApiContext : IdentityDbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }
        public DbSet<User> UsersT { get; set; }
    }
}
