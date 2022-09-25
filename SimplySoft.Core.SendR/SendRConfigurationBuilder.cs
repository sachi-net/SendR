using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplySoft.Core.SendR.Email;
using SimplySoft.Core.SendR.Email.Models;
using SimplySoft.Core.SendR.GlobalProperties;
using System;
using System.Collections.Generic;

namespace SimplySoft.Core.SendR
{
    /// <summary>
    /// SendR service configuration builder with related settings and options.
    /// </summary>
    public sealed class SendRConfigurationBuilder
    {
        private readonly IConfiguration _config;
        private readonly IServiceCollection _services;

        internal SendRConfigurationBuilder(IConfiguration config, IServiceCollection services)
        {
            _config = config;
            _services = services;
        }

        /// <summary>
        /// Add Email notification facility with SendR notification subsystem by pre-configured options from <see cref="IConfiguration"/>.
        /// </summary>
        /// <returns><see cref="SendRConfigurationBuilder"/> that can be used to further configure SendR services.</returns>
        /// <exception cref="ApplicationException">Throws when the provided <see cref="IConfiguration"/> is not defined.</exception>
        /// <remarks>Use <c>Build()</c> after configuring all SendR services in order to 
        /// allow further chaining other services into this <see cref="IServiceCollection"/>.</remarks>
        public SendRConfigurationBuilder AddEmail()
        {
            if (_config is null)
            {
                throw new ApplicationException(Messages.CONFIGURATION_ERROR);
            }

            var section = "SendR:Notification:Email";

            EmailMessage.Settings = new EmailSettings
            {
                Host = _config.GetValue<string>($"{section}:Host"),
                Port = _config.GetValue<int>($"{section}:Port"),
                Username = _config.GetValue<string>($"{section}:Username"),
                Password = _config.GetValue<string>($"{section}:Password"),
                BusinessName = _config.GetValue<string>($"{section}:BusinessName"),
                TimeOut = _config.GetValue<int>($"{section}:TimeOut"),
                EnableSsl = _config.GetValue<bool>($"{section}:EnableSsl")
            };

            EmailOptions.Templates = new List<EmailTemplate>();
            EmailMessage.ServiceConfigured = true;

            return this;
        }

        /// <summary>
        /// Add Email notification facility with SendR notification subsystem by manual <see cref="EmailOptions"/> configuration settings.
        /// The added options override the configurations from <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="setupAction">Configuration setup action with <see cref="EmailOptions"/>.</param>
        /// <returns><see cref="SendRConfigurationBuilder"/> that can be used to further configure SendR services.</returns>
        /// <remarks>Use <c>Build()</c> after configuring all SendR services in order to 
        /// allow further chaining other services into this <see cref="IServiceCollection"/>.</remarks>
        public SendRConfigurationBuilder AddEmail(Action<EmailOptions> setupAction)
        {
            AddEmail();
            setupAction.Invoke(new EmailOptions());
            return this;
        }

        /// <summary>
        /// Finalize SendR notification service configuration and preserve the ability to further chaining other services 
        /// into this <see cref="IServiceCollection"/>.
        /// </summary>
        /// <returns><see cref="IServiceCollection"/> after configured SendR services.</returns>
        public IServiceCollection Build()
        {
            return _services;
        }

        /// <summary>
        /// Configure Email notification facility with SendR notification subsystem with provided <see cref="EmailOptions"/> out of the 
        /// dependency injection pipeline.
        /// </summary>
        /// <param name="setupAction">Configuration setup action with <see cref="EmailOptions"/>.</param>
        public static void ConfigureEmailService(Action<EmailOptions> setupAction)
        {
            EmailMessage.Settings = new EmailSettings();
            EmailOptions.Templates = new List<EmailTemplate>();
            setupAction.Invoke(new EmailOptions());
            EmailMessage.ServiceConfigured = true;
        }
    }
}
