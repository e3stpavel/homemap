using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceStates.Core;
using Homemap.Infrastructure.Messaging.Core;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Services.Subscription
{
    internal class DeviceStateSubscriptionService : AbstractSubscriptionService<DeviceStateMessage>
    {
        private readonly IValidator<DeviceStateMessage> _validator;

        public DeviceStateSubscriptionService
        (
            string topic,
            MessagingClient messagingClient,
            JsonSerializerOptions jsonSerializerOptions,
            IValidator<DeviceStateMessage> validator
        ) : base(topic, messagingClient, jsonSerializerOptions)
        {
            _validator = validator;
        }

        protected override DeviceStateMessage? OnMessageReceived(string topic, DeviceStateMessage payload)
        {
            var validationResult = _validator.Validate(payload);
            if (!validationResult.IsValid)
                return null;

            return payload;
        }
    }
}
