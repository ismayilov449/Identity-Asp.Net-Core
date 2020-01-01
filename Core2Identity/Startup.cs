using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Infrastructure;
using Core2Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core2Identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CoreIdentityDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

            //services.ConfigureApplicationCookie(opt => opt.LoginPath = "/Member/Login");
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                       {

                           options.User.RequireUniqueEmail = true;
                           options.Password.RequiredLength = 7;
                           options.Password.RequireLowercase = false;
                           options.Password.RequireUppercase = false;
                           options.Password.RequireNonAlphanumeric = false;
                           options.Password.RequireDigit = false;



                       })
                           .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                           .AddDefaultTokenProviders();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "default",
                     template: "{controller}/{action}/{id?}",
                     defaults: new { controller = "Home", action = "Index" }
                     );


            });


        }
    }
}
