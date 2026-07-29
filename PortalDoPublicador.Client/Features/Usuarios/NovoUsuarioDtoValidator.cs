using FluentValidation;

namespace PortalDoPublicador.Client.Features.Usuarios;

public class NovoUsuarioDtoValidator : AbstractValidator<NovoUsuarioDto>
{
    public NovoUsuarioDtoValidator()
    {
        RuleFor(x => x.NomeCompleto)
            .NotEmpty().WithMessage("O nome completo é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");

        RuleFor(x => x.NomeExibicao)
            .NotEmpty().WithMessage("O nome de exibição é obrigatório.")
            .MaximumLength(30).WithMessage("O nome de exibição não pode exceder 30 caracteres.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateTime.Today).WithMessage("A data de nascimento deve ser no passado.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Informe um endereço de e-mail válido.");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.");

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("O endereço é obrigatório.");

        RuleFor(x => x.SituacaoEspiritual)
            .IsInEnum().WithMessage("Selecione uma situação espiritual válida.");
    }
}
