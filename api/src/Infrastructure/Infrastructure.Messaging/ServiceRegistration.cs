using Homemap.Infrastructure.Messaging.Core;
using Homemap.Infrastructure.Messaging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homemap.Infrastructure.Messaging
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMessagingService(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MqttDefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found (message queue)");

            services.AddSingleton(_ => new MessagingClientOptions
            {
                ConnectionUri = connectionString,
            });

            services.AddSingleton<MessagingClient>();
            services.AddHostedService(provider => provider.GetRequiredService<MessagingClient>());

            return services;
        }
    }
}
