using Homemap.Infrastructure.Messaging.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Formatter;
using System.Buffers;

namespace Homemap.Infrastructure.Messaging.Core
{
    internal class MessagingClient : IHostedService, IDisposable
    {
        private bool _isDisposed;

        protected readonly IMqttClient _mqttClient;

        private readonly MqttClientOptions _mqttClientOptions;

        private readonly ILogger<MessagingClient> _logger;

        public event EventHandler<MessagingClientMessageReceivedEventArgs>? MessageReceived;

        public MessagingClient(MessagingClientOptions options, ILogger<MessagingClient> logger)
        {
            _logger = logger;

            var mqttFactory = new MqttClientFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                .WithConnectionUri(options.ConnectionUri)
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .WithClientId("homemap-api/" + Guid.NewGuid().ToString())
                .WithCleanSession()
                .Build();

            _mqttClientOptions = mqttClientOptions;
            _mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
        }

        private Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            MessageReceived?.Invoke(this, new MessagingClientMessageReceivedEventArgs
            {
                Topic = args.ApplicationMessage.Topic,
                Payload = args.ApplicationMessage.Payload.ToArray(),
            });

            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ConnectAsync(cancellationToken);
        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            await _mqttClient.ConnectAsync(_mqttClientOptions, cancellationToken);

            // This will throw an exception if the server does not reply
            await _mqttClient.PingAsync(cancellationToken);
            _logger.LogInformation("Connected to broker...");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await DisconnectAsync(cancellationToken);
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            await _mqttClient.DisconnectAsync(MqttClientDisconnectOptionsReason.NormalDisconnection, cancellationToken: cancellationToken);
            _logger.LogInformation("Disconnected from broker...");
        }

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
                    _mqttClient.ApplicationMessageReceivedAsync -= MqttClient_ApplicationMessageReceivedAsync;
                    _mqttClient.Dispose();
                }

                _isDisposed = true;
            }
        }

        ~MessagingClient() => Dispose(false);
    }
}
