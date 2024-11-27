using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.Infrastructure.Messaging.Models;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Core
{
    internal abstract class AbstractPublishingService<T> : IPublishingService<T>
    {
        private readonly string _topic;

        private readonly MessagingClient _messagingClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        protected abstract MessagingClientPublishAsyncOptions ConfigureMessageOptions();

        public AbstractPublishingService(string topic, MessagingClient messagingClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _topic = topic;
            _messagingClient = messagingClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task PublishAsync(T message)
        {
            byte[] payload = JsonSerializer.SerializeToUtf8Bytes(message, _jsonSerializerOptions);
            var options = ConfigureMessageOptions();

            await _messagingClient.PublishAsync(_topic, payload, options);
        }
    }
}
