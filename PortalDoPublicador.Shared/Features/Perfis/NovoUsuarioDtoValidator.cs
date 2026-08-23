using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PortalDoPublicador.Shared.Infrastructure.Data;

namespace PortalDoPublicador.Shared.Features.Perfis;

public class NovoUsuarioDtoValidator : AbstractValidator<NovoUsuarioDto>
{
    public NovoUsuarioDtoValidator(SharedDbContext context)
    {
        RuleFor(x => x.NomeCompleto)
            .NotEmpty().WithMessage("Nome Completo é obrigatório.");

        RuleFor(x => x.NomeExibicao)
            .NotEmpty().WithMessage("Nome de Exibição é obrigatório.");

        RuleFor(x => x.DataNascimento)
            .NotNull().WithMessage("Data de Nascimento é obrigatória.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.")
            .MustAsync(async (dto, email, cancellation) => 
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return true;
                }
                
                return !await context.Usuarios.AnyAsync(u => u.Email == email && u.Id != dto.Id, cancellation);
            }).WithMessage("Este e-mail já está em uso.");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório.");

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("Endereço é obrigatório.");
            
        RuleFor(x => x.Senha)
            .NotEmpty().When(x => x.Id is null || x.Id == Guid.Empty).WithMessage("Senha é obrigatória.")
            .MinimumLength(6).When(x => !string.IsNullOrWhiteSpace(x.Senha)).WithMessage("Senha deve ter no mínimo 6 caracteres.");
            
        RuleFor(x => x.SituacaoEspiritual)
            .IsInEnum().WithMessage("Situação Espiritual inválida.");
            
        RuleFor(x => x.ModalidadePioneiro)
            .IsInEnum().WithMessage("Modalidade de Pioneiro inválida.");
    }
}
