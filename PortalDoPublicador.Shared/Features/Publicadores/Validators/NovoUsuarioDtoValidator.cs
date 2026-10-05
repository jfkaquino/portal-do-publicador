using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PortalDoPublicador.Shared.Extensions;
using PortalDoPublicador.Shared.Features.Publicadores.DTOs;
using PortalDoPublicador.Shared.Infrastructure.Data;

namespace PortalDoPublicador.Shared.Features.Publicadores.Validators;

public class NovoUsuarioDtoValidator : AbstractValidator<NovoUsuarioDto>
{
    public NovoUsuarioDtoValidator(SharedDbContext context)
    {
        RuleFor(x => x.NomeCompleto)
            .Obrigatorio().WithName("Nome Completo");

        RuleFor(x => x.NomeExibicao)
            .Obrigatorio().WithName("Nome de Exibição");

        RuleFor(x => x.DataNascimento)
            .NaoNulo().WithName("Data de Nascimento");

        RuleFor(x => x.Email)
            .Obrigatorio().WithName("E-mail")
            .EmailValido()
            .MustAsync(async (dto, email, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return true;
                }

                return !await context.Usuarios.AnyAsync(u => u.Email == email && u.Id != dto.Id, cancellation);
            }).WithMessage("Este e-mail já está em uso.");

        RuleFor(x => x.Telefone)
            .Obrigatorio();

        RuleFor(x => x.Endereco)
            .Obrigatorio().WithName("Endereço");

        RuleFor(x => x.Senha)
            .Obrigatorio().When(x => x.Id is null || x.Id == Guid.Empty)
            .TamanhoMinimo(6).When(x => !string.IsNullOrWhiteSpace(x.Senha));

        RuleFor(x => x.SituacaoEspiritual)
            .EnumValido().WithName("Situação Espiritual");

        RuleFor(x => x.ModalidadePioneiro)
            .EnumValido().WithName("Modalidade de Pioneiro");
    }
}
