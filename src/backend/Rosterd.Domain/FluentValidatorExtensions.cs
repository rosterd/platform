using FluentValidation;

namespace Rosterd.Domain
{
    public static class FluentValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> NotStartWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Must(m => m != null && !m.StartsWith(" ")).WithMessage("'{PropertyName}' should not start with whitespace");

        public static IRuleBuilderOptions<T, string> NotEndWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Must(m => m != null && !m.EndsWith(" ")).WithMessage("'{PropertyName}' should not end with whitespace");
    }
}
