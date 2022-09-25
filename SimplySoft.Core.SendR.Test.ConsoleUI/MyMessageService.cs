using SimplySoft.Core.SendR.Email;
using System.Threading.Tasks;

namespace SimplySoft.Core.SendR.Test.ConsoleUI
{
    public class MyMessageService
    {
        private readonly INotificationCollection _notifications;

        public MyMessageService(INotificationCollection notifications)
        {
            _notifications = notifications;
        }

        public async Task SendMessage()
        {
            await EmailMessage.Create("tony@mail.com", "Welcome", "Hello Tony Stark!").SendAsync();
        }

        public void SendMultipleMessages()
        {
            EmailMessage.Create("kara@mail.com", "Welcome", "Hello Kara Danvers!").AddTo(_notifications);
            EmailMessage.Create("barry@mail.com", "Welcome", "Hello Barry Allen!").AddTo(_notifications);
            EmailMessage.Create("oliver@mail.com", "Welcome", "Hello Oliver Queen!").AddTo(_notifications);

            Task.Run(_notifications.SendAllAsync);
        }

        public void SendWithTemplate()
        {
            var data = new { Name = "Peter", Age = 45, IsMarried = true };
            EmailMessage.Create("my-template", "peter@mail.com", data).Result.AddTo(_notifications);

            Task.Run(_notifications.SendAllAsync);
        }
    }
}
