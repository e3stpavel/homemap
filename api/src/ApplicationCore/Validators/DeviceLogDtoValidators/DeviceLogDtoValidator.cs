using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceLogs;

namespace Homemap.ApplicationCore.Validators.DeviceLogDtoValidators
{
    public class DeviceLogDtoValidator : AbstractValidator<DeviceLogDto>
    {
        public DeviceLogDtoValidator()
        {
            Include(new AbstractDeviceLogDtoValidator());

            RuleFor(log => log.Device)
                .NotEmpty()
                .SetValidator(new DeviceDtoValidator());
        }
    }
}
