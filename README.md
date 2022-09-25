# SendR
SendR (SendR.Core) is .NET Core based reusable library designed to send simple messages or structured message notifications using simple pre-configurations. Current version of SendR is built for emails out of the box. More notification instances will be added in future releases. SendR can be extended for your custom messaging services as well.

## Prerequisites
SendR.Core requires [.NET 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1) or later. SendR is designed to work with Microsoft dependency injection pipeline. However, SendR is also usable out of the dependecy injection as well.

## Quick Demo
SendR can easily send messages even for bulk collection using its `INotificationCollection` service.

Configure SendR settings in `appsettings.json`
```JSON
"SendR": {
  "Notification": {
    "Email": {
      "Host": "smtp.example-host.com",
      "Port": 587,
      "Username": "example-client@mail.com",
      "Password": "client-password",
      "BuisnessName": "SendR Message Service",
      "TimeOut": 60000,
      "EnableSsl": true
    }
  }
}
```

Register SendR dependency services in `Startup.cs` or `Program.cs` (in .NET 6)
```C#
using SendR;

// ...

services.ConfigureSendRNotifications(Configuration).AddEmail().Build();

// ...
```

Send messages using `INotificationCollection`
```C#
using SimplySoft.Core.SendR.Email;
using System.Threading.Tasks;

public class MyMessagingService 
{
  private readonly INotificationCollection _notifications;
  
  public MyMessagingService(INotificationCollection notifications) 
  {
    _notifications = notifications;
  }
  
  // Send message at a time
  public async Task SendMessage() 
  {
    await EmailMessage.Create("tony@mail.com", "Welcome", "Hello Tony Stark!").SendAsync();
  }
  
  // Send multiple messages at once
  public void SendMultipleMessages() 
  {
    EmailMessage.Create("kara@mail.com", "Welcome", "Hello Kara Danvers!").AddTo(_notifications);
    EmailMessage.Create("barry@mail.com", "Welcome", "Hello Barry Allen!").AddTo(_notifications);
    EmailMessage.Create("oliver@mail.com", "Welcome", "Hello Oliver Queen!").AddTo(_notifications);
    
    Task.Run(_notifications.SendAllAsync);
  }
}
```
