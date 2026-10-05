using PortalDoPublicador.Shared.Features.Publicadores.Enums;

namespace PortalDoPublicador.Shared.Features.Publicadores;

public class Pioneiro
{
    public Guid Id { get; set; }

    public required Usuario Usuario { get; set; }

    public ModalidadePioneiro ModalidadePioneiro { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}