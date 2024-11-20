using FluentValidation;
using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.ApplicationCore.Models;
using Homemap.Infrastructure.Messaging.Core;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Homemap.Infrastructure.Messaging.Services
{
    internal class DeviceLogMessagingService : MessagingService<DeviceLogDto>, IDeviceLogMessagingService
    {
        private readonly MessagingClient _messagingClient;

        private readonly IValidator<DeviceLogDto> _validator;

        public DeviceLogMessagingService
        (
            MessagingClient messagingClient,
            IValidator<DeviceLogDto> validator,
            IOptions<JsonOptions> jsonOptions
        ) : base(messagingClient, jsonOptions)
        {
            _messagingClient = messagingClient;
            _validator = validator;
        }

        public async Task<DeviceLogDto?> GetDeviceLogAsync(CancellationToken cancellationToken)
        {
            await _IsMessageReceived.WaitAsync(cancellationToken);
            _receivedMessages.TryDequeue(out DeviceLogDto? deviceLog);

            if (deviceLog is null)
                return null;

            var validationResult = _validator.Validate(deviceLog);
            if (!validationResult.IsValid)
                return null;

            return deviceLog;
        }

        public async Task ListenByProjectIdAsync(int projectId, CancellationToken cancellationToken)
        {
            string topic = $"logs/{projectId}/#";

            await _messagingClient.SubscribeAsync(topic);

            cancellationToken.Register(async () =>
                await _messagingClient.UnsubscribeAsync(topic));
        }
    }
}
