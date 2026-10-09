using PortalDoPublicador.Shared.Features.Designacoes;
using PortalDoPublicador.Shared.Features.Designacoes.Enums;
using PortalDoPublicador.Shared.Features.Programacoes;
using PortalDoPublicador.Shared.Features.Programacoes.Enums;
using ReuniaoModel = PortalDoPublicador.Shared.Features.Programacoes.Reuniao;

namespace PortalDoPublicador.Client.Features.Programacoes.ReuniaoMeioDeSemana;

public static class ReuniaoMeioDeSemanaMock
{
    public static ReuniaoModel CriarPadrao()
    {
        var reuniao = new ReuniaoModel
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today.AddHours(19).AddMinutes(30),
            Referencia = "JEREMIAS 29-30"
        };

        List<Parte> partesPadrao =
        [
            // ABERTURA
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.Abertura, Ordem = 1, Tema = "Cântico 12 e oração", TempoMinutos = 5, Descricao = "Comentários iniciais (1 min)", Referencia = "mwb26.07", Template = TipoTemplateParte.ComentariosIniciaisFinais },

            // TESOUROS DA PALAVRA DE DEUS
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.TesourosDaPalavraDeDeus, Ordem = 2, Tema = "1. Jeová disciplina seus servos na medida certa", TempoMinutos = 10, Referencia = "Jer. 29-30", Descricao = "Discurso baseado no artigo principal.", Template = TipoTemplateParte.DiscursoTesouros },
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.TesourosDaPalavraDeDeus, Ordem = 3, Tema = "2. Joias espirituais", TempoMinutos = 10, Referencia = "Jer. 29-30", Descricao = "Perguntas e respostas com a assistência.", Template = TipoTemplateParte.JoiasEspirituais },
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.TesourosDaPalavraDeDeus, Ordem = 4, Tema = "3. Leitura da Bíblia", TempoMinutos = 4, Referencia = "Jeremias 30:1-11", Descricao = "Leitura do trecho bíblico designado.", Template = TipoTemplateParte.LeituraBiblia },

            // FAÇA SEU MELHOR NO MINISTÉRIO
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.FacaSeuMelhorNoMinisterio, Ordem = 5, Tema = "4. Iniciando conversas", TempoMinutos = 3, Referencia = "mwb26.07", Descricao = "Demonstração usando a sugestão de conversa.", Template = TipoTemplateParte.IniciandoConversas },
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.FacaSeuMelhorNoMinisterio, Ordem = 6, Tema = "5. Iniciando conversas", TempoMinutos = 4, Referencia = "mwb26.07", Descricao = "Demonstração abordando morador ocupado.", Template = TipoTemplateParte.IniciandoConversas },
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.FacaSeuMelhorNoMinisterio, Ordem = 7, Tema = "6. Discurso", TempoMinutos = 5, Referencia = "mwb26.07", Descricao = "Discurso para a congregação.", Template = TipoTemplateParte.DiscursoEstudante },

            // NOSSA VIDA CRISTÃ
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.NossaVidaCrista, Ordem = 8, Tema = "Cântico", NumeroCantico = 3, TempoMinutos = 5, Referencia = string.Empty, Descricao = "Cântico intermediário da reunião.", Template = TipoTemplateParte.Cantico },
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.NossaVidaCrista, Ordem = 9, Tema = "7. Jeová dá esperança a seus servos", TempoMinutos = 15, Referencia = "mwb26.07", Descricao = "Consideração com a congregação e vídeo.", Template = TipoTemplateParte.ParteVidaCrista },
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.NossaVidaCrista, Ordem = 10, Tema = "8. Campanha especial em setembro", TempoMinutos = 5, Referencia = "mwb26.07", Descricao = "Discurso feito pelo superintendente de serviço. Fale com entusiasmo sobre a campanha e explique o que foi planejado para a congregação.", Template = TipoTemplateParte.ParteVidaCrista },
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.NossaVidaCrista, Ordem = 11, Tema = "9. Estudo bíblico de congregação", TempoMinutos = 30, Referencia = "bt cap. 15", Descricao = "Leitura e comentários dos parágrafos designados.", Template = TipoTemplateParte.EstudoBiblicoCongregacao },

            // CONCLUSÃO
            new() { Id = Guid.NewGuid(), Reuniao = reuniao, SecaoReuniao = TipoSecaoReuniao.Conclusao, Ordem = 12, Tema = "Cântico", NumeroCantico = 156, TempoMinutos = 4, Referencia = string.Empty, Descricao = "Comentários finais (3 min) seguidos de cântico e oração de encerramento.", Template = TipoTemplateParte.Cantico }
        ];

        // Designações mock iniciais
        var parteAbertura = partesPadrao.FirstOrDefault(p => p.Ordem == 1);
        if (parteAbertura is not null)
        {
            parteAbertura.Designacoes.Add(new DesignacaoParte
            {
                Id = Guid.NewGuid(),
                Parte = parteAbertura,
                Tipo = new TipoDesignacao { Id = Guid.NewGuid(), Nome = "Presidente", Categoria = CategoriaDesignacao.Parte },
                Usuario = new PortalDoPublicador.Shared.Features.Publicadores.Usuario
                {
                    Id = Guid.NewGuid(),
                    NomeCompleto = "Carlos Silva",
                    NomeExibicao = "Carlos Silva",
                    Email = "carlos.silva@congregacao.org",
                    Telefone = "(11) 98888-8888",
                    Endereco = "Rua A",
                    SenhaHash = "hash"
                }
            });
        }

        var parte8 = partesPadrao.FirstOrDefault(p => p.Ordem == 10);
        if (parte8 is not null)
        {
            parte8.Designacoes.Add(new DesignacaoParte
            {
                Id = Guid.NewGuid(),
                Parte = parte8,
                Tipo = new TipoDesignacao { Id = Guid.NewGuid(), Nome = "Parte", Categoria = CategoriaDesignacao.Parte },
                Usuario = new PortalDoPublicador.Shared.Features.Publicadores.Usuario
                {
                    Id = Guid.NewGuid(),
                    NomeCompleto = "Jorge Aquino",
                    NomeExibicao = "Jorge Aquino",
                    Email = "jorge.aquino@congregacao.org",
                    Telefone = "(11) 97777-7777",
                    Endereco = "Rua B",
                    SenhaHash = "hash"
                }
            });
        }

        reuniao.Partes = partesPadrao;
        return reuniao;
    }
}
