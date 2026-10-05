using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PortalDoPublicador.Api.Infrastructure.Data;
using PortalDoPublicador.Shared.Infrastructure.Sync;

namespace PortalDoPublicador.Api.Features;

public static class SyncEndpoint
{
    public static void MapSyncEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sync", async (
            [FromBody] SyncRequest requisicao,
            ServerDbContext context,
            IServiceProvider serviceProvider) =>
        {
            // 1. Mapeamento de Tipos Seguros (Assim como fizemos no client)
            var entityTypes = context.Model.GetEntityTypes()
                .Select(t => t.ClrType)
                .Where(t => typeof(ISyncable).IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // 2. PROCESSAMENTO DO PUSH (O que veio do celular)
            if (requisicao.DadosPush is not null && requisicao.DadosPush.Count > 0)
            {
                // Agrupamos por tabela para ganhar performance no servidor também
                var pacotesPorTabela = requisicao.DadosPush.GroupBy(p => p.EntityName);

                foreach (var grupo in pacotesPorTabela)
                {
                    if (!entityTypes.TryGetValue(grupo.Key, out var entityType)) continue;

                    foreach (var payload in grupo)
                    {
                        var entidadeBanco = (await context.FindAsync(entityType, payload.EntityId)) as ISyncable;

                        // Descobrindo a intenção baseada nos Deltas
                        var isInsert = payload.EntityChanges.Count > 0 && payload.PreviousValues.Count == 0;
                        var isUpdate = payload.EntityChanges.Count > 0 && payload.PreviousValues.Count > 0;
                        var isDelete = payload.EntityChanges.Count == 0 && payload.PreviousValues.Count > 0;

                        if (isInsert)
                        {
                            // Prevenção de Duplo-Envio: Só insere se não existir no banco oficial
                            if (entidadeBanco == null)
                            {
                                entidadeBanco = (ISyncable)Activator.CreateInstance(entityType)!;
                                entidadeBanco.Id = payload.EntityId;

                                var entry = context.Entry(entidadeBanco);
                                AplicarMudancas(entry, payload.EntityChanges, options);

                                context.Add(entidadeBanco);
                            }
                        }
                        else if (isUpdate)
                        {
                            // Só atualiza se o registro ainda existir no banco
                            if (entidadeBanco != null)
                            {
                                // TODO: Aqui entraria a Resolução de Conflitos (RowVersion check)
                                var entry = context.Entry(entidadeBanco);
                                AplicarMudancas(entry, payload.EntityChanges, options);
                            }
                        }
                        else if (isDelete)
                        {
                            if (entidadeBanco != null)
                            {
                                context.Remove(entidadeBanco);
                            }
                        }
                    }
                }

                // Salva todas as alterações recebidas de uma vez no banco oficial
                await context.SaveChangesAsync();
            }

            // 3. PROCESSAMENTO DO PULL (O que o servidor precisa mandar de volta)
            List<SyncPayload> payloadsParaDevolver = [];

            // TODO: Lógica para buscar registros alterados no banco desde requisicao.LastSync

            var response = new SyncResponse
            {
                DadosPull = payloadsParaDevolver,
                Timestamp = DateTime.UtcNow
            };

            return Results.Ok(response);
        });
    }

    // Método auxiliar para manter o código limpo
    private static void AplicarMudancas(
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry,
        Dictionary<string, object> mudancas,
        JsonSerializerOptions options)
    {
        foreach (var mudanca in mudancas)
        {
            var propEntry = entry.Property(mudanca.Key);
            if (propEntry != null && mudanca.Value is JsonElement jsonElement)
            {
                propEntry.CurrentValue = jsonElement.Deserialize(propEntry.Metadata.ClrType, options);
            }
        }
    }
}