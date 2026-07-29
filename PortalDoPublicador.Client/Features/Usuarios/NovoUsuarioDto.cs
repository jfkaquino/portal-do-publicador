using PortalDoPublicador.Shared.Features.Perfis;

namespace PortalDoPublicador.Client.Features.Usuarios;

public class NovoUsuarioDto
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string NomeExibicao { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
    public bool IsMasculino { get; set; } = true;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public SituacaoEspiritual SituacaoEspiritual { get; set; }
    public ModalidadePioneiro ModalidadePioneiro { get; set; }
    public Familia? Familia { get; set; }
}
