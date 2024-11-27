using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.Infrastructure.Messaging.Models;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Homemap.Infrastructure.Messaging.Core
{
    internal abstract class AbstractMessagingService<T> : IMessagingService<T>
    {
        private bool _isDisposed;

        private readonly string _topic;

        private readonly string _topicPattern;

        private readonly MessagingClient _messagingClient;

        private readonly SemaphoreSlim _messageReceivedNotification = new(0);

        private readonly ConcurrentQueue<T> _messagesReceived = new();

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        protected abstract T? OnMessageReceived(string topic, T payload);

        public AbstractMessagingService(string topic, MessagingClient messagingClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _topic = topic;
            _topicPattern = _topic.Replace("+", "[^/]").Replace("#", ".*"); // TODO: test it

            _messagingClient = messagingClient;
            _messagingClient.MessageReceived += MessagingClient_MessageReceived;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        private void MessagingClient_MessageReceived(object? sender, MessagingClientMessageReceivedEventArgs args)
        {
            if (!Regex.IsMatch(args.Topic, _topicPattern))
                return;

            T? payload = JsonSerializer.Deserialize<T>(args.Payload, _jsonSerializerOptions);
            if (payload is null)
                return;

            T? message = OnMessageReceived(args.Topic, payload);

            if (message is not null)
            {
                _messagesReceived.Enqueue(message);
                _messageReceivedNotification.Release();
            }
        }

        public async Task<T?> GetNextMessageAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _messageReceivedNotification.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return default;
            }

            _messagesReceived.TryDequeue(out var message);

            return message;
        }

        public Task PublishAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SubscribeAsync()
        {
            await _messagingClient.SubscribeAsync(_topic);
        }

        public async Task UnsubscribeAsync()
        {
            await _messagingClient.UnsubscribeAsync(_topic);
        }

        #region IDisposable implementation

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    _messagingClient.MessageReceived -= MessagingClient_MessageReceived;
                    _messageReceivedNotification.Dispose();
                }

                _isDisposed = true;
            }
        }

        ~AbstractMessagingService() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
