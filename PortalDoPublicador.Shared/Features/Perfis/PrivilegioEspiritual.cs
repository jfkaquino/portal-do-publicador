using System;

namespace PortalDoPublicador.Shared.Features.Perfis;

[Flags]
public enum PrivilegioEspiritual
{
    Nenhum = 0,
    Publicador = 1,
    PioneiroAuxiliar = 2,
    PioneiroRegular = 4,
    PioneiroEspecial = 8,
    ServoMinisterial = 16,
    Anciao = 32,
    Admin = 64
}
