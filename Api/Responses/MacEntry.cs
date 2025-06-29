using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

/// <summary>
/// Represents a MAC entry in the UniFi network
/// </summary>
public class MacEntry
{
    [JsonPropertyName("hostname")]
    public string? Hostname { get; set; }

    [JsonPropertyName("age")]
    public int? Age { get; set; }

    [JsonPropertyName("ip")]
    public string? IP { get; set; }

    [JsonPropertyName("mac")]
    public string? Mac { get; set; }

    [JsonPropertyName("static")]
    public bool? Static { get; set; }

    [JsonPropertyName("uptime")]
    public double? Uptime { get; set; }

    [JsonPropertyName("vlan")]
    public int? Vlan { get; set; }

    [JsonPropertyName("is_only_station_on_port")]
    public bool? Is_only_station_on_port { get; set; }
}
