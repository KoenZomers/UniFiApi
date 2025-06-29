using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

public class Port_Overrides : BaseResponse
{
    [JsonPropertyName("port_idx")]
    public int? Port_idx { get; set; }

    [JsonPropertyName("portconf_id")]
    public string? Portconf_id { get; set; }

    [JsonPropertyName("op_mode")]
    public string? Op_mode { get; set; }

    [JsonPropertyName("aggregate_num_ports")]
    public int? Aggregate_num_ports { get; set; }
}
