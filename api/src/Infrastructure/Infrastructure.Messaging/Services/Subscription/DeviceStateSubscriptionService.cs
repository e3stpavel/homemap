using FluentValidation;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.Infrastructure.Messaging.Core;
using System.Text.Json;

namespace Homemap.Infrastructure.Messaging.Services.Subscription
{
    internal class DeviceStateSubscriptionService : AbstractSubscriptionService<StateMessageDto>
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

        protected override StateMessageDto? OnMessageReceived(string topic, StateMessageDto payload)
        {
            var validationResult = _validator.Validate(payload);
            if (!validationResult.IsValid)
                return null;

            return payload;
        }
    }
}
