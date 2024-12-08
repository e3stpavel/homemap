using Homemap.ApplicationCore.Models.Messaging;
using Homemap.Infrastructure.Messaging.Core;
using Homemap.Infrastructure.Messaging.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Homemap.Infrastructure.Messaging.Services.Publishing
{
    internal class DeviceStatePublishingService : PublishingService<StateMessageDto>
    {
        public DeviceStatePublishingService
        (
            string topic,
            MessagingClient messagingClient,
            JsonSerializerOptions jsonSerializerOptions
        ) : base(topic, messagingClient, ConfigureJsonSerializerOptions(jsonSerializerOptions))
        {
        }

        private static JsonSerializerOptions ConfigureJsonSerializerOptions(JsonSerializerOptions defaultOptions)
        {
            return new JsonSerializerOptions(defaultOptions)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public override Task PublishAsync(StateMessageDto message)
        {
            return PublishAsync(message, new MessagingClientPublishAsyncOptions()
            {
                QoS = MessagingClientPublishAsyncOptions.QualityOfService.EXACTLY_ONCE,
            });
        }
    }
}
