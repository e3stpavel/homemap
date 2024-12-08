using FluentValidation;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.Infrastructure.Messaging.Core;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Services.Subscription
{
    internal class DeviceLogSubscriptionService : SubscriptionService<LogMessageDto>
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

        protected override LogMessageDto? DeserializeMessage(string topic, byte[] payload)
        {
            var rawMessage = base.DeserializeMessage(topic, payload);
            if (rawMessage is null)
                return null;

            // extract device id from topic
            if (!int.TryParse(topic.Split('/').ElementAt(5), out int deviceId))
                return null;

            LogMessageDto message = rawMessage with { DeviceId = deviceId };

            var validationResult = _validator.Validate(message);
            if (!validationResult.IsValid)
                return null;

            return message;
        }
    }
}
