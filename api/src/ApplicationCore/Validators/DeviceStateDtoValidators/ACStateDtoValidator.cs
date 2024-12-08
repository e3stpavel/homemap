using FluentValidation;
using Homemap.ApplicationCore.Models.DeviceStates;
using Homemap.ApplicationCore.Validators.Extensions;

namespace Homemap.ApplicationCore.Validators.DeviceStateDtoValidators
{
    public class ACStateDtoValidator : AbstractValidator<ACStateDto>
    {
        public ACStateDtoValidator()
        {
            RuleFor(x => x.Temperature)
                .IsTemperature();
        }
    }
}
