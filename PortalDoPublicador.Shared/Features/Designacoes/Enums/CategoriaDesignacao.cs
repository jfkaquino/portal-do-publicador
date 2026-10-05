using System.ComponentModel.DataAnnotations;

namespace PortalDoPublicador.Shared.Features.Designacoes.Enums;

public enum CategoriaDesignacao
{
    [Display(Name = "Mecânica")]
    Mecanica = 0,

    [Display(Name = "Reunião")]
    Reuniao = 1,

    [Display(Name = "Parte")]
    Parte = 2,

    [Display(Name = "Ministério")]
    Ministerio = 3
}