using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.Infrastructure.Messaging.Models;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Core
{
    internal abstract class PublishingService<T> : IPublishingService<T>
    {
        private readonly string _topic;

        private readonly MessagingClient _messagingClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public PublishingService(string topic, MessagingClient messagingClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _topic = topic;
            _messagingClient = messagingClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        protected async Task PublishAsync(T message, MessagingClientPublishAsyncOptions options)
        {
            byte[] payload = JsonSerializer.SerializeToUtf8Bytes(message, _jsonSerializerOptions);

            await _messagingClient.PublishAsync(_topic, payload, options);
        }

        public virtual async Task PublishAsync(T message)
        {
            var defaultOptions = new MessagingClientPublishAsyncOptions();
            await PublishAsync(message, defaultOptions);
        }
    }
}
