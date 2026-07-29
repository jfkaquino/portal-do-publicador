using Mapster;

namespace PortalDoPublicador.Shared.Features.Perfis;

[AdaptTo("[name]FormDto")]
public class Usuario
{
    public Guid Id { get; set; }
    public required string NomeCompleto { get; set; }
    public required string NomeExibicao { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool IsMasculino { get; set; } = true;
    public required string Email { get; set; }
    public required string Telefone { get; set; }
    public required string Endereco { get; set; }
    public Familia? Familia { get; set; }
    public required Perfil? Perfil { get; set; }
}
