namespace PortalDoPublicador.Shared.Infrastructure.Sync;

public class SyncPayload
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required DateTime Timestamp { get; init; }
    public Guid EntityId { get; init; }
    public required string EntityName { get; init; }
    public required Dictionary<string, object> EntityChanges { get; init; }
    public required Dictionary<string, object> PreviousValues { get; init; }
    public required int? RowVersion { get; init; }
}