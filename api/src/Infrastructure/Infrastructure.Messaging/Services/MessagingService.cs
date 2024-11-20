using Homemap.Infrastructure.Messaging.Core;
using Homemap.Infrastructure.Messaging.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Services
{
    internal abstract class MessagingService<T> : IDisposable
    {
        private bool _isDisposed;

        private readonly MessagingClient _messagingClient;

        protected readonly SemaphoreSlim _IsMessageReceived = new(0);

        protected readonly ConcurrentQueue<T> _receivedMessages = new();

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MessagingService(MessagingClient messagingClient, IOptions<JsonOptions> jsonOptions)
        {
            _messagingClient = messagingClient;
            _jsonSerializerOptions = jsonOptions.Value.SerializerOptions;
            _messagingClient.MessageReceived += MessagingClient_MessageReceived;
        }

        private void MessagingClient_MessageReceived(object? sender, MessagingClientMessageReceivedEventArgs args)
        {
            T? message = JsonSerializer.Deserialize<T>(args.Payload, _jsonSerializerOptions);

            if (message is not null)
            {
                _receivedMessages.Enqueue(message);
                _IsMessageReceived.Release();
            }
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    _messagingClient.MessageReceived -= MessagingClient_MessageReceived;
                    _IsMessageReceived.Dispose();
                }

                _isDisposed = true;
            }
        }

        ~MessagingService() => Dispose(false);

        #endregion
    }
}
