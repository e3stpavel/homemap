using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homemap.Infrastructure.Messaging
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMessagingService(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MqttDefaultConnection") ?? throw new InvalidOperationException("Connection string not found (message queue)");

            return services;
        }
    }
}
