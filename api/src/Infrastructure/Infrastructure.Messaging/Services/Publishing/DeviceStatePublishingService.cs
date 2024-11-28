using Homemap.ApplicationCore.Models.DeviceStates.Core;
using Homemap.Infrastructure.Messaging.Core;
using Homemap.Infrastructure.Messaging.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Homemap.Infrastructure.Messaging.Services.Publishing
{
    internal class DeviceStatePublishingService : AbstractPublishingService<DeviceStateMessage>
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

        protected override MessagingClientPublishAsyncOptions ConfigureMessageOptions()
        {
            return new MessagingClientPublishAsyncOptions()
            {
                QoS = MessagingClientPublishAsyncOptions.QualityOfService.EXACTLY_ONCE,
            };
        }
    }
}
