using FluentValidation;
using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.ApplicationCore.Models.DeviceLogs;
using Homemap.ApplicationCore.Models.DeviceStates.Core;
using Homemap.Infrastructure.Messaging.Services.Publishing;
using Homemap.Infrastructure.Messaging.Services.Subscription;
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

        public IPublishingService<T> CreatePublishingService<T>(string topic)
        {
            return typeof(T) switch
            {
                Type type when type == typeof(DeviceStateMessage) =>
                    (IPublishingService<T>)new DeviceStatePublishingService(
                        topic,
                        _messagingClient,
                        _jsonSerializerOptions
                    ),

                _ => throw new NotImplementedException()
            };
        }

        public ISubscriptionService<T> CreateSubscriptionService<T>(string topic)
        {
            using var scope = _scopeFactory.CreateScope();

            return typeof(T) switch
            {
                Type type when type == typeof(DeviceLogMessage) =>
                    (ISubscriptionService<T>)new DeviceLogSubscriptionService(
                        topic,
                        _messagingClient,
                        _jsonSerializerOptions,
                        scope.ServiceProvider.GetRequiredService<IValidator<DeviceLogMessage>>()
                    ),

                Type type when type == typeof(DeviceStateMessage) =>
                    (ISubscriptionService<T>)new DeviceStateSubscriptionService(
                        topic,
                        _messagingClient,
                        _jsonSerializerOptions,
                        scope.ServiceProvider.GetRequiredService<IValidator<DeviceStateMessage>>()
                    ),

                _ => throw new NotImplementedException()
            };
        }
    }
}
