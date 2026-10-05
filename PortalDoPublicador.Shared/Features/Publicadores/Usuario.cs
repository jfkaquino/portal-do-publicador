using PortalDoPublicador.Shared.Features.Designacoes;
using PortalDoPublicador.Shared.Features.Publicadores.Enums;

namespace PortalDoPublicador.Shared.Features.Publicadores;

public class Usuario
{
    // Dados Pessoais Básicos
    public Guid Id { get; set; }
    public required string NomeCompleto { get; set; }
    public required string NomeExibicao { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool IsMasculino { get; set; } = true;
    public string? Email { get; set; }
    public required string Telefone { get; set; }
    public required string Endereco { get; set; }
    public string? SenhaHash { get; set; }

    public Guid? ChefeFamiliaId { get; set; }
    public Usuario? ChefeFamilia { get; set; }

    public SituacaoEspiritual SituacaoEspiritual { get; set; }
    public DateTime? DataBatismo { get; set; }

    public Grupo? Grupo { get; set; }

    public List<Pioneiro> HistoricoPioneiro { get; set; } = [];

    public List<TipoDesignacao> Qualificacoes { get; set; } = [];

    public TipoResponsabilidade ResponsabilidadesTitular { get; set; }
    public TipoResponsabilidade ResponsabilidadesAjudante { get; set; }

    public List<Relatorio> Relatorios { get; set; } = [];
}
