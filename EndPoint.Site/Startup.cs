using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Chatrooms.FacadPattern;
using MatinGram.Application.Services.Messages.FacadPattern;
using MatinGram.Application.Services.Users.FacadPattern;
using MatinGram.Common.Enums;
using MatinGram.Persistace.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site
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

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Signin");
                options.AccessDeniedPath = new PathString("/");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200.0);
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole(UserInRole.Admin.ToString()));
                options.AddPolicy("User", policy => policy.RequireRole(UserInRole.User.ToString(), UserInRole.Admin.ToString()));
            });

            #region --Add DependencyInjection Service--
            //Add DataBase Service
            services.AddScoped<IDataBaseContext, DataBaseContext>();

            //Add Facad Patterns
            services.AddScoped<IUsersFacad, UsersFacad>();
            services.AddScoped<IMessagesFacad, MessagesFacad>();
            services.AddScoped<IChatroomsFacad, ChatroomsFacad>();


            #endregion


            #region --ConnectionString--
            string contectionString = Configuration.GetConnectionString("SqlServer");

            services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(contectionString));
            #endregion

            services.AddControllersWithViews();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
