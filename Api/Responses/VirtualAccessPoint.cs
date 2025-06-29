using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

/// <summary>
/// Details of a UniFi virtual access point (VAP) definition
/// </summary>
public class VirtualAccessPoint : BaseResponse
{
    /// <summary>
    /// MAC address of the VAP
    /// </summary>
    [JsonPropertyName("bssid")]
    public string? BssId { get; set; }

    /// <summary>
    /// Name of the network
    /// </summary>
    [JsonPropertyName("essid")]
    public string? Essid { get; set; }

    /// <summary>
    /// The channel on which the network operates
    /// </summary>
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}