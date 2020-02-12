using IdentityServer4.Services;
using IdentityServer4.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ELearningV1._3._1.Context;
using ELearningV1._3._1.Models;

namespace ELearningV1._3._1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
            });

            services.AddDbContextPool<ApiContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("ElearningDbConnection")));

            services.AddIdentity<User, IdentityRole>(opt => opt.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<ApiContext>();
            services.AddMvcCore(opt => { opt.EnableEndpointRouting = false; });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseIdentityServer();

            app.UseMvc();

        }
    }
}
