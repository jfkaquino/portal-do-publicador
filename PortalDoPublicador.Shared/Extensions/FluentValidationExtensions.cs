using FluentValidation;

namespace PortalDoPublicador.Shared.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> Obrigatorio<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        => ruleBuilder.NotEmpty().WithMessage("{PropertyName} é obrigatório(a).");

    public static IRuleBuilderOptions<T, TProperty> NaoNulo<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        => ruleBuilder.NotNull().WithMessage("{PropertyName} é obrigatório(a).");

    public static IRuleBuilderOptions<T, string> TamanhoMaximo<T>(this IRuleBuilder<T, string> ruleBuilder, int max)
        => ruleBuilder.MaximumLength(max).WithMessage("{PropertyName} deve ter no máximo {MaxLength} caracteres.");

    public static IRuleBuilderOptions<T, string> TamanhoMinimo<T>(this IRuleBuilder<T, string> ruleBuilder, int min)
        => ruleBuilder.MinimumLength(min).WithMessage("{PropertyName} deve ter no mínimo {MinLength} caracteres.");

    public static IRuleBuilderOptions<T, string> EmailValido<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.EmailAddress().WithMessage("O formato do {PropertyName} é inválido.");

    public static IRuleBuilderOptions<T, TProperty> EnumValido<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        => ruleBuilder.IsInEnum().WithMessage("O valor informado para {PropertyName} não é válido.");

    public static IRuleBuilderOptions<T, string> TelefoneValido<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.Matches(@"^\(?[1-9]{2}\)?\s?(?:[2-8]|9[1-9])[0-9]{3}\-?[0-9]{4}$")
                      .WithMessage("{PropertyName} não é um número de telefone válido.");
}
