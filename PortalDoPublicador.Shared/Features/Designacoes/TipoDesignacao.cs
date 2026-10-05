using PortalDoPublicador.Shared.Features.Designacoes.Enums;

namespace PortalDoPublicador.Shared.Features.Designacoes;

public class TipoDesignacao
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public CategoriaDesignacao Categoria { get; set; }
}
