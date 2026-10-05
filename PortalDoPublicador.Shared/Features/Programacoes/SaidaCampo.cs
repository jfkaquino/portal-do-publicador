using PortalDoPublicador.Shared.Features.Designacoes;

namespace PortalDoPublicador.Shared.Features.Programacoes;

public class SaidaCampo
{
    public Guid Id { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Notas { get; set; }

    public List<DesignacaoCampo> Designacoes { get; set; } = [];
}
