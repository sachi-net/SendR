using SimplySoft.Core.SendR.Enumerators;
using SimplySoft.Core.SendR.Exceptions;
using SimplySoft.Core.SendR.GlobalProperties;
using System;
using System.Collections.Generic;

namespace SimplySoft.Core.SendR.Email.Models
{
    /// <summary>
    /// SendR Email service configuration options.
    /// </summary>
    public class EmailOptions
    {
        private string host;
        private int port;
        private string username;
        private string password;
        private string businessName;
        private int timeOut;
        private bool enableSsl;
        private FallbackActionMode sendingFailedActionMode;
        private Action sendingFailedFallback;

        internal static IList<EmailTemplate> Templates;

        /// <summary>
        /// The SMTP host name or IP address.
        /// </summary>
        public string Host { 
            get { return host; } 
            set { host = value; EmailMessage.Settings.Host = host; } 
        }

        /// <summary>
        /// The port number of SMTP endpoint.
        /// </summary>
        public int Port { 
            get { return port; } 
            set { port = value; EmailMessage.Settings.Port = port; } 
        }

        /// <summary>
        /// Username of the sender to authenticate SMTP communication.
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; EmailMessage.Settings.Username = username; }
        }

        /// <summary>
        /// Password of the sender to authenticate SMTP communication.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; EmailMessage.Settings.Password = password; }
        }

        /// <summary>
        /// Display name to associated with this Username to be shown in email client application.
        /// </summary>
        public string BusinessName
        {
            get { return businessName; }
            set { businessName = value; EmailMessage.Settings.BusinessName = businessName; }
        }

        /// <summary>
        /// The timeout period (in milliseconds) to wait during SMTP transaction. Default value is 10000ms (or 100s).
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; EmailMessage.Settings.TimeOut = timeOut; }
        }

        /// <summary>
        /// Whether to use SSL (Secure Socket Layer) encryption while communicating with this SMTP server.
        /// </summary>
        public bool EnableSsl
        {
            get { return enableSsl; }
            set { enableSsl = value; EmailMessage.Settings.EnableSsl = enableSsl; }
        }

        /// <summary>
        /// Set the fallback ation type when failing to send an email. The default is set to <see cref="FallbackActionMode.ThrowException"/>.
        /// </summary>
        public FallbackActionMode SendingFailedFallbackMode
        {
            get { return sendingFailedActionMode; }
            set { sendingFailedActionMode = value; EmailMessage.SendingFailed = sendingFailedActionMode; }
        }

        /// <summary>
        /// Fallback action to perform after failed to send an email. This action will only invoked if the <see cref="SendingFailedFallbackMode"/> 
        /// is set to <see cref="FallbackActionMode.ExecuteAction"/>.
        /// </summary>
        public Action SendingFailedFallback
        {
            get { return sendingFailedFallback; }
            set { sendingFailedFallback = value; EmailMessage.SendingFailFallback = sendingFailedFallback; }
        }

        /// <summary>
        /// Add custom email template with specified <paramref name="name"/>, <paramref name="subject"/> and <paramref name="templatePath"/>.
        /// </summary>
        /// <param name="name">Name of the template. Name must be unique.</param>
        /// <param name="subject">Email subject associated with this template.</param>
        /// <param name="templatePath">Absolute path reference to the template file.</param>
        /// <exception cref="ArgumentException">Throws when template name is null or empty.</exception>
        /// <exception cref="DuplicateTemplateException">Throws when a template with this name is already defined.</exception>
        public void AddTemplate(string name, string subject, string templatePath)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Messages.EMAIL_TEMPLATE_NAME_REQUIRED, nameof(name));

            AddTemplate(new EmailTemplate
            {
                Name = name,
                Subject = subject,
                Path = templatePath
            });
        }

        /// <summary>
        /// Add custom email template with specified <see cref="EmailTemplate"/> object.
        /// </summary>
        /// <param name="emailTemplate">Email template to be added.</param>
        /// <exception cref="ArgumentException">Throws when template name is null or empty.</exception>
        /// <exception cref="DuplicateTemplateException">Throws when a template with this name is already defined.</exception>
        public void AddTemplate(EmailTemplate emailTemplate)
        {
            if (emailTemplate is null)
                return;

            var name = emailTemplate.Name;

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Messages.EMAIL_TEMPLATE_NAME_REQUIRED);

            if (EmailMessage.Templates.Contains(emailTemplate))
                throw new DuplicateTemplateException(Messages.
                    DUPLICATE_TEMPLATE.Replace("[T]", "Email").Replace("[N]", emailTemplate.Name));

            EmailMessage.Templates.Add(emailTemplate);
        }
    }
}
