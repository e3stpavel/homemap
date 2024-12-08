using FluentValidation;
using Homemap.ApplicationCore.Models;
using Homemap.ApplicationCore.Validators.Extensions;

namespace Homemap.ApplicationCore.Validators
{
    public class DeviceLogDtoValidator : AbstractValidator<DeviceLogDto>
    {
        public DeviceLogDtoValidator()
        {
            RuleFor(log => log.Level)
                .IsLevel();

            RuleFor(log => log.Message)
                .IsMessage();

            RuleFor(log => log.Timestamp)
                .IsTimestamp();

            RuleFor(log => log.Device)
                .NotEmpty()
                .SetValidator(new DeviceDtoValidator());
        }
    }
}
