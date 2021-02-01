using Newtonsoft.Json;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Details of a UniFi wireless network definition
    /// </summary>
    public class WirelessNetwork : BaseResponse
    {
        /// <summary>
        /// Unique network ID
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the network
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Is this network enabled?
        /// </summary>
        [JsonProperty(PropertyName = "enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Security level (Open, WEP, WPA, etc.)
        /// </summary>
        [JsonProperty(PropertyName = "security")]
        public string Security { get; set; }

        /// <summary>
        /// In case of WPA, which mode (WPA2, Enterprise, etc.)
        /// </summary>
        [JsonProperty(PropertyName = "wpa_mode")]
        public string WPAMode { get; set; }

        /// <summary>
        /// In case of WPA, which encryption method (CCMP, etc.)
        /// </summary>
        [JsonProperty(PropertyName = "wpa_enc")]
        public string WPAEncryption { get; set; }

        /// <summary>
        /// Id of the User Group
        /// </summary>
        [JsonProperty(PropertyName = "usergroup_id")]
        public string UserGroupId { get; set; }

        /// <summary>
        /// Is a MAC-Filter enabled?
        /// </summary>
        [JsonProperty(PropertyName = "mac_filter_enabled")]
        public bool? IsMACFilterEnabled { get; set; }

        /// <summary>
        /// Default MAC-Filter policy
        /// </summary>
        [JsonProperty(PropertyName = "mac_filter_policy")]
        public string MACFilterPolicy { get; set; }

        /// <summary>
        /// Is VLAN enabled?
        /// </summary>
        [JsonProperty(PropertyName = "vlan_enabled")]
        public bool IsVLANEnabled { get; set; }

        /// <summary>
        /// If VLAN is enabled, VLAN number
        /// </summary>
        [JsonProperty(PropertyName = "vlan")]
        public string VLAN { get; set; }

        /// <summary>
        /// Is this SSID hidden?
        /// </summary>
        [JsonProperty(PropertyName = "hide_ssid")]
        public bool IsSSIDHidden { get; set; }

        /// <summary>
        /// Id of the WLAN Group
        /// </summary>
        [JsonProperty(PropertyName = "wlangroup_id")]
        public string WLANGroupId { get; set; }

        /// <summary>
        /// Id of the RADIUS Profile
        /// </summary>
        [JsonProperty(PropertyName = "radiusprofile_id")]
        public string RadiusProfileId { get; set; }
    }
}