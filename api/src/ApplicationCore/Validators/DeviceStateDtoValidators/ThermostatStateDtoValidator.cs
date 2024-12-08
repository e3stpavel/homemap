using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceStates;
using Homemap.ApplicationCore.Validators.Extensions;

namespace Homemap.ApplicationCore.Validators.DeviceStateDtoValidators
{
    public class ThermostatStateDtoValidator : AbstractValidator<ThermostatStateDto>
    {
        public ThermostatStateDtoValidator()
        {
            RuleFor(x => x.Temperature)
                .IsTemperature();
        }
    }
}
