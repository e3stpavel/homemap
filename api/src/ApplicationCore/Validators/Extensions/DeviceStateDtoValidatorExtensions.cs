using FluentValidation;

namespace Homemap.ApplicationCore.Validators.Extensions
{
    internal static class DeviceStateDtoValidatorExtensions
    {
        #region IsTemperature

        public static IRuleBuilderOptions<T, decimal?> IsTemperature<T>(this IRuleBuilder<T, decimal?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(18)
                .LessThanOrEqualTo(30)
                .PrecisionScale(3, 1, true)
                .When(property => property is not null);
        }

        public static IRuleBuilderOptions<T, decimal> IsTemperature<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(18)
                .LessThanOrEqualTo(30)
                .PrecisionScale(3, 1, true);
        }

        #endregion

        #region IsBrightness

        public static IRuleBuilderOptions<T, int?> IsBrightness<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .When(property => property is not null);
        }

        public static IRuleBuilderOptions<T, int> IsBrightness<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);
        }

        #endregion

        #region IsLightTemperature

        public static IRuleBuilderOptions<T, int?> IsLightTemperature<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(1500)
                .LessThanOrEqualTo(7000)
                .When(property => property is not null);
        }

        public static IRuleBuilderOptions<T, int> IsLightTemperature<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(1500)
                .LessThanOrEqualTo(7000);
        }

        #endregion
    }
}
