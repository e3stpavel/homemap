using FluentValidation;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.Infrastructure.Messaging.Core;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Services.Subscription
{
    internal class DeviceStateSubscriptionService : SubscriptionService<StateMessageDto>
    {
        private readonly IValidator<StateMessageDto> _validator;

        public DeviceStateSubscriptionService
        (
            string topic,
            MessagingClient messagingClient,
            JsonSerializerOptions jsonSerializerOptions,
            IValidator<StateMessageDto> validator
        ) : base(topic, messagingClient, jsonSerializerOptions)
        {
            _validator = validator;
        }

        protected override StateMessageDto? DeserializeMessage(string topic, byte[] payload)
        {
            var message = base.DeserializeMessage(topic, payload);
            if (message is null)
                return null;

            var validationResult = _validator.Validate(message);
            if (!validationResult.IsValid)
                return null;

            return message;
        }
    }
}
