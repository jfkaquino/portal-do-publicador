using Microsoft.EntityFrameworkCore;
using PortalDoPublicador.Shared.Infrastructure.Data;

namespace PortalDoPublicador.Api.Infrastructure.Data;

public class ServerDbContext(DbContextOptions<ServerDbContext> options) : SharedDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
