using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

public class Profile : BaseResponse
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
