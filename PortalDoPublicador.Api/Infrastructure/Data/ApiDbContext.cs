using Microsoft.EntityFrameworkCore;
using PortalDoPublicador.Shared.Infrastructure.Data;

namespace PortalDoPublicador.Api.Infrastructure.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : SharedDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
