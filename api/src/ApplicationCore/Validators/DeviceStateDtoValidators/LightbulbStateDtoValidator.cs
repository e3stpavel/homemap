using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceStates;
using Homemap.ApplicationCore.Validators.Extensions;

namespace Homemap.ApplicationCore.Validators.DeviceStateDtoValidators
{
    public class LightbulbStateDtoValidator : AbstractValidator<LightbulbStateDto>
    {
        public LightbulbStateDtoValidator()
        {
            RuleFor(x => x.Brightness)
                .IsBrightness();

            RuleFor(x => x.LightTemperature)
                .IsLightTemperature();
        }
    }
}
