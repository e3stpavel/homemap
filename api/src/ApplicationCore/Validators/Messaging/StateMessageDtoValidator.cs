using FluentValidation;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.ApplicationCore.Validators.Extensions;

namespace Homemap.ApplicationCore.Validators.Messaging
{
    public class StateMessageDtoValidator : AbstractValidator<StateMessageDto>
    {
        public StateMessageDtoValidator()
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
