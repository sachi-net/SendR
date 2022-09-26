# SendR
SendR (SendR.Core) is .NET Core based reusable library designed to send simple messages or structured message notifications using simple pre-configurations. Current version of SendR is built for emails out of the box. More notification instances will be added in future releases. SendR can be extended for your custom messaging services as well.

## Prerequisites
SendR.Core requires [.NET 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1) or later. SendR is designed to work with Microsoft dependency injection pipeline. However, SendR is also usable out of the dependency injection as well.

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
  
  public MyMessagingService(INotificationCollection notifications) => _notifications = notifications;
  
  // Send notification at a time
  public async Task SendMessage() 
  {
    await EmailMessage.Create("tony@mail.com", "Welcome", "Hello Tony Stark!").SendAsync();
    await EmailMessage.Create("peter@mail.com", "Welcome", "Hello Peter Parker!").SendAsync();
    await EmailMessage.Create("bruce@mail.com", "Welcome", "Hello Bruce Banner!").SendAsync();
  }
  
  // Send multiple notifications at once
  public void SendMultipleMessages() 
  {
    EmailMessage.Create("kara@mail.com", "Welcome", "Hello Kara Danvers!").AddTo(_notifications);
    EmailMessage.Create("barry@mail.com", "Welcome", "Hello Barry Allen!").AddTo(_notifications);
    EmailMessage.Create("oliver@mail.com", "Welcome", "Hello Oliver Queen!").AddTo(_notifications);
    
    Task.Run(_notifications.SendAllAsync);
  }
}
```

---

## Table of Content
+ [Prerequisites](#prerequisites)
+ [Quick Demo](#quick-demo)
+ [SendR Notifications](#sendr-notifications)
+ [Configure SendR Notifications](#configure-sendr-notifications)
  + [Configure Email Service](#configure-email-service)
    + [Configure Email with `appsettings.json`](#configure-email-with-appsettingsjson)
    + [Configure Email with `EmailOptions`](#configure-email-with-emailoptions)
    + [Configure Email out of DI pipeline](#configure-email-out-of-di-pipeline)
+ [Send Notifications](#send-notifications)
  + [Send Email](#send-email)

---

## SendR Notifications
SendR is a single package that helps you to send messages using its `INotificationCollection` as managed notification handler. SendR can use `INotificationCollection` to easily send and manage any type of message that implements `INotificationSendable` interface. SendR comes with pre-built email messaging service which can work with `INotificationCollection` easily.

---

## Configure SendR Notifications
In .NET Core Dependency Injection (DI) system, all pre-built messaging services must be registered before using them in codebase. SendR is also capable of configuring its services in a non-DI enabled environment as well. However, if SendR is configured into DI service collection, the `INotificationCollection` handler will be available across the DI pipeline throughout your application.

### Configure Email Service
Email service requires some initial options to perform SMTP transaction. You can define them to configure email service in two ways. 

#### Configure Email with `appsettings.json`
The easiest and quickest way to configure SendR is to using `appsettings.json`. Define SMTP settings required for email service in `appsettings.json` as follows.
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
Then, add SendR services to application's dependency injection system.
If your app is .NET 5 or bellow, use `Startup.cs` to configure them.
```C#
using SendR;

public void ConfigureServices(IServiceCollection services) {
  services.ConfigureSendRNotifications(Configuration).AddEmail().Build();
}
```

If your app is .NET 6 or above, use `Program.cs` to configure them.
```C#
using SendR;

builder.Services.ConfigureSendRNotifications(builder.Configuration).AddEmail().Build();
```

#### Configure Email with `EmailOptions`
Using `EmailOptions` allows you to configure SendR email service further. `EmailOptions` consists with following constructs.

##### Enum

`FallbackActionMode` defines fallback action type to handle a failure when performing SendR transaction.

|FallbackActionMode|Description|
|--|--|
|Ignore|Ignore the underlying failure and try to proceed forward.|
|ThrowException|Throw an exception on a failure of SendR transaction.|
|ExecuteAction|Invoke the provided action on a failure of SendR transaction. This requires to setup fallback action into `EmailOptions.SendingFailedFallback` action delegate.|

##### Properties

|Name|Type|Description|
|--|--|--|
|BusinessName|`string`|Display name to associated with this Username to be shown in email client application.|
|EnableSsl|`bool`|Whether to use SSL (Secure Socket Layer) encryption while communicating with this SMTP server.|
|Host|`string`|The SMTP host name or IP address.|
|Password|`string`|Password of the sender to authenticate SMTP communication.|
|Port|`int`|The port number of SMTP endpoint.|
|SendingFailedFallbackMode|`FallbackActionMode`|Set the fallback ation type when failing to send an email. The default is set to `FallbackActionMode.ThrowException`.|
|SendingFailedFallback|`Action`|Fallback action to perform after failed to send an email. This action will only invoked if the `SendingFailedFallbackMode` is set to `FallbackActionMode.ExecuteAction`.|

##### Methods

|Name|Return Type|Description|
|--|--|--|
|`AddTemplate(EmailTemplate)`|`void`|Add custom email template with specified `EmailTemplate` object.|
|`AddTemplate(string, string, string)`|`void`|Add custom email template with specified `name`, `subjec` and`templatePath`.|

If your app is .NET 5 or bellow, use `Startup.cs` to configure them.
```C#
using SendR;

public void ConfigureServices(IServiceCollection services) {
  services.ConfigureSendRNotifications(Configuration)
    .AddEmail(options => {
      var root = AppDomain.CurrentDomain.BaseDirectory;
    
      options.Host = "smtp.example-host.com";
      options.Port = 587;
      options.Username = "example-client@mail.com";
      options.Password = "client-password";
      options.BusinessName = "SendR Message Service";
      options.TimeOut = 60000;
      options.EnableSsl = true;
      options.SendingFailedFallbackMode = FallbackActionMode.ExecuteAction;
      options.SendingFailedFallback = () => Logger.LogError("Sending failed");
      options.AddTemplate("welcome-message", "Greetings", @$"{root}\MyTemplates\Welcome.html");
    }).Build();
}
```

If your app is .NET 6 or above, use `Program.cs` to configure them.
```C#
using SendR;

builder.Services.ConfigureSendRNotifications(builder.Configuration)
  .AddEmail(options => {
      var root = AppDomain.CurrentDomain.BaseDirectory;
    
      options.Host = "smtp.example-host.com";
      options.Port = 587;
      options.Username = "example-client@mail.com";
      options.Password = "client-password";
      options.BusinessName = "SendR Message Service";
      options.TimeOut = 60000;
      options.EnableSsl = true;
      options.SendingFailedFallbackMode = FallbackActionMode.ExecuteAction;
      options.SendingFailedFallback = () => Logger.LogError("Sending failed");
      options.AddTemplate("welcome-message", "Greetings", @$"{root}\MyTemplates\Welcome.html");
    }).Build();
```

> **Note** Adding custom email templates will be discussed later in this document.

##### Configure Email out of DI pipeline
If you want use SendR out of Microsoft Dependency Injection (DI) context, you can configure SendR services where your application starip trigger as follows.
```C#
SendRConfigurationBuilder.ConfigureEmailService(options => {
  var root = AppDomain.CurrentDomain.BaseDirectory;
    
  options.Host = "smtp.example-host.com";
  options.Port = 587;
  options.Username = "example-client@mail.com";
  options.Password = "client-password";
  options.BusinessName = "SendR Message Service";
  options.TimeOut = 60000;
  options.EnableSsl = true;
  options.SendingFailedFallbackMode = FallbackActionMode.ExecuteAction;
  options.SendingFailedFallback = () => Logger.LogError("Sending failed");
  options.AddTemplate("welcome-message", "Greetings", @$"{root}\MyTemplates\Welcome.html");
});
```

---

## Send Notifications
All SendR notification implementations have to follow `INotificationSendable` in order to take the advantage of `INotificationCollection` service out of the DI pipeline.

### Send Email
`EmailMessage` will be used to manage email related functionalities within SendR. The `EmailMessage` is consist of following constructs.
