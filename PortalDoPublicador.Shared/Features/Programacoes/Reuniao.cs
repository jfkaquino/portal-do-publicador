using PortalDoPublicador.Shared.Features.Designacoes;

namespace PortalDoPublicador.Shared.Features.Programacoes;

public class Reuniao
{
    public Guid Id { get; set; }
    public DateTime DataInicio { get; set; }
    public string? Referencia { get; set; }

    public int AssistenciaPresentes { get; set; }
    public int AssistenciaConectados { get; set; }
    public int TotalAssistencia => AssistenciaPresentes + AssistenciaConectados;

    public List<Parte> Partes { get; set; } = [];
    public List<DesignacaoReuniao> Designacoes { get; set; } = [];
}
