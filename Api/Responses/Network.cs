using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the network
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Identifier of the site in UniFi to which this client is connected
        /// </summary>
        [JsonProperty(PropertyName = "site_id")]
        public string SiteId { get; set; }

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
        /// Purpose of this network (corporate, guest, vlan only, etc.)
        /// </summary>
        [JsonProperty(PropertyName = "purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// IP-Subnet (CIDR)
        /// </summary>
        [JsonProperty(PropertyName = "ip_subnet")]
        public string IpSubnet { get; set; }

        /// <summary>
        /// Domain Name of this Network
        /// </summary>
        [JsonProperty(PropertyName = "domain_name")]
        public string DomainName { get; set; }

        /// <summary>
        /// Is NAT enabled?
        /// </summary>
        [JsonProperty(PropertyName = "is_nat")]
        public bool? IsNAT { get; set; }

        /// <summary>
        /// Is DHCP enabled?
        /// </summary>
        [JsonProperty(PropertyName = "dhcpd_enabled")]
        public bool? IsDHCPEnabled { get; set; }

        /// <summary>
        /// DHCP Start IP Address
        /// </summary>
        [JsonProperty(PropertyName = "dhcpd_start")]
        public string DHCPStart { get; set; }

        /// <summary>
        /// DHCP End IP Address
        /// </summary>
        [JsonProperty(PropertyName = "dhcpd_stop")]
        public string DHCPStop { get; set; }

        /// <summary>
        /// Network Group
        /// </summary>
        [JsonProperty(PropertyName = "networkgroup")]
        public string NetworkGroup { get; set; }

        /// <summary>
        /// Internal identifier of the site
        /// </summary>
        [JsonProperty(PropertyName = "attr_hidden_id")]
        public string HiddenId { get; set; }

        /// <summary>
        /// Boolean indicating if deletion of this site is being disallowed
        /// </summary>
        [JsonProperty(PropertyName = "attr_no_delete")]
        public bool? DontAllowDeletion { get; set; }
    }
}