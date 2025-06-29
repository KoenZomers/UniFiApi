using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

/// <summary>
/// Represents a MAC entry in the UniFi network
/// </summary>
public class Port : BaseResponse
{
    [JsonPropertyName("port_idx")]
    public int? Port_idx { get; set; }

    [JsonPropertyName("mac_table")]
    public List<MacEntry>? Mac_table { get; set; }
}
