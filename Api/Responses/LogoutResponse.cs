using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

/// <summary>
/// Logout response
/// </summary>
public class LogoutResponse : BaseResponse
{
    /// <summary>
    /// Notes if logout was successful or not
    /// </summary>
    [JsonPropertyName("Success")]
    public bool Success { get; set; }
}
