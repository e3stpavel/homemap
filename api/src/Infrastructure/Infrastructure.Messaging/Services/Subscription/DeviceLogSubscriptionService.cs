using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceLogs;
using Homemap.Infrastructure.Messaging.Core;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Services.Subscription
{
    internal class DeviceLogSubscriptionService : AbstractSubscriptionService<DeviceLogMessage>
    {
        private readonly IValidator<DeviceLogMessage> _validator;

        public DeviceLogSubscriptionService
        (
            string topic,
            MessagingClient messagingClient,
            JsonSerializerOptions jsonSerializerOptions,
            IValidator<DeviceLogMessage> validator
        ) : base(topic, messagingClient, jsonSerializerOptions)
        {
            _validator = validator;
        }

        protected override DeviceLogMessage? OnMessageReceived(string topic, DeviceLogMessage payload)
        {
            // extract device id from topic
            if (!int.TryParse(topic.Split('/').ElementAt(5), out int deviceId))
                return null;

            DeviceLogMessage logMessage = payload with { DeviceId = deviceId };

            var validationResult = _validator.Validate(logMessage);
            if (!validationResult.IsValid)
                return null;

            return logMessage;
        }
    }
}
