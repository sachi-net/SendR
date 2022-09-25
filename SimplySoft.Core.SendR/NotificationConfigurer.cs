using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplySoft.Core.SendR;

namespace SendR
{
    /// <summary>
    /// Extensions methods to configure SendR services into application's dependency injection pipeline.
    /// </summary>
    public static class NotificationConfigurer
    {
        /// <summary>
        /// Adds SendR notification services into the provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add SendR services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> where the service options are stored.</param>
        /// <returns><see cref="SendRConfigurationBuilder"/> that can be used to further configure SendR services.</returns>
        /// <remarks>Use <c>Build()</c> after configuring all SendR services in order to 
        /// allow further chaining other services into this <see cref="IServiceCollection"/>.</remarks>
        public static SendRConfigurationBuilder ConfigureSendRNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationBuilder = new SendRConfigurationBuilder(configuration, services);
            services.AddScoped<INotificationCollection, NotificationCollection>();
            return configurationBuilder;
        }
    }
}
