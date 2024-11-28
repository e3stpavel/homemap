using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceStates.Core;

namespace Homemap.ApplicationCore.Validators.DeviceStateDtoValidators.Core
{
    public class DeviceStateDtoValidator : AbstractValidator<DeviceStateDto>
    {
        public DeviceStateDtoValidator
        (
            ACStateDtoValidator acStateDtoValidator,
            ThermostatStateDtoValidator thermostatStateDtoValidator,
            LightbulbStateDtoValidator lightbulbStateDtoValidator
        )
        {
            RuleFor(x => x)
               .SetInheritanceValidator(v =>
               {
                   v.Add(acStateDtoValidator);
                   v.Add(thermostatStateDtoValidator);
                   v.Add(lightbulbStateDtoValidator);
               });

            RuleFor(x => x.IsTurnedOn)
                .Must(x => x == false || x == true);
        }
    }
}
