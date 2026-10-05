using PortalDoPublicador.Shared.Features.Designacoes;
using PortalDoPublicador.Shared.Features.Programacoes.Enums;

namespace PortalDoPublicador.Shared.Features.Programacoes;

public class Parte
{
    public Guid Id { get; set; }

    public Guid ReuniaoId { get; set; }
    public required Reuniao Reuniao { get; set; }

    public TipoSecaoReuniao SecaoReuniao { get; set; }
    public int Ordem { get; set; }
    public required string Tema { get; set; }
    public string? Descricao { get; set; }
    public int TempoMinutos { get; set; }
    public string? Referencia { get; set; }

    public TipoTemplateParte Template { get; set; } = TipoTemplateParte.Personalizada;
    public int? NumeroCantico { get; set; }

    public List<DesignacaoParte> Designacoes { get; set; } = [];
}
