using FluentValidation;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.ApplicationCore.Validators.Extensions;

namespace Homemap.ApplicationCore.Validators.Messaging
{
    public class LogMessageDtoValidator : AbstractValidator<LogMessageDto>
    {
        public LogMessageDtoValidator()
        {
            RuleFor(log => log.Level)
                .IsLevel();

            RuleFor(log => log.Message)
                .IsMessage();

            RuleFor(log => log.Timestamp)
                .IsTimestamp();

            RuleFor(log => log.DeviceId)
                .NotEmpty();
        }
    }
}
