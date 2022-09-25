using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySoft.Core.SendR;
using SimplySoft.Core.SendR.Email;
using SimplySoft.Core.Test.WebUI.Models;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SimplySoft.Core.Test.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationCollection _notifications;

        public HomeController(ILogger<HomeController> logger, INotificationCollection notifications)
        {
            _logger = logger;
            _notifications = notifications;
        }

        public IActionResult Index()
        {
            try
            {
                string myGmail = "netsachintha@gmail.com";

                EmailMessage.Create(myGmail, "Welcome", "Hello! welcome to SendR!").AddTo(_notifications);
                EmailMessage.Create("send-wish", myGmail, new { Name = "Sachintha", Age = 65, IsMarried = false }).Result.AddTo(_notifications);

                //Task.Run(_notifications.SendAsync);

                return View((object)"Hello");
            }
            catch (Exception exp)
            {
                StringBuilder message = new StringBuilder();
                message
                    .Append($"<h4 style='color:#de3748'>{exp.Message}</h4>")
                    .Append($"<p style='color:red;font-size:0.85rem;padding-left:1rem'>" +
                        $"<code>{exp.StackTrace}</code></p>");
                return View((object)message.ToString());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Email()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Email(MailModel mail)
        {
            if (ModelState.IsValid)
            {
                string subject = mail.Subject;
                string message = mail.Message;
            }

            ModelState.Clear();

            mail.ResponseMessage = new ResponseMessage
            {
                AlertLevel = AlertLevel.Success,
                Message = "Your action is successful"
            };

            return View(mail);
        }

        public IActionResult Start()
        {
            var model = new StartModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Start(StartModel model)
        {
            string expectedPin = "1234";
            int nextStage = (int)TempData["Stage"];

            switch (nextStage)
            {
                case 1:
                    TempData["Username"] = model.UsernamePrompt.Username;
                    model.NextStage = nextStage + 1;
                    model.ResponseMessage = new ResponseMessage
                    {
                        AlertLevel = AlertLevel.Information,
                        Message = "You are now in stage 2!"
                    };
                    break;
                case 2:
                    if (!model.ActivationPrompt.Pin.Equals(expectedPin))
                    {
                        ModelState.Clear();
                        model.NextStage = nextStage;
                        model.ActivationPrompt.Pin = string.Empty;
                        model.ResponseMessage = new ResponseMessage
                        {
                            AlertLevel = AlertLevel.Error,
                            Message = "Invalid verification PIN!"
                        };
                    }
                    else
                    {
                        model.NextStage = nextStage + 1;
                        model.ResponseMessage = new ResponseMessage
                        {
                            AlertLevel = AlertLevel.Information,
                            Message = "You are now in stage 3!"
                        };
                    }
                    break;
                case 3:
                    TempData["Password"] = model.ResetPasswordPrompt.Password;
                    model.NextStage = 0;
                    model.ResponseMessage = new ResponseMessage
                    {
                        AlertLevel = AlertLevel.Success,
                        Message = $"Success! Username: {TempData["Username"]} " +
                        $"| Password: {TempData["Password"]}"
                    };

                    TempData.Remove("Username");
                    TempData.Remove("Stage");

                    break;
            }

            return View(model);
        }

        public IActionResult AccountRecovery()
        {
            var model = new StartModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult AccountRecovery(StartModel model)
        {
            string expectedPin = "1234";
            int nextStage = (int)TempData["Stage"];

            try
            {
                switch (nextStage)
                {
                    case 1:
                        TempData["Username"] = model.UsernamePrompt.Username;
                        model.NextStage = nextStage + 1;
                        model.ActivationPrompt = new ActivationModel 
                        { 
                            Email = "netsachintha@gmail.com".ToMask('@', maskCharacter: '•') 
                        };
                        break;
                    case 2:
                        if (!model.ActivationPrompt.Pin.Equals(expectedPin))
                        {
                            ModelState.Clear();
                            model.NextStage = nextStage;
                            model.ActivationPrompt.Pin = string.Empty;
                            model.ResponseMessage = new ResponseMessage
                            {
                                Style = Style.Banner,
                                AlertLevel = AlertLevel.Error,
                                Title = "Error",
                                Message = "Invalid verification PIN!",
                                ShowCloseButton = false
                            };
                        }
                        else
                        {
                            model.NextStage = nextStage + 1;
                        }
                        break;
                    case 3:
                        TempData["Password"] = model.ResetPasswordPrompt.Password;
                        model.NextStage = 0;
                        model.ResponseMessage = new ResponseMessage
                        {
                            Style = Style.Banner,
                            AlertLevel = AlertLevel.Success,
                            Title = "Success",
                            Message = $"Username: {TempData["Username"]} " +
                            $"| Password: {TempData["Password"]}"
                        };

                        TempData.Remove("Username");
                        TempData.Remove("Stage");

                        break;
                }
            }
            catch (Exception)
            {
                return View(model);
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
