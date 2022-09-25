using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendR;
using SimplySoft.Core.SendR.Enumerators;

namespace SimplySoft.Core.Test.WebUI
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
            services.AddControllersWithViews();

            services.ConfigureSendRNotifications(Configuration)
                .AddEmail(opt =>
                {
                    var appRoot = AppDomain.CurrentDomain.BaseDirectory;
                    opt.Host = "mail";
                    opt.Port = 000;
                    opt.Username = "example@mail.com";
                    opt.Password = "password";
                    opt.BusinessName = "SendR";
                    opt.EnableSsl = true;
                    opt.TimeOut = 60000;
                    opt.SendingFailedFallbackMode = FallbackActionMode.ThrowException;
                    opt.AddTemplate("welcome", "Welcome to SendR", $"{appRoot}/EmailTemplates/Welcome.html");
                    opt.AddTemplate("send-wish", "Happy Birthday", $"{appRoot}/EmailTemplates/Send-Wish.html");
                }).Build();
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
