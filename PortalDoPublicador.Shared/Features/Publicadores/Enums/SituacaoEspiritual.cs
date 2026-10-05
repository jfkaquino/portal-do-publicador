using System.ComponentModel.DataAnnotations;

namespace PortalDoPublicador.Shared.Features.Publicadores.Enums;

public enum SituacaoEspiritual
{
    [Display(Name = "Nenhum")]
    Nenhum,

    [Display(Name = "Publicador não batizado")]
    PublicadorNaoBatizado,

    [Display(Name = "Publicador batizado")]
    Publicador,

    [Display(Name = "Servo ministerial")]
    ServoMinisterial,

    [Display(Name = "Ancião")]
    Anciao,

    [Display(Name = "Removido")]
    Removido
}