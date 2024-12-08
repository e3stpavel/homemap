using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.Infrastructure.Messaging.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Homemap.Infrastructure.Messaging.Core
{
    internal abstract class SubscriptionService<T> : ISubscriptionService<T>
    {
        private readonly string _topic;

        private readonly string _topicPattern;

        private readonly MessagingClient _messagingClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SubscriptionService(string topic, MessagingClient messagingClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _topic = topic;
            _topicPattern = _topic.Replace("+", "[^/]").Replace("#", ".*"); // TODO: test it

            _messagingClient = messagingClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        protected virtual T? DeserializeMessage(string topic, byte[] payload)
        {
            return JsonSerializer.Deserialize<T>(payload, _jsonSerializerOptions);
        }

        public async IAsyncEnumerable<T> StreamAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await _messagingClient.SubscribeAsync(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // create a task that resolves once message arrives
                    var tcs = new TaskCompletionSource<T>();
                    cancellationToken.Register(() => tcs.TrySetCanceled());

                    void MessagingClient_MessageReceived(object? sender, MessagingClientMessageReceivedEventArgs args)
                    {
                        if (!Regex.IsMatch(args.Topic, _topicPattern))
                            return;

                        T? message = DeserializeMessage(args.Topic, args.Payload);

                        if (message is not null)
                            tcs.TrySetResult(message);
                    }

                    try
                    {
                        _messagingClient.MessageReceived += MessagingClient_MessageReceived;
                        yield return await tcs.Task;
                    }
                    finally
                    {
                        _messagingClient.MessageReceived -= MessagingClient_MessageReceived;
                    }
                }
            }
            finally
            {
                await _messagingClient.UnsubscribeAsync(_topic);
            }
        }
    }
}
