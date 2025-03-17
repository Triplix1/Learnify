using System.Text.Json.Serialization;

namespace Learnify.Core.Dto.MeetingConnection;

public class ConnectionInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; }
    
    [JsonPropertyName("data")]
    public string Data { get; set; }
}