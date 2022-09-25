using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendR;
using SimplySoft.Core.SendR.Email;
using SimplySoft.Core.SendR.Enumerators;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SimplySoft.Core.SendR.Test.ConsoleUI
{
    internal class Program
    {
        private static IHost _host;

        static void Main(string[] args)
        {
            try
            {
                BuildStartupConfig();
                var notifications = CreateInstance<NotificationCollection>();

                var data = new { Name = "Sonny", Age = 30, IsMarried = false };
                string to = "me@mail.com";
                EmailMessage.Create(to, "Test.SendR", "Hello there!").AddTo(notifications);
                EmailMessage.Create("greet", to, data).Result.AddTo(notifications);
                
                Task.Run(notifications.SendAllAsync);
            }
            catch (Exception exp)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exp.Message);
                Console.ResetColor();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void BuildStartupConfig()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.ConfigureSendRNotifications(context.Configuration)
                    .AddEmail(options =>
                    {
                        var root = AppDomain.CurrentDomain.BaseDirectory;
                        options.AddTemplate("greet", "Greetings", $@"{root}\EmailTemplates\greetings.html");
                    }).Build();
                }).Build();

            _host = host;
        }

        static TInstance CreateInstance<TInstance>()
        {
            return ActivatorUtilities.CreateInstance<TInstance>(_host.Services);
        }
    }
}
