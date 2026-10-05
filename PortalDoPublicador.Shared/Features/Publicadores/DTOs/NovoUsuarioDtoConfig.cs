using Mapster;
using PortalDoPublicador.Shared.Features.Publicadores.Enums;

namespace PortalDoPublicador.Shared.Features.Publicadores.DTOs;

public class NovoUsuarioDtoConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<NovoUsuarioDto, Usuario>()
            .Ignore(dest => dest.Id)
            .Map(dest => dest.SenhaHash, src => src.Senha)
            .AfterMapping((src, dest) =>
            {
                dest.SituacaoEspiritual = src.SituacaoEspiritual;

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