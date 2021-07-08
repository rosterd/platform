using System.Collections.Generic;
using FluentValidation;

namespace Rosterd.Web.Infra
{
    public static class FluentValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> NotStartWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Must(m => m != null && !m.StartsWith(" ")).WithMessage("'{PropertyName}' should not start with whitespace");

        public static IRuleBuilderOptions<T, string> NotEndWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Must(m => m != null && !m.EndsWith(" ")).WithMessage("'{PropertyName}' should not end with whitespace");

        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num) =>
            ruleBuilder.Must((rootObject, list, context) => {
                    context.MessageFormatter
                        .AppendArgument("MaxElements", num)
                        .AppendArgument("TotalElements", list.Count);

                    return list.Count < num;
                })
                .WithMessage("{PropertyName} must contain fewer than {MaxElements} items. The list contains {TotalElements} element");

        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainAtLeastOne<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder) =>
            ruleBuilder.Must((rootObject, list, context) => list.IsNotNullOrEmpty())
                .WithMessage("{PropertyName} must contain at least one element");
    }
}
