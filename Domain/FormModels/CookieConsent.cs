

using System.Text.Json.Serialization;

namespace Domain.FormModels;

public class CookieConsent
{

    [JsonPropertyName("darkmode")]
    public bool DarkMode { get; set; }
    
    [JsonPropertyName("essential")]
    public bool Essential { get; set; }

    [JsonPropertyName("functional")]
    public bool Functional { get; set; }

    [JsonPropertyName("analytics")]
    public bool Analytics { get; set; }

    [JsonPropertyName("marketing")]
    public bool Marketing { get; set; }
}
