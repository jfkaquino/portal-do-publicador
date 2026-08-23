using System;

namespace PortalDoPublicador.Shared.Features.Perfis;

[Flags]
public enum TipoResponsabilidade
{
    Nenhuma = 0,
    Audio = 1,
    Microfones = 2,
    Palco = 4,
    Indicador = 8,
    Limpeza = 16,
    Jardim = 32,
    Contas = 64,
    Literatura = 128,
    Territorios = 256,
    CapitaoGrupo = 512,
    AjudanteGrupo = 1024
}
