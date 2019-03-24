using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Common.DbModels;
using Helper.JWT;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSite.Context;

namespace JWT
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

            // Ayağa kalkacak serviceler tanımlandı  
            ConfigureSingletonServices(services);
            ConfigureScopedServices(services);
            // session kullanılacağı belirtildi    
            services.AddSession();
            // Authentication işlemi gerçekleşmediyse gönderilecek url belirtildi.    
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = "/Account/Login";
                    }
                );

            services.AddDbContext<testDBContext>(options =>
 options.UseSqlServer(Configuration.GetConnectionString("ApplicationUser")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<testDBContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();
         
            services.AddAuthorization(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder(new[] {
                    CookieAuthenticationDefaults.AuthenticationScheme,
                })
                .RequireAuthenticatedUser()
                .Build();

                options.DefaultPolicy = defaultPolicy;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });


        }



        private void ConfigureSingletonServices(IServiceCollection services)
        {
            services.AddSingleton(c => new HttpClient() { Timeout = TimeSpan.FromDays(1) });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
        private void ConfigureScopedServices(IServiceCollection services)
        {
            services.AddScoped<WebUserManager>();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
