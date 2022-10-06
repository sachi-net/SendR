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
    + [Send simple email](#send-simple-email)
    + [Send structured email using templates](#send-structured-email-using-templates)
+ [Implement custom notification](#implement-custom-notification)

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

If your app is .NET 6, use `Program.cs` to configure them.
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
All SendR notification implementations have to follow `INotificationSendable` in order to take the advantage of `INotificationCollection` service in order to have the advantage of DI.

### Send Email
`EmailMessage` will be used to manage email related functionalities within SendR. `EmailMessage` is found at,
```C#
namespace SimplySoft.Core.SendR.Email;
public class EmailMessage : INotificationSendable
```

The `EmailMessage` consists of following constructs.

Properties

|Name|Type|Description|
|--|--|--|
|Bcc|`MailAddressCollection`|Get all blind carbon copied recipients (BCC) of this `EmailMessage`|
|Cc|`MailAddressCollection`|Get all carbon copied recipients (CC) of this `EmailMessage`|
|Message|`string`|Get the message body of this `EmailMessage`|
|Subject|`string`|Get or set the subject of this `EmailMessage`|
|To|`MailAddressCollection`|Get all recipients (To) of this `EmailMessage`|

Methods

|Name|Return Type|Reference|Description|
|--|--|--|--|
|`AddTo(INotificationCollection)`|`void`|object|Add this `EmailMessage` to the provided `INotificationCollection`|
|`Create(string, string, string)`|`EmailMessage`|static|Create an instance of `EmailMessage` addressed to a single recipient. This does not use HTML rendering in message body|
|`Create(string, string, [dynamic])`|`EmailMessage`|static|Create an instance of `EmailMessage` using a defined `template` addressed to a single recipient. This uses HTML rendering in message body.|
|`Create(string, IEnumerable<MailAddress>, [dynamic], [IEnumerable<MailAddress>], [IEnumerable<MailAddress>])`|`EmailMessage`|static|Create an instance of `EmailMessage` using a defined `template` addressed to a list of `to"`, `cc` and `bcc` recipients. This uses HTML rendering in message body.|
|`Create(IEnumerable<MailAddress>, string, string, [IEnumerable<MailAddress>], [IEnumerable<MailAddress>], [bool])`|`EmailMessage`|static|Create an instance of `EmailMessage` addressed to a list of `to`, `cc` and `bcc` recipients.|
|`Send()`|`void`|object|Send this `EmailMessage` to the specified recipients.|

#### Send simple email

To send an email to a single recipient,
```C#
await EmailMessage.Create("me@mail.com", "Greetings", "Hello there!").SendAsync();
```

To send multiple email messages at once, Use `INotificationCollection` as follows.
```C#
// Inject INotificationCollection into your coding context
private readonly INotificationCollection _notifications;

EmailMessage.Create("tony@mail.com", "Greetings", "Hello Tony!").AddTo(_notifications);
EmailMessage.Create("kara@mail.com", "Greetings", "Hello Kara!").AddTo(_notifications);
EmailMessage.Create("bruce@mail.com", "Greetings", "Hello Bruce!").AddTo(_notifications);

Task.Run(_notifications.SendAllAsync);
```

> **Note** In context that DI is not configured, manually initialize `NotificationCollection` object.

#### Send structured email using templates
SendR can send use text-based predefine template to reuse the email content. Further, these templates support dynamic data binding at runtime, so you can reuse the same template with different dataset for different instances.

**Step 1 - Create a template**

Let's create the following HTML email template named `my-greeting.html` that uses a dynamic data model with 3 fields `Name`, `Age` and `IsMarried` and place in the path of `project-root\EmailTemplates\`.
```HTML
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
    <div>
        <h2>Greetings..!</h2>
        <p>
            Name: {{Name}} <br />
            Age: {{Age}} <br />
            Married: {{IsMarried}} <br />
        </p>
    </div>
</body>
</html>
```

**Step 2 - Add template in SendR service configuration**

Open `Startup.cs` or `Program.cs` (in .NET 6) to add the above template into SendR configurations. To register an email template into SendR, you can use `AddTemplate()` methods in `EmailOptions`. See [EmailOptions](#configure-email-with-emailoptions) for more. In this case, let's consider the reference name of this template as `greet` and its associated email subject is `Greetings`.

In .NET 5 or below,
```C#
using SendR;

public void ConfigureServices(IServiceCollection services) {
  services.ConfigureSendRNotifications(Configuration)
    .AddEmail(options => {
      var root = AppDomain.CurrentDomain.BaseDirectory;
      options.AddTemplate("greet", "Greetings", @$"{root}\MyTemplates\my-greeting.html");
    }).Build();
}
```

In .NET 6,
```C#
using SendR;

builder.Services.ConfigureSendRNotifications(builder.Configuration)
  .AddEmail(options => {
      var root = AppDomain.CurrentDomain.BaseDirectory;
      options.AddTemplate("greet", "Greetings", @$"{root}\MyTemplates\my-greeting.html");
    }).Build();
```

**Step 3 - Send email using the template**
Use the template to send an email as follows.
```C#
await EmailMessage.Create("greet", "tony@mail.com", new { Name = "Tony", Age = 57, IsMarried = true }).Result.SendAsync();
```
> **Warning** The properties of the dynamic object pass into the `Create()` method must match with the field names defined in the template.

---

## Implement custom notification

You can create you own custom notification implementation and still use it with `INotificationCollection`. The custom notification must implement the `INotificationSendable` interface allow it work with `INotificationCollection`.

For example, let's consider following `SMSNotification`.

```C#
public class SMSNotification : INotificationSendable 
{
  // Implement AddTo method
  public void AddTo(INotificationCollection notifications)
  {
    notifications.Add(this);
  }
  
  // Implement Send method for your custom notification object
  public void Send() { ... }
  
  // Implement SendAsync method for your custom notification object
  public async Task SendAsync() { ... }
}
```
