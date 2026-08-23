using System.ComponentModel.DataAnnotations.Schema;
using Mapster;
using PortalDoPublicador.Shared.Features.Designacoes;

namespace PortalDoPublicador.Shared.Features.Perfis;

[AdaptTo("[name]FormDto")]
public class Usuario
{
    // Dados Pessoais Básicos
    public Guid Id { get; set; }
    public required string NomeCompleto { get; set; }
    public required string NomeExibicao { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool IsMasculino { get; set; } = true;
    public required string Email { get; set; }
    public required string Telefone { get; set; }
    public required string Endereco { get; set; }
    public required string SenhaHash { get; set; }
    
    // Auto-Relacionamento de Família
    public Guid? ChefeFamiliaId { get; set; }
    [ForeignKey("ChefeFamiliaId")]
    public Usuario? ChefeFamilia { get; set; }
    
    // Dados Teocráticos (Antigo Perfil)
    public SituacaoEspiritual SituacaoEspiritual { get; set; }
    public DateTime? DataBatismo { get; set; }
    
    [InverseProperty("Membros")]
    public Grupo? Grupo { get; set; }
    
    public List<Pioneiro> HistoricoPioneiro { get; set; } = [];
    
    public PrivilegioEspiritual Permissoes { get; set; }
    
    public TipoResponsabilidade ResponsabilidadesTitular { get; set; }
    public TipoResponsabilidade ResponsabilidadesAjudante { get; set; }
    
    public List<Relatorio> Relatorios { get; set; } = [];
}
