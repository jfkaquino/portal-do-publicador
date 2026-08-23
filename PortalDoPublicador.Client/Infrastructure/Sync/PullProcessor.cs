using System.Reflection;
using System.Text.Json;
using DnetIndexedDb;
using Microsoft.EntityFrameworkCore;
using PortalDoPublicador.Shared.Infrastructure.Sync;

namespace PortalDoPublicador.Client.Infrastructure.Sync;

public class PullProcessor(IndexedDbInterop indexedDb, IServiceScopeFactory scopeFactory)
{
    public async Task ProcessarFilaPullAsync()
    {
        await indexedDb.OpenIndexedDb();
        var payloads = await indexedDb.GetAll<SyncPayload>("SyncPullQueue");

        if (payloads is null or { Count: 0 })
        {
            return;
        }

        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ClientDbContext>();
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        try
        {
            var entityTypes = context.Model.GetEntityTypes()
                .Select(t => t.ClrType)
                .Where(t => typeof(ISyncable).IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var payloadsProcessados = new List<Guid>(payloads.Count);

            var pacotesPorTabela = payloads.GroupBy(p => p.EntityName);

            // Resgata o método de pré-carregamento para usarmos via Reflexão
            var preLoadMethodInfo = GetType().GetMethod(
                nameof(PrecarregarEntidadesAsync),
                BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var grupo in pacotesPorTabela)
            {
                if (!entityTypes.TryGetValue(grupo.Key, out var entityType))
                {
                    payloadsProcessados.AddRange(grupo.Select(p => p.Id));
                    continue;
                }

                var efEntityType = context.Model.FindEntityType(entityType);

                // ====================================================================
                // OTIMIZAÇÃO: SOLUÇÃO DO GARGALO N+1
                // Pré-carrega todas as entidades que serão afetadas neste lote
                // ====================================================================
                var pkProperty = efEntityType?.FindPrimaryKey()?.Properties.FirstOrDefault();
                if (pkProperty != null && preLoadMethodInfo != null)
                {
                    var idsDoGrupo = grupo.Select(p => (object)p.EntityId!).Distinct().ToList();

                    // Invoca o método dinâmico com o Tipo da Entidade e o Tipo da Chave (ex: <Publicador, Guid>)
                    var preLoadMethod = preLoadMethodInfo.MakeGenericMethod(entityType, pkProperty.ClrType);

                    // Passa a lista de IDs, o nome da propriedade primária, e o context local
                    await (Task)preLoadMethod.Invoke(this, new object[] { idsDoGrupo, pkProperty.Name, context })!;
                }
                // ====================================================================

                foreach (var payload in grupo)
                {
                    // Como pré-carregamos acima, o FindAsync NÃO VAI fazer requisição ao banco de dados!
                    // Ele encontra a entidade a custo zero direto na memória (no ChangeTracker).
                    var entidadeLocal = await context.FindAsync(entityType, payload.EntityId) as ISyncable;

                    if (payload.EntityChanges.Count == 0 && payload.PreviousValues.Count == 0)
                    {
                        if (entidadeLocal != null)
                        {
                            context.Remove(entidadeLocal);
                        }
                    }
                    else
                    {
                        if (entidadeLocal == null)
                        {
                            var novaEntidade = (ISyncable)Activator.CreateInstance(entityType)!;
                            novaEntidade.Id = payload.EntityId;

                            entidadeLocal = novaEntidade;
                            context.Add(entidadeLocal);
                        }

                        var entry = context.Entry(entidadeLocal);

                        foreach (var mudanca in payload.EntityChanges)
                        {
                            var propertyMetadata = efEntityType?.FindProperty(mudanca.Key);
                            if (propertyMetadata != null && mudanca.Value is JsonElement jsonElement)
                            {
                                var propEntry = entry.Property(mudanca.Key);
                                propEntry.CurrentValue = jsonElement.Deserialize(propertyMetadata.ClrType, options);
                            }
                        }

                        if (payload.RowVersion.HasValue)
                        {
                            var rowVersionProp = entry.Property("RowVersion");
                            if (rowVersionProp != null)
                            {
                                rowVersionProp.CurrentValue = payload.RowVersion.Value;
                            }
                        }
                    }

                    payloadsProcessados.Add(payload.Id);
                }
            }

            // O SaveChanges envia todos os inserts/updates/deletes num pacote só
            await context.SaveLocalChangesAsync();

            if (payloadsProcessados.Count > 0)
            {
                var tarefasDelecao = payloadsProcessados.Select(id =>
                    indexedDb.DeleteByKey<Guid>("SyncPullQueue", id).AsTask()
                );

                await Task.WhenAll(tarefasDelecao);
            }
        }
        finally
        {
            context.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    /// <summary>
    /// Método auxiliar injetado via reflexão para fazer o download das entidades
    /// em lote de forma strongly-typed, aproveitando as features do EF Core.
    /// </summary>
    private async Task PrecarregarEntidadesAsync<TEntity, TKey>(IEnumerable<object> ids, string keyName, ClientDbContext context)
        where TEntity : class
    {
        // 1. Converte o IEnumerable<object> bruto para uma lista fortemente tipada (ex: List<Guid> ou List<int>)
        var idList = ids.Select(id =>
        {
            if (id is TKey typedId) return typedId;
            if (typeof(TKey) == typeof(Guid)) return (TKey)(object)Guid.Parse(id.ToString()!);
            return (TKey)Convert.ChangeType(id, typeof(TKey));
        }).ToList();

        // 2. Dividimos a query em lotes (chunks) de 500 registros. 
        // Isso evita que o SQLite/SQL Server retorne erro de "Excesso de parâmetros na cláusula IN".
        const int tamanhoLote = 500;

        for (var i = 0; i < idList.Count; i += tamanhoLote)
        {
            var lote = idList.Skip(i).Take(tamanhoLote).ToList();

            // Usamos EF.Property para não depender de uma interface específica.
            // O LoadAsync puxa tudo do banco num único SELECT IN (...) e pendura no tracking da memória.
            await context.Set<TEntity>()
                .Where(e => lote.Contains(EF.Property<TKey>(e, keyName)))
                .LoadAsync();
        }
    }
}