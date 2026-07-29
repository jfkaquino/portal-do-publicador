using System.ComponentModel.DataAnnotations;

namespace PortalDoPublicador.Shared.Features.Perfis;

public enum ModalidadePioneiro
{
    [Display(Name = "Nenhum")]
    Nenhum = 0,
    
    [Display(Name = "Pioneiro Auxiliar (15 Horas)")]
    Auxiliar15horas,
    
    [Display(Name = "Pioneiro Auxiliar (30 Horas)")]
    Auxiliar30horas,
    
    [Display(Name = "Pioneiro Regular")]
    Regular,
    
    [Display(Name = "Pioneiro Especial")]
    Especial
}
