namespace PortalDoPublicador.Shared.Infrastructure.Sync;

public interface ISyncable
{
    Guid Id { get; set; }
    int? RowVersion { get; set; }
}
