using PortalDoPublicador.Client.Infrastructure;
using PortalDoPublicador.Shared.Features.Perfis;
using Microsoft.EntityFrameworkCore;

namespace PortalDoPublicador.Client.Features.Usuarios;

public class UsuariosService(ClientDbContext context)
{
    public async Task<List<Usuario>> GetUsuariosAsync()
    {
        return await context.Usuarios
            .Include(u => u.Perfil)
            .ToListAsync();
    }

    public async Task CriarUsuarioAsync(Usuario usuario)
    {
        if (usuario.Id == Guid.Empty)
            usuario.Id = Guid.NewGuid();

        if (usuario.Perfil is not null)
        {
            if (usuario.Perfil.Id == Guid.Empty)
                usuario.Perfil.Id = Guid.NewGuid();
            usuario.Perfil.Usuario = usuario;
        }

        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
    }
}
