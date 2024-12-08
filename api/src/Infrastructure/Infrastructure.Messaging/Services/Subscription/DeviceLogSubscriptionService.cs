using FluentValidation;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.Infrastructure.Messaging.Core;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Services.Subscription
{
    internal class DeviceLogSubscriptionService : AbstractSubscriptionService<LogMessageDto>
    {
        private readonly IValidator<LogMessageDto> _validator;

        public DeviceLogSubscriptionService
        (
            string topic,
            MessagingClient messagingClient,
            JsonSerializerOptions jsonSerializerOptions,
            IValidator<LogMessageDto> validator
        ) : base(topic, messagingClient, jsonSerializerOptions)
        {
            _validator = validator;
        }

        protected override LogMessageDto? OnMessageReceived(string topic, LogMessageDto payload)
        {
            // extract device id from topic
            if (!int.TryParse(topic.Split('/').ElementAt(5), out int deviceId))
                return null;

            LogMessageDto logMessage = payload with { DeviceId = deviceId };

            var validationResult = _validator.Validate(logMessage);
            if (!validationResult.IsValid)
                return null;

            return logMessage;
        }
    }
}
