using PortalDoPublicador.Shared.Features.Designacoes;

namespace PortalDoPublicador.Shared.Features.Eventos;

public class SaidaServicoCampo
{
    public Guid Id { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Notas { get; set; }
    
    public required string PontoDeEncontro { get; set; }
    public required string TerritorioAlocado { get; set; }
    
    public List<Designacao> Designacoes { get; set; } = [];
}
