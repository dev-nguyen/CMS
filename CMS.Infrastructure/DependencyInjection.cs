using CMS.ApplicationCore.Service;
using CMS.Entity;
using CMS.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CMS.ApplicationCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddDbContext<CMSContext>(options =>
            {
                options.UseSqlServer("Server=.;Database=CMS;uid=sa;pwd=Admin@123");
            });
            service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IAuthService, AuthService>();

            service.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;

                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<CMSContext>()
                .AddDefaultTokenProviders();

            service.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.Cookie.Name = "Default Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
            });

            service.AddAuthentication().AddGoogle(opts =>
            {
                opts.ClientId = "179941545130-ed22lf39347lr58dmm8597kqbacghine.apps.googleusercontent.com";
                opts.ClientSecret = "y66kQdNb5-vp-XGzQnGPDh7Y";
                opts.SignInScheme = IdentityConstants.ExternalScheme;
            });

            return service;
        }
    }
}
