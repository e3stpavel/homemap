using FluentValidation;
using Homemap.ApplicationCore.Models;

namespace Homemap.ApplicationCore.Validators
{
    public class AbstractDeviceLogDtoValidator : AbstractValidator<AbstractDeviceLogDto>
    {
        public AbstractDeviceLogDtoValidator()
        {
            RuleFor(log => log.Level)
                .Must(level => new List<string>(["error", "warning", "info"]).Contains(level));

            RuleFor(log => log.Message)
                .NotEmpty();

            RuleFor(log => log.Timestamp)
                .LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
