using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

/// <summary>
/// Represents a MAC Table list in the UniFi network
/// </summary>
public class MacTable : BaseResponse
{
    /// <summary>
    /// The MAC address of the UniFi devices for which the table is shown, i.e. a switch
    /// </summary>
    [JsonPropertyName("mac")]
    public string? MacAddress { get; set; }

    /// <summary>
    /// The ports on the UniFi device and the MAC entries of the connected devices to the port
    /// </summary>
    [JsonPropertyName("ports")]
    public List<Port>? Ports { get; set; }
}
