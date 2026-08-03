using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace PortalDoPublicador.Client.Components;

public class AcaoGrid
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Texto { get; set; } = string.Empty;
    public Icon? Icone { get; set; }
    public Appearance Aparencia { get; set; } = Appearance.Neutral;
    public EventCallback OnClick { get; set; }
}
