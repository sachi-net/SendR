using SimplySoft.Core.SendR.Email.Models;
using SimplySoft.Core.SendR.Enumerators;
using SimplySoft.Core.SendR.Exceptions;
using SimplySoft.Core.SendR.GlobalProperties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SimplySoft.Core.SendR.Email
{
    /// <summary>
    /// Provide Email service with SendR notification subsystem.
    /// </summary>
    public class EmailMessage : INotificationSendable
    {
        internal static bool ServiceConfigured = false;
        internal static EmailSettings Settings { get; set; }
        internal static IList<EmailTemplate> Templates { get; } = new List<EmailTemplate>();
        internal static Action SendingFailFallback { get; set; }
        internal static FallbackActionMode SendingFailed { get; set; } = FallbackActionMode.ThrowException;

        private readonly MailMessage mailMessage = new MailMessage();

        /// <summary>
        /// Get all recipients (To) of this <see cref="EmailMessage"/> as a <see cref="MailAddressCollection"/>.
        /// </summary>
        public MailAddressCollection To { get { return mailMessage.To; } }

        /// <summary>
        /// Get all carbon copied recipients (CC) of this <see cref="EmailMessage"/> as a <see cref="MailAddressCollection"/>.
        /// </summary>
        public MailAddressCollection Cc { get { return mailMessage.CC; } }

        /// <summary>
        /// Get all blind carbon copied recipients (BCC) of this <see cref="EmailMessage"/> as a <see cref="MailAddressCollection"/>.
        /// </summary>
        public MailAddressCollection Bcc { get { return mailMessage.Bcc; } }

        /// <summary>
        /// Get or set the subject of this <see cref="EmailMessage"/>.
        /// </summary>
        public string Subject { get { return mailMessage.Subject; } set { mailMessage.Subject = value; } }

        /// <summary>
        /// Get the message body of this <see cref="EmailMessage"/>.
        /// </summary>
        public string Message { get { return mailMessage.Body; } }

        private EmailMessage() { }

        /// <summary>
        /// Create an instance of <see cref="EmailMessage"/> addressed to a single recipient. This does not use HTML rendering in message body.
        /// </summary>
        /// <param name="to">Address of the recipient where this <see cref="EmailMessage"/> is about to sent.</param>
        /// <param name="subject">Subject associated with this <see cref="EmailMessage"/>.</param>
        /// <param name="message">Body of this <see cref="EmailMessage"/>.</param>
        /// <returns>An instance of <see cref="EmailMessage"/> with configured setup.</returns>
        public static EmailMessage Create(string to, string subject, string message)
        {
            IList<MailAddress> recipients = new List<MailAddress> { new MailAddress(to) };
            return Create(recipients, subject, message);
        }

        /// <summary>
        /// Create an instance of <see cref="EmailMessage"/> using a defined template addressed to a single recipient. 
        /// This uses HTML rendering in message body.
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="to">Address of the recipient where this <see cref="EmailMessage"/> is about to sent.</param>
        /// <param name="data"><see langword="dynamic"/> data model to bind with the message body of this <see cref="EmailMessage"/>.
        /// <br/>This parameter is optional.</param>
        /// <returns>An instance of <see cref="EmailMessage"/> with configured setup as an awaitable <see cref="Task"/>.</returns>
        /// <exception cref="TemplateNotDefinedException">Throws when there is no <see cref="EmailTemplate"/> found by the <paramref name="templateName"/> in 
        /// the registered template collection.</exception>
        /// <exception cref="FileNotFoundException">Throws when there is no reference file found by the <see cref="EmailTemplate.Path"/>.</exception>
        public static async Task<EmailMessage> Create(string templateName, string to, dynamic data = null)
        {
            IList<MailAddress> recipients = new List<MailAddress> { new MailAddress(to) };
            return await Create(templateName, recipients, data, cc: null, bcc: null);
        }

        /// <summary>
        /// Create an instance of <see cref="EmailMessage"/> using a defined template addressed to a list of <paramref name="to"/>, <paramref name="cc"/> and 
        /// <paramref name="bcc"/> recipients. This uses HTML rendering in message body.
        /// </summary>
        /// <param name="templateName">Name of the Email template registered with <see cref="EmailOptions"/>.</param>
        /// <param name="to">Address list of recipients where this <see cref="EmailMessage"/> is about to sent.</param>
        /// <param name="data"><see langword="dynamic"/> data model to bind with the message body of this <see cref="EmailMessage"/>.
        /// <br/>This parameter is optional.</param>
        /// <param name="cc">Address list of carbon copied recipients where this <see cref="EmailMessage"/> is about to forward.</param>
        /// <param name="bcc">Address list of blind carbon copied recipients where this <see cref="EmailMessage"/> is about to forward.</param>
        /// <returns>An instance of <see cref="EmailMessage"/> with configured setup as an awaitable <see cref="Task"/>.</returns>
        /// <exception cref="ServiceNotConfiguredException">Throws when SendR notification is not configured use Email service.</exception>
        /// <exception cref="TemplateNotDefinedException">Throws when there is no <see cref="EmailTemplate"/> found by the <paramref name="templateName"/> in 
        /// the registered template collection.</exception>
        /// <exception cref="FileNotFoundException">Throws when there is no reference file found by the <see cref="EmailTemplate.Path"/>.</exception>
        public static async Task<EmailMessage> Create(string templateName, IEnumerable<MailAddress> to, 
            dynamic data = null, IEnumerable<MailAddress> cc = null, IEnumerable<MailAddress> bcc = null)
        {
            if (!ServiceConfigured)
                throw new ServiceNotConfiguredException(Messages
                    .SERVICE_NOT_CONFIGURED.Replace("[S]", typeof(EmailMessage).FullName));

            var template = Templates
                .Where(t => t.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (template is null)
                throw new TemplateNotDefinedException(Messages
                    .EMAIL_TEMPLATE_NOT_DEFINED.Replace("[T]", templateName));

            if (!File.Exists(template.Path))
                throw new FileNotFoundException(Messages
                    .EMAIL_TEMPLATE_PATH_NOT_EXIST.Replace("[P]", template.Path));


            var content = await File.ReadAllTextAsync(template.Path);

            if (data != null)
                BindData(data, ref content);

            return Create(to, template.Subject, content, cc, bcc, useHtml: true);
        }

        /// <summary>
        /// Create an instance of <see cref="EmailMessage"/> addressed to a list of <paramref name="to"/>, <paramref name="cc"/> and 
        /// <paramref name="bcc"/> recipients.
        /// </summary>
        /// <param name="to">Address list of recipients where this <see cref="EmailMessage"/> is about to sent.</param>
        /// <param name="subject">Subject associated with this <see cref="EmailMessage"/>.</param>
        /// <param name="message">Body of this <see cref="EmailMessage"/>.</param>
        /// <param name="cc">Address list of carbon copied recipients where this <see cref="EmailMessage"/> is about to forward.</param>
        /// <param name="bcc">Address list of blind carbon copied recipients where this <see cref="EmailMessage"/> is about to forward.</param>
        /// <param name="useHtml">Whether to use HTML rendered body for this <see cref="EmailMessage"/>. Default is set to <see langword="false"/>.</param>
        /// <returns>An instance of <see cref="EmailMessage"/> with configured setup.</returns>
        /// <exception cref="ServiceNotConfiguredException">Throws when SendR notification is not configured use Email service.</exception>
        public static EmailMessage Create(IEnumerable<MailAddress> to, string subject, string message,
            IEnumerable<MailAddress> cc = null, IEnumerable<MailAddress> bcc = null, bool useHtml = false)
        {
            if (!ServiceConfigured)
                throw new ServiceNotConfiguredException(Messages
                    .SERVICE_NOT_CONFIGURED.Replace("[S]", typeof(EmailMessage).FullName));

            var email = new EmailMessage();
            email.mailMessage.Subject = subject;
            email.mailMessage.Body = message;
            email.mailMessage.IsBodyHtml = useHtml;
            email.mailMessage.From = new MailAddress(Settings.Username, Settings.BusinessName);

            foreach (var address in to)
                email.mailMessage.To.Add(address);

            if (cc?.Count() > 0)
            {
                foreach (var address in cc)
                    email.mailMessage.CC.Add(address);
            }

            if (bcc?.Count() > 0)
            {
                foreach (var address in bcc)
                    email.mailMessage.Bcc.Add(address);
            }

            return email;
        }

        /// <summary>
        /// Add this <see cref="EmailMessage"/> to the provided <see cref="INotificationCollection"/>.
        /// </summary>
        /// <param name="notifications"><see cref="INotificationCollection"/> in which this <see cref="EmailMessage"/> is added into.</param>
        public void AddTo(INotificationCollection notifications)
        {
            notifications.Add(this);
        }

        /// <summary>
        /// Send this <see cref="EmailMessage"/> to the specified recipients.
        /// </summary>
        /// <exception cref="ServiceNotConfiguredException">Throws when SendR notification is not configured use Email service.</exception>
        /// <exception cref="NotificationNotSentException">Throws when SendR failed to send this <see cref="EmailMessage"/>.</exception>
        public void Send()
        {
            var client = BuildClient();
            try
            {
                client.Send(mailMessage);
            }
            catch (NotificationNotSentException exp)
            {
                HandleFallback(SendingFailed, SendingFailFallback, exp);
            }
        }

        /// <summary>
        /// Send this <see cref="EmailMessage"/> to the specified recipients asynchronously.
        /// </summary>
        /// <returns>Awaitable <see cref="Task"/> as the result of this operation.</returns>
        /// <exception cref="ServiceNotConfiguredException">Throws when SendR notification is not configured use Email service.</exception>
        /// <exception cref="NotificationNotSentException">Throws when SendR failed to send this <see cref="EmailMessage"/>.</exception>
        public async Task SendAsync()
        {
            var client = BuildClient();
            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception exp)
            {
                HandleFallback(SendingFailed, SendingFailFallback, exp);
            }
        }

        private static void BindData(dynamic data, ref string content)
        {
            IDictionary<string, object> properties = ((object)data).GetType().GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(data));

            foreach (var property in properties)
            {
                string key = "{{#}}".Replace("#", property.Key);
                content = content.Replace(key, property.Value.ToString());
            }
        }

        private static SmtpClient BuildClient()
        {
            if (!ServiceConfigured)
                throw new ServiceNotConfiguredException(Messages
                    .SERVICE_NOT_CONFIGURED.Replace("[S]", typeof(EmailMessage).FullName));

            var client = new SmtpClient
            {
                Host = Settings.Host,
                Port = Settings.Port,
                Credentials = new NetworkCredential
                {
                    UserName = Settings.Username,
                    Password = Settings.Password
                },
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = Settings.EnableSsl,
                Timeout = Settings.TimeOut
            };

            return client;
        }

        private void HandleFallback(FallbackActionMode fallbackActionMode, Action action, Exception exception = null)
        {
            switch (fallbackActionMode)
            {
                default:
                case FallbackActionMode.ThrowException:
                    throw exception;
                case FallbackActionMode.Ignore:
                    return;
                case FallbackActionMode.ExecuteAction:
                    action?.Invoke(); break;
            }
        }
    }
}
