using CMS.ApplicationCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CMS.Web
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
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllersWithViews();
            services.AddInfrastructure();

            //services.AddAntiforgery(options => options.HeaderName = "ValidationXSRFToken");
            //services.AddAntiforgery(options =>
            //{
            //    // Set Cookie properties using CookieBuilder properties†.
            //    options.FormFieldName = "AntiforgeryFieldname";
            //    options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
            //    options.SuppressXFrameOptionsHeader = false;
            //});


            var refAssembyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            //Load referenced assemblies
            foreach (var asslembyNames in refAssembyNames)
            {
                Assembly.Load(asslembyNames);
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(s=>s.GetName().Name.StartsWith("CMS"));

            foreach(var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                { 
                    if(type.Name.Contains("Service") && !type.IsInterface && !type.IsGenericType)
                    {
                        Type interfaceType = type.GetInterfaces().SingleOrDefault(t => t.IsGenericType == false);
                        if (interfaceType != null)
                        {
                            services.AddScoped(interfaceType, type);
                        }
                    }
                }
            }

        }   

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "areas",
                //    pattern: "{area:exists}/{controller=Catalog}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                   name: "admin",
                   areaName: "Admin",
                   pattern: "Admin/{controller=Category}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
