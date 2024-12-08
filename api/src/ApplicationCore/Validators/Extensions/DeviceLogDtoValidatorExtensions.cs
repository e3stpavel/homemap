using FluentValidation;

namespace Homemap.ApplicationCore.Validators.Extensions
{
    internal static class DeviceLogDtoValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsLevel<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(level => new List<string>(["error", "warning", "info"]).Contains(level));
        }

        public static IRuleBuilderOptions<T, string> IsMessage<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty();
        }

        public static IRuleBuilderOptions<T, DateTime> IsTimestamp<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
