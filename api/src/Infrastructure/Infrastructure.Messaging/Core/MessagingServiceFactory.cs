using FluentValidation;
using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.ApplicationCore.Models.DeviceLogs;
using Homemap.Infrastructure.Messaging.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Core
{
    internal class MessagingServiceFactory : IMessagingServiceFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly MessagingClient _messagingClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MessagingServiceFactory
        (
            IServiceScopeFactory scopeFactory,
            MessagingClient messagingClient,
            IOptions<JsonOptions> jsonOptions
        )
        {
            _scopeFactory = scopeFactory;
            _messagingClient = messagingClient;
            _jsonSerializerOptions = jsonOptions.Value.SerializerOptions;
        }

        public IMessagingService<T> Create<T>(string topic)
        {
            using var scope = _scopeFactory.CreateScope();

            return typeof(T) switch
            {
                Type type when type == typeof(DeviceLogMessage) =>
                    (IMessagingService<T>)new DeviceLogMessagingService(
                        topic,
                        _messagingClient,
                        _jsonSerializerOptions,
                        scope.ServiceProvider.GetRequiredService<IValidator<DeviceLogMessage>>()
                    ),
                _ => throw new NotImplementedException()
            };
        }
    }
}
