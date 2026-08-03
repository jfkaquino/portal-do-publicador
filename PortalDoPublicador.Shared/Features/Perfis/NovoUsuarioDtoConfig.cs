using System;
using Mapster;

namespace PortalDoPublicador.Shared.Features.Perfis;

public class NovoUsuarioDtoConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<NovoUsuarioDto, Usuario>()
            .Ignore(dest => dest.Id)
            .AfterMapping((src, dest) =>
            {
                if (dest.Perfil == null)
                {
                    dest.Perfil = new Perfil
                    {
                        Usuario = dest,
                        SituacaoEspiritual = src.SituacaoEspiritual
                    };
                }
                else
                {
                    dest.Perfil.SituacaoEspiritual = src.SituacaoEspiritual;
                }

                // Apenas adiciona histórico de pioneiro automaticamente na criação inicial (quando Id está vazio)
                if (dest.Id == Guid.Empty && src.ModalidadePioneiro != ModalidadePioneiro.Nenhum)
                {
                    dest.Perfil.HistoricoPioneiro.Add(new Pioneiro
                    {
                        Perfil = dest.Perfil,
                        ModalidadePioneiro = src.ModalidadePioneiro,
                        DataInicio = DateTime.Today
                    });
                }
            });

        config.NewConfig<Usuario, NovoUsuarioDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SituacaoEspiritual, src => src.Perfil != null ? src.Perfil.SituacaoEspiritual : SituacaoEspiritual.Publicador)
            .Map(dest => dest.ModalidadePioneiro, src => src.Perfil != null && src.Perfil.HistoricoPioneiro.Any() ? src.Perfil.HistoricoPioneiro.OrderByDescending(p => p.DataInicio).First().ModalidadePioneiro : ModalidadePioneiro.Nenhum);
    }
}
