using Homemap.Infrastructure.Messaging.Core;
using Homemap.Infrastructure.Messaging.Models;
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

        public MessagingService(MessagingClient messagingClient)
        {
            _messagingClient = messagingClient;
            _messagingClient.MessageReceived += MessagingClient_MessageReceived;
        }

        private void MessagingClient_MessageReceived(object? sender, MessagingClientMessageReceivedEventArgs args)
        {
            T? message = JsonSerializer.Deserialize<T>(args.Payload);

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
                    _messagingClient.Dispose();
                    _IsMessageReceived.Dispose();
                }

                _isDisposed = true;
            }
        }

        ~MessagingService() => Dispose(false);

        #endregion
    }
}
