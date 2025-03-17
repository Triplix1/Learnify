using System.Text.Json.Serialization;

namespace Learnify.Core.Dto.MeetingConnection;

public class ConnectionCreatedRequest
{
    [JsonPropertyName("sessionId")]
    public string SessionId { get; set; }
    
    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; }
    
    [JsonPropertyName("event")]
    public string Event { get; set; }
    
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    
    [JsonPropertyName("connection")]
    public ConnectionInfo Connection { get; set; }
}