using SimplySoft.Core.SendR;
using SimplySoft.Core.SendR.Email;
using SimplySoft.Core.SendR.Enumerators;
using System;
using System.Threading.Tasks;

namespace SimplySoft.Core.Test.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                ConfigureSendR();

                INotificationCollection notification = new NotificationCollection();

                var dataMs = new { Name = "Sachintha Outlook", Age = 29, IsMarried = false };
                EmailMessage.Create("send-wish", "netsachintha@outlook.com", dataMs).Result.AddTo(notification);

                var dataGm = new { Name = "Sachintha Gmail", Age = 29, IsMarried = false };
                EmailMessage.Create("send-wish", "netsachintha@gmail.com", dataGm).Result.AddTo(notification);

                Task.Run(notification.SendAllAsync);
            }
            catch (Exception exp)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exp.Message);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void ConfigureSendR()
        {
            SendRConfigurationBuilder.ConfigureEmailService(opt =>
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
            });

            Console.WriteLine("Configuration successful.");
        }
    }
}
