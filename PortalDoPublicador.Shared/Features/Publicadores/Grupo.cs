namespace PortalDoPublicador.Shared.Features.Publicadores;

public class Grupo
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }

    public Usuario? Superintendente { get; set; }

    public Usuario? Ajudante { get; set; }

    public List<Usuario> Membros { get; set; } = [];
}
