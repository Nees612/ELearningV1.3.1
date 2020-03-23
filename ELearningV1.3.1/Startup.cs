using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Web.Mvc;
using ELearningV1._3._1.Managers;
using ELearningV1._3._1.Units;
using ELearningV1._3._1.Interfaces;

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

            services.AddScoped<UnitOfWork>();
            services.AddScoped<CookieManager>();

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApiContext>();

            services.AddDbContextPool<ApiContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("ElearningDbConnection")));


            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Audience = "http://localhost:4200/";
                    options.ClaimsIssuer = "http://localhost:4200/";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = true
                    };
                });

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

            app.UseRouting();

            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                await next();
            });

            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc(opt =>
            {
                opt.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id}",
                    defaults: new
                    {
                        controller = "Home",
                        action = "Index",
                        id = UrlParameter.Optional
                    });
            });
        }
    }
}
