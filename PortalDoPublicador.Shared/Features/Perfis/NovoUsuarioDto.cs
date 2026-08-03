using System;

namespace PortalDoPublicador.Shared.Features.Perfis;

public class NovoUsuarioDto
{
    public Guid? Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string NomeExibicao { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
    public bool IsMasculino { get; set; } = true;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public SituacaoEspiritual SituacaoEspiritual { get; set; }
    public ModalidadePioneiro ModalidadePioneiro { get; set; }
    public Guid? FamiliaId { get; set; }
}
