using PortalDoPublicador.Shared.Features.Designacoes;

namespace PortalDoPublicador.Shared.Features.Eventos;

public class Reuniao
{
    public Guid Id { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Notas { get; set; }
    
    public TipoReuniao Tipo { get; set; }
    
    public AssistenciaReuniao? Assistencia { get; set; }
    
    public List<Designacao> Designacoes { get; set; } = [];
    public List<Parte> Partes { get; set; } = [];
}

public enum TipoReuniao
{
    MeioDeSemana = 1,
    FimDeSemana = 2
}
