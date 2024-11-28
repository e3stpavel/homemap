using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceStates.Core;
using Homemap.ApplicationCore.Validators.Extensions;

namespace Homemap.ApplicationCore.Validators.DeviceStateDtoValidators.Core
{
    public class DeviceStateMessageValidator : AbstractValidator<DeviceStateMessage>
    {
        public DeviceStateMessageValidator()
        {
            RuleFor(x => x.Status)
                .Must(status => new List<string>(["on", "off"]).Contains(status));

            RuleFor(x => x.Temperature)
                .IsTemperature();

            RuleFor(x => x.Brightness)
               .IsBrightness();

            RuleFor(x => x.LightTemperature)
                .IsLightTemperature();
        }
    }
}
