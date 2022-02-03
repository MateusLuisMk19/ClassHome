using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassHome.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClassHome
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDbContext<ClassHomedbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("ClassHomedbContext")));

            services.AddIdentity<UserModel, IdentityRole<int>>(options =>
                {
                    options.User.RequireUniqueEmail = true; //false
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //idem
                    options.Password.RequireNonAlphanumeric = false; //true
                    options.Password.RequireUppercase = false; //true;
                    options.Password.RequireLowercase = false; //true;
                    options.Password.RequireDigit = false; //true;
                    options.Password.RequiredUniqueChars = 1; //1;
                    options.Password.RequiredLength = 6; //6;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); //5
                    options.Lockout.MaxFailedAccessAttempts = 5; //5
                    options.Lockout.AllowedForNewUsers = true; //true		
                    options.SignIn.RequireConfirmedEmail = false; //false
                    options.SignIn.RequireConfirmedPhoneNumber = false; //false
                    options.SignIn.RequireConfirmedAccount = false; //false
                })
                .AddEntityFrameworkStores<ClassHomedbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "ClassHomeRequest"; //AspNetCore.Cookies
                options.Cookie.HttpOnly = true; //true
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5); //14 dias
                options.LoginPath = "/User/Login"; // /Account/Login
                options.LogoutPath = "/Home/Index";  // /Account/Logout
                options.AccessDeniedPath = "/User/AcessoRestrito"; // /Account/AccessDenied
                options.SlidingExpiration = true; //true - gera um novo cookie a cada requisição se o cookie estiver com menos de meia vida
                options.ReturnUrlParameter = "returnUrl"; //returnUrl
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
        UserManager<UserModel> userManager, RoleManager<IdentityRole<int>> roleManager, ClassHomedbContext _context)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Turma}/{action=Index}/{id?}");
            });
            Inicializador.InicializarIdentity(userManager, roleManager, _context);
        }
    }
}
