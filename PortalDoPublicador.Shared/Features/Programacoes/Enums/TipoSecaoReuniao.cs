using System.ComponentModel.DataAnnotations;

namespace PortalDoPublicador.Shared.Features.Programacoes.Enums;

public enum TipoSecaoReuniao
{
    // Reunião de Meio de Semana
    [Display(Name = "Abertura")]
    Abertura = 0,

    [Display(Name = "Tesouros da Palavra de Deus")]
    TesourosDaPalavraDeDeus = 1,

    [Display(Name = "Faça Seu Melhor no Ministério")]
    FacaSeuMelhorNoMinisterio = 2,

    [Display(Name = "Nossa Vida Cristã")]
    NossaVidaCrista = 3,

    [Display(Name = "Conclusão")]
    Conclusao = 4,

    // Reunião de Fim de Semana
    [Display(Name = "Discurso Público")]
    DiscursoPublico = 5,

    [Display(Name = "Estudo de A Sentinela")]
    EstudoDeASentinela = 6
}