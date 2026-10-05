namespace PortalDoPublicador.Shared.Features.Programacoes.Enums;

public static class TipoTemplateParteExtensions
{
    public static (string Tema, int Minutos, TipoSecaoReuniao Secao) ObterValoresPadrao(this TipoTemplateParte template) => template switch
    {
        TipoTemplateParte.Cantico => ("Cântico", 5, TipoSecaoReuniao.NossaVidaCrista),
        TipoTemplateParte.DiscursoTesouros => ("Discurso", 10, TipoSecaoReuniao.TesourosDaPalavraDeDeus),
        TipoTemplateParte.JoiasEspirituais => ("Joias espirituais", 10, TipoSecaoReuniao.TesourosDaPalavraDeDeus),
        TipoTemplateParte.LeituraBiblia => ("Leitura da Bíblia", 4, TipoSecaoReuniao.TesourosDaPalavraDeDeus),
        TipoTemplateParte.IniciandoConversas => ("Iniciando conversas", 3, TipoSecaoReuniao.FacaSeuMelhorNoMinisterio),
        TipoTemplateParte.CultivandoInteresse => ("Cultivando o interesse", 4, TipoSecaoReuniao.FacaSeuMelhorNoMinisterio),
        TipoTemplateParte.ExplicandoCrencas => ("Explicando suas crenças", 5, TipoSecaoReuniao.FacaSeuMelhorNoMinisterio),
        TipoTemplateParte.FazendoDiscipulos => ("Fazendo discípulos", 5, TipoSecaoReuniao.FacaSeuMelhorNoMinisterio),
        TipoTemplateParte.DiscursoEstudante => ("Discurso", 5, TipoSecaoReuniao.FacaSeuMelhorNoMinisterio),
        TipoTemplateParte.ParteVidaCrista => ("Parte", 15, TipoSecaoReuniao.NossaVidaCrista),
        TipoTemplateParte.EstudoBiblicoCongregacao => ("Estudo bíblico de congregação", 30, TipoSecaoReuniao.NossaVidaCrista),
        TipoTemplateParte.ComentariosIniciaisFinais => ("Comentários e Oração", 5, TipoSecaoReuniao.Abertura),
        _ => ("Nova Parte", 5, TipoSecaoReuniao.Abertura)
    };
}
