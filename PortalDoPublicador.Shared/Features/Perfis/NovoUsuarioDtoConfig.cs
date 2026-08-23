using System;
using System.Linq;
using Mapster;

namespace PortalDoPublicador.Shared.Features.Perfis;

public class NovoUsuarioDtoConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<NovoUsuarioDto, Usuario>()
            .Ignore(dest => dest.Id)
            // Aqui normalmente injetaríamos um serviço de Hash, mas pelo Mapster direto podemos apenas mapear
            .Map(dest => dest.SenhaHash, src => src.Senha) // Temporário: em um ambiente real deve ser um Hash gerado no endpoint
            .AfterMapping((src, dest) =>
            {
                dest.SituacaoEspiritual = src.SituacaoEspiritual;

                // Apenas adiciona histórico de pioneiro automaticamente na criação inicial (quando Id está vazio)
                if (dest.Id == Guid.Empty && src.ModalidadePioneiro != ModalidadePioneiro.Nenhum)
                {
                    dest.HistoricoPioneiro.Add(new Pioneiro
                    {
                        Usuario = dest,
                        ModalidadePioneiro = src.ModalidadePioneiro,
                        DataInicio = DateTime.Today
                    });
                }
            });

        config.NewConfig<Usuario, NovoUsuarioDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SituacaoEspiritual, src => src.SituacaoEspiritual)
            .Map(dest => dest.ModalidadePioneiro, src => src.HistoricoPioneiro.Count != 0 ? src.HistoricoPioneiro.OrderByDescending(p => p.DataInicio).First().ModalidadePioneiro : ModalidadePioneiro.Nenhum);
    }
}
