using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

/// <summary>
/// Login response
/// </summary>
public class LoginResponse : BaseResponse
{
    /// <summary>
    /// Unique id of logged in user
    /// </summary>
    [JsonPropertyName("Unique_Id")]
    public string? UniqueId { get; set; }
}
