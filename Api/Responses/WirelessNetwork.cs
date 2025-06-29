using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses;

/// <summary>
/// Details of a UniFi wireless network definition
/// </summary>
public class WirelessNetwork : BaseResponse
{
    /// <summary>
    /// Unique network ID
    /// </summary>
    [JsonPropertyName("_id")]
    public string Id { get; set; }

    /// <summary>
    /// Name of the network
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Is this network enabled?
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Security level (Open, WEP, WPA, etc.)
    /// </summary>
    [JsonPropertyName("security")]
    public string Security { get; set; }

    /// <summary>
    /// In case of WPA, which mode (WPA2, Enterprise, etc.)
    /// </summary>
    [JsonPropertyName("wpa_mode")]
    public string WPAMode { get; set; }

    /// <summary>
    /// In case of WPA, which encryption method (CCMP, etc.)
    /// </summary>
    [JsonPropertyName("wpa_enc")]
    public string WPAEncryption { get; set; }

    /// <summary>
    /// Id of the User Group
    /// </summary>
    [JsonPropertyName("usergroup_id")]
    public string UserGroupId { get; set; }

    /// <summary>
    /// Is a MAC-Filter enabled?
    /// </summary>
    [JsonPropertyName("mac_filter_enabled")]
    public bool? IsMACFilterEnabled { get; set; }

    /// <summary>
    /// Default MAC-Filter policy
    /// </summary>
    [JsonPropertyName("mac_filter_policy")]
    public string MACFilterPolicy { get; set; }

    /// <summary>
    /// Is VLAN enabled?
    /// </summary>
    [JsonPropertyName("vlan_enabled")]
    public bool IsVLANEnabled { get; set; }

    /// <summary>
    /// If VLAN is enabled, VLAN number
    /// </summary>
    [JsonPropertyName("vlan")]
    public string VLAN { get; set; }

    /// <summary>
    /// Is this SSID hidden?
    /// </summary>
    [JsonPropertyName("hide_ssid")]
    public bool IsSSIDHidden { get; set; }

    /// <summary>
    /// Id of the WLAN Group
    /// </summary>
    [JsonPropertyName("wlangroup_id")]
    public string WLANGroupId { get; set; }

    /// <summary>
    /// Id of the RADIUS Profile
    /// </summary>
    [JsonPropertyName("radiusprofile_id")]
    public string RadiusProfileId { get; set; }
}