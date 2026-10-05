using System.ComponentModel.DataAnnotations;

namespace PortalDoPublicador.Shared.Features.Programacoes.Enums;

public enum TipoTemplateParte
{
    [Display(Name = "Personalizada")]
    Personalizada = 0,

    [Display(Name = "Cântico")]
    Cantico = 1,

    [Display(Name = "Discurso (Tesouros)")]
    DiscursoTesouros = 2,

    [Display(Name = "Joias Espirituais")]
    JoiasEspirituais = 3,

    [Display(Name = "Leitura da Bíblia")]
    LeituraBiblia = 4,

    [Display(Name = "Iniciando Conversas")]
    IniciandoConversas = 5,

    [Display(Name = "Cultivando o Interesse")]
    CultivandoInteresse = 6,

    [Display(Name = "Explicando suas Crenças")]
    ExplicandoCrencas = 7,

    [Display(Name = "Fazendo Discípulos")]
    FazendoDiscipulos = 8,

    [Display(Name = "Discurso de Estudante")]
    DiscursoEstudante = 9,

    [Display(Name = "Parte (Nossa Vida Cristã)")]
    ParteVidaCrista = 10,

    [Display(Name = "Estudo Bíblico de Congregação")]
    EstudoBiblicoCongregacao = 11,

    [Display(Name = "Comentários Iniciais/Finais")]
    ComentariosIniciaisFinais = 12
}