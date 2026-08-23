using PortalDoPublicador.Shared.Features.Designacoes;

namespace PortalDoPublicador.Shared.Features.Eventos;

public class Parte
{
    public Guid Id { get; set; }
    
    public required Reuniao Reuniao { get; set; }

    public TipoSecaoReuniao SecaoReuniao { get; set; }

    public required string Tema { get; set; }
    public int TempoMinutos { get; set; }
    public string? Referencia { get; set; } 
    
    public List<Designacao> Designacoes { get; set; } = [];
}
