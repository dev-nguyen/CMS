using CMS.ApplicationCore.Service;
using CMS.Entity;
using CMS.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CMS.ApplicationCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddHttpContextAccessor();
            service.AddDbContext<CMSContext>(options =>
            {
                options.UseSqlServer("Server=.;Database=Test;uid=sa;pwd=Admin@123");
            });
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
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
                options.Cookie.Name = "Default_Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
            });
            service.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));

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
