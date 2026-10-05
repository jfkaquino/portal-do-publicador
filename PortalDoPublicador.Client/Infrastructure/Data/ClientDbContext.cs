using DnetIndexedDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.JSInterop;
using PortalDoPublicador.Shared.Infrastructure.Data;
using PortalDoPublicador.Shared.Infrastructure.Sync;

namespace PortalDoPublicador.Client.Infrastructure.Data;

public class ClientDbContext(DbContextOptions<ClientDbContext> options, IndexedDbInterop indexedDb, IJSRuntime jsRuntime) : SharedDbContext(options)
{
    public async override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        if (Database.CurrentTransaction != null)
        {
            return await base.SaveChangesAsync(ct);
        }

        using var transaction = await Database.BeginTransactionAsync(ct);

        try
        {
            var entradasModificadas = ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            var datetime = DateTime.UtcNow;
            List<SyncPayload> syncPayloads = [];

            foreach (var entry in entradasModificadas)
            {
                Dictionary<string, object> changes = [];
                Dictionary<string, object> previous = [];

                var propriedadesAlvo = entry.Properties
                    .Where(p => !p.Metadata.IsShadowProperty() || p.Metadata.IsForeignKey());

                if (entry.State == EntityState.Modified)
                {
                    propriedadesAlvo = propriedadesAlvo.Where(p => p.IsModified);
                }

                foreach (var prop in propriedadesAlvo)
                {
                    var nomeColuna = prop.Metadata.Name;

                    if (entry.State is EntityState.Added or EntityState.Modified)
                    {
                        changes[nomeColuna] = prop.CurrentValue!;
                    }

                    if (entry.State is EntityState.Deleted or EntityState.Modified)
                    {
                        previous[nomeColuna] = prop.OriginalValue!;
                    }
                }

                syncPayloads.Add(new SyncPayload
                {
                    Timestamp = datetime,
                    EntityId = (Guid)entry.Property("Id").CurrentValue!,
                    EntityName = entry.Metadata.ClrType.Name,
                    EntityChanges = changes,
                    PreviousValues = previous,
                    RowVersion = entry.Metadata.FindProperty("RowVersion") != null ? entry.Property("RowVersion").CurrentValue as int? : null
                });
            }

            var result = await base.SaveChangesAsync(ct);

            if (syncPayloads.Count != 0)
            {
                await indexedDb.OpenIndexedDb();
                await indexedDb.AddItems("SyncPushQueue", syncPayloads);
                await transaction.CommitAsync(ct);
                await jsRuntime.InvokeVoidAsync("SincronizacaoOffline.registrarSync");
            }
            else
            {
                await transaction.CommitAsync(ct);
            }

            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<int> SaveLocalChangesAsync(CancellationToken ct = default)
    {
        var result = await base.SaveChangesAsync(ct);
        return result;
    }


}