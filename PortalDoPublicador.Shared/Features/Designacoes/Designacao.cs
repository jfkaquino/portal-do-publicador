using PortalDoPublicador.Shared.Features.Eventos;
using PortalDoPublicador.Shared.Features.Perfis;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalDoPublicador.Shared.Features.Designacoes;

public class Designacao
{
    public Guid Id { get; set; }
    
    // Quem fará
    [ForeignKey("UsuarioId")]
    public required Usuario Usuario { get; set; }
    
    // O que fará
    [ForeignKey("TipoDesignacaoId")]
    public required TipoDesignacao Tipo { get; set; }
    
    public bool Confirmado { get; set; }
    public DateTime DataAtribuicao { get; set; } = DateTime.UtcNow;
    
    // Onde/Quando fará (Opcionais - Preenchido conforme o contexto)
    
    [ForeignKey("ReuniaoId")]
    public Reuniao? Reuniao { get; set; }
    
    [ForeignKey("SaidaServicoCampoId")]
    public SaidaServicoCampo? SaidaServicoCampo { get; set; }
    
    [ForeignKey("ParteId")]
    public Parte? Parte { get; set; }
    
    [ForeignKey("GrupoId")]
    public Grupo? Grupo { get; set; }
}
