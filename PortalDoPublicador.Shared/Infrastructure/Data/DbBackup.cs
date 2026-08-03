using System.Text.Json.Serialization;

namespace PortalDoPublicador.Shared.Infrastructure.Data;

public class DbBackup
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "app.db";
    
    [JsonPropertyName("data")]
    public byte[] Data { get; set; } = Array.Empty<byte>();
}
