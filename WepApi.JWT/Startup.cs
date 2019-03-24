using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.DbModels;
using Helper.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebSite.Context;

namespace WepApi.JWT
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);


         
            services.AddDbContext<testDBContext>(options =>
                         options.UseSqlServer(Configuration.GetConnectionString("ApplicationUser")));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<testDBContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
     .AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.RequireHttpsMetadata = true;
         options.Events = new JwtBearerEvents
         {
             OnAuthenticationFailed = context =>
             {
                 Console.WriteLine("OnAuthenticationFailed: " +
                     context.Exception.Message);
                 return Task.CompletedTask;
             },
             OnTokenValidated = context =>
             {
                 Console.WriteLine("OnTokenValidated: " +
                     context.SecurityToken);
                 return Task.CompletedTask;
             }

             //OnTokenValidated = context =>
             //{
             //    // Add the access_token as a claim, as we may actually need it
             //    var accessToken = context.SecurityToken as JwtSecurityToken;
             //    if (accessToken != null)
             //    {
             //        ClaimsIdentity identity = context.Result.Principal.Identity as ClaimsIdentity;
             //        if (identity != null)
             //        {
             //            identity.AddClaim(new Claim("access_token", accessToken.RawData));
             //        }
             //    }

             //    return Task.CompletedTask;
             //}
         };
         options.TokenValidationParameters = new TokenValidationParameters()
         {
 
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = "Fiver.Security.Bearer",
                            ValidAudience = "Fiver.Security.Bearer",
                            IssuerSigningKey = 
                                  JwtSecurityKey.Create("251a905e08844317a5ea47ab0f6ea2dd")
         };
     });
         
        }


       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id}");
            });
        
       //app.UseSwaggerUI(c =>
       //{
       //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core RESTful API v1");
       //    c.RoutePrefix = "swagger/ui";
       //});
        }
    }
}
