using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Details of a UniFi network definition
    /// </summary>
    public class Network : BaseResponse
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
        /// Identifier of the site in UniFi to which this client is connected
        /// </summary>
        [JsonPropertyName("site_id")]
        public string SiteId { get; set; }

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
        /// Purpose of this network (corporate, guest, vlan only, etc.)
        /// </summary>
        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// IP-Subnet (CIDR)
        /// </summary>
        [JsonPropertyName("ip_subnet")]
        public string IpSubnet { get; set; }

        /// <summary>
        /// Domain Name of this Network
        /// </summary>
        [JsonPropertyName("domain_name")]
        public string DomainName { get; set; }

        /// <summary>
        /// Is NAT enabled?
        /// </summary>
        [JsonPropertyName("is_nat")]
        public bool? IsNAT { get; set; }

        /// <summary>
        /// Is DHCP enabled?
        /// </summary>
        [JsonPropertyName("dhcpd_enabled")]
        public bool? IsDHCPEnabled { get; set; }

        /// <summary>
        /// DHCP Start IP Address
        /// </summary>
        [JsonPropertyName("dhcpd_start")]
        public string DHCPStart { get; set; }

        /// <summary>
        /// DHCP End IP Address
        /// </summary>
        [JsonPropertyName("dhcpd_stop")]
        public string DHCPStop { get; set; }

        /// <summary>
        /// Network Group
        /// </summary>
        [JsonPropertyName("networkgroup")]
        public string NetworkGroup { get; set; }

        /// <summary>
        /// Internal identifier of the site
        /// </summary>
        [JsonPropertyName("attr_hidden_id")]
        public string HiddenId { get; set; }

        /// <summary>
        /// Boolean indicating if deletion of this site is being disallowed
        /// </summary>
        [JsonPropertyName("attr_no_delete")]
        public bool? DontAllowDeletion { get; set; }
    }
}