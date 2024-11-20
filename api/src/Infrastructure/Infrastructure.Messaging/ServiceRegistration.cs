using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.Infrastructure.Messaging.Core;
using Homemap.Infrastructure.Messaging.Models;
using Homemap.Infrastructure.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homemap.Infrastructure.Messaging
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMessagingService(this IServiceCollection services, IConfiguration configuration)
        {
            // configure connection options
            string connectionString = configuration.GetConnectionString("MqttDefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found (message queue)");

            services.AddSingleton(_ => new MessagingClientOptions
            {
                ConnectionUri = connectionString,
            });

            // inject client and run it on the background
            services.AddSingleton<MessagingClient>();
            services.AddHostedService(provider => provider.GetRequiredService<MessagingClient>());

            // inject services
            services.AddScoped<IDeviceLogMessagingService, DeviceLogMessagingService>();

            return services;
        }
    }
}
