using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceLogs;

namespace Homemap.ApplicationCore.Validators.DeviceLogDtoValidators
{
    public class DeviceLogMessageValidator : AbstractValidator<DeviceLogMessage>
    {
        public DeviceLogMessageValidator()
        {
            Include(new AbstractDeviceLogDtoValidator());

            RuleFor(log => log.DeviceId)
                .NotEmpty();
        }
    }
}
