using Microsoft.FluentUI.AspNetCore.Components;
using PortalDoPublicador.Shared.Features.Programacoes.Enums;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace PortalDoPublicador.Client.Features.Programacoes.ReuniaoMeioDeSemana;

public static class TipoTemplateParteUIExtensions
{
    public static Icon ObterIcone(this TipoTemplateParte template) => template switch
    {
        TipoTemplateParte.Cantico => new Icons.Regular.Size20.MusicNote2(),
        TipoTemplateParte.DiscursoTesouros => new Icons.Regular.Size20.Lightbulb(),
        TipoTemplateParte.JoiasEspirituais => new Icons.Regular.Size20.Diamond(),
        TipoTemplateParte.LeituraBiblia => new Icons.Regular.Size20.BookOpen(),
        TipoTemplateParte.IniciandoConversas or
        TipoTemplateParte.CultivandoInteresse or
        TipoTemplateParte.FazendoDiscipulos or
        TipoTemplateParte.ExplicandoCrencas => new Icons.Regular.Size20.ChatBubblesQuestion(),
        TipoTemplateParte.EstudoBiblicoCongregacao => new Icons.Regular.Size20.People(),
        TipoTemplateParte.DiscursoEstudante => new Icons.Regular.Size20.Mic(),
        TipoTemplateParte.ComentariosIniciaisFinais => new Icons.Regular.Size20.Comment(),
        _ => new Icons.Regular.Size20.DocumentText()
    };

    public static Color ObterCorIcone(this TipoTemplateParte template) => template switch
    {
        TipoTemplateParte.Cantico => Color.Primary,
        TipoTemplateParte.DiscursoTesouros or
        TipoTemplateParte.JoiasEspirituais or
        TipoTemplateParte.LeituraBiblia => Color.Primary,
        _ => Color.Default
    };
}
