using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace RestaurantWebApp
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                options.LogoutPath = new PathString("/Account/Login");
            }
            );
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            CreateRoles(serviceProvider).Wait();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                    endpoints.MapRazorPages();
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Member" };
            foreach(var roleName in roleNames)
            {
                var roleExists = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var _user = await UserManager.FindByNameAsync("admin@myFood.com");
            if (_user != null)
            {
                await UserManager.AddToRoleAsync(_user, "Admin");
            }
        }
    }
}
