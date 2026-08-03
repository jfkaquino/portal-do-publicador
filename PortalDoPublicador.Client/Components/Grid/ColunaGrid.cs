using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace PortalDoPublicador.Client.Components;

public class ColunaGrid<TItem>
{
    public string Titulo { get; set; } = string.Empty;
    public Func<TItem, string?>? Valor { get; set; }
    public RenderFragment<TItem>? Template { get; set; }
    public GridSort<TItem>? SortBy { get; set; }
}
