using FluentValidation;
using Homemap.ApplicationCore.Models;
using Homemap.ApplicationCore.Validators.DeviceStateDtoValidators;

namespace Homemap.ApplicationCore.Validators
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
