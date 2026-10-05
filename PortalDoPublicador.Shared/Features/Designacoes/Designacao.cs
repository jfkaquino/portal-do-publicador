using PortalDoPublicador.Shared.Features.Programacoes;
using PortalDoPublicador.Shared.Features.Publicadores;

namespace PortalDoPublicador.Shared.Features.Designacoes;

public abstract class Designacao
{
    public Guid Id { get; set; }

    public Guid? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public required TipoDesignacao Tipo { get; set; }

    public DateTime DataAtribuicao { get; set; } = DateTime.UtcNow;
    public bool Confirmado { get; set; }
}

public class DesignacaoReuniao : Designacao
{
    public Guid ReuniaoId { get; set; }
    public Reuniao Reuniao { get; set; } = null!;
}

public class DesignacaoParte : Designacao
{
    public Guid ParteId { get; set; }
    public Parte Parte { get; set; } = null!;
}

public class DesignacaoCampo : Designacao
{
    public Guid SaidaServicoCampoId { get; set; }
    public SaidaCampo SaidaServicoCampo { get; set; } = null!;
}

public class DesignacaoDiscurso : DesignacaoParte
{
    public string? Nome { get; set; }
    public string? Congregacao { get; set; }
}
