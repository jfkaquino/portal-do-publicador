using System.ComponentModel.DataAnnotations;

namespace PortalDoPublicador.Shared.Features.Publicadores.Enums;

[Flags]
public enum TipoResponsabilidade
{
    [Display(Name = "Nenhum")]
    Nenhum = 0,

    [Display(Name = "Coordenador do Corpo de Anciãos")]
    Coordenador = 1,

    [Display(Name = "Secretário")]
    Secretario = 2,

    [Display(Name = "Superintendente de Serviço")]
    SuperintendenteServico = 4,

    [Display(Name = "Dirigente do Estudo de A Sentinela")]
    DirigenteEstudoSentinela = 8,

    [Display(Name = "Superintendente da Reunião Vida no Ministério")]
    SuperintendenteVidaMinisterio = 16,

    [Display(Name = "Superintendente de Grupo")]
    SuperintendenteGrupo = 32,
}