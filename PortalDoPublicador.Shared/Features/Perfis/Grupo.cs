using System.ComponentModel.DataAnnotations.Schema;

namespace PortalDoPublicador.Shared.Features.Perfis;

public class Grupo
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    
    [ForeignKey("SuperintendenteId")]
    public Usuario? Superintendente { get; set; }
    
    [ForeignKey("AjudanteId")]
    public Usuario? Ajudante { get; set; }
    
    public List<Usuario> Membros { get; set; } = [];
}
