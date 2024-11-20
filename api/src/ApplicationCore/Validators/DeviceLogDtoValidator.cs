using FluentValidation;
using Homemap.ApplicationCore.Models;

namespace Homemap.ApplicationCore.Validators
{
    public class DeviceLogDtoValidator : AbstractValidator<DeviceLogDto>
    {
        public DeviceLogDtoValidator()
        {
            RuleFor(log => log.Level)
                .Must(level => new List<string>(["error", "warning", "info"]).Contains(level));

            RuleFor(log => log.Message)
                .NotEmpty();

            RuleFor(log => log.Timestamp)
                .LessThanOrEqualTo(DateTime.UtcNow);

            // probably we should not validate it here with database call
            RuleFor(log => log.DeviceId)
                .NotEmpty();
        }
    }
}
