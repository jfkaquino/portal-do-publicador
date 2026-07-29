namespace PortalDoPublicador.Shared.Features.Perfis;

public class Familia
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    
    public List<Usuario> Usuarios { get; set; } = [];
}
