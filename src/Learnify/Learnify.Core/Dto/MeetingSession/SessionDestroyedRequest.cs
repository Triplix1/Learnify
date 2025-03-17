using System.Text.Json.Serialization;

namespace Learnify.Core.Dto.MeetingSession;

public class SessionDestroyedRequest
{
    [JsonPropertyName("sessionId")]
    public string SessionId { get; set; }

    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; }
}