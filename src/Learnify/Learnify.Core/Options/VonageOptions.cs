using System.Text.Json.Serialization;

namespace Learnify.Core.Options;

public class VonageOptions
{
    [JsonPropertyName("Application.Id")]
    public string ApplicationId { get; set; }

    [JsonPropertyName("Application.Key")]
    public string ApplicationKey { get; set; }
}