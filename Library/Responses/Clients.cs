using Newtonsoft.Json;
using System;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Details about one client known to UniFi
    /// </summary>
    public class Clients : BaseResponse
    {
        // TODO: Add comments for each property

        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_is_guest_by_uap")]
        public bool? IsGuestByUap { get; set; }

        [JsonProperty(PropertyName = "_last_seen_by_uap")]
        public long? LastSeenByUapRaw { get; set; }

        [JsonIgnore]
        public TimeSpan? LastSeenByUap
        {
            get { return LastSeenByUapRaw.HasValue ? (TimeSpan?) TimeSpan.FromTicks(LastSeenByUapRaw.Value) : null; }
            set { LastSeenByUapRaw = value.HasValue ? (long?) value.Value.Ticks : null;  }
        }

        [JsonProperty(PropertyName = "_uptime_by_uap")]
        public long? UptimeByUapRaw { get; set; }

        [JsonIgnore]
        public TimeSpan? UptimeByUap
        {
            get { return UptimeByUapRaw.HasValue ? (TimeSpan?)TimeSpan.FromTicks(UptimeByUapRaw.Value) : null; }
            set { UptimeByUapRaw = value.HasValue ? (long?)value.Value.Ticks : null; }
        }

        [JsonProperty(PropertyName = "ap_mac")]
        public string AccessPointMacAddress { get; set; }

        [JsonProperty(PropertyName = "assoc_time")]
        public long? AssociatedTimeRaw { get; set; }

        [JsonIgnore]
        public TimeSpan? AssociatedTime
        {
            get { return AssociatedTimeRaw.HasValue ? (TimeSpan?)TimeSpan.FromTicks(AssociatedTimeRaw.Value) : null; }
            set { AssociatedTimeRaw = value.HasValue ? (long?)value.Value.Ticks : null; }
        }

        /// <summary>
        /// Boolean indicating if the client is authorized on the UniFi network. Only has a meaning when the client is connected to a guest network which requires consent or login first.
        /// </summary>
        [JsonProperty(PropertyName = "authorized")]
        public bool? IsAuthorized { get; set; }

        [JsonProperty(PropertyName = "bssid")]
        public string BssId { get; set; }

        [JsonProperty(PropertyName = "bytes-r")]
        public long? BytesReceived { get; set; }

        [JsonProperty(PropertyName = "ccq")]
        public int? Ccq { get; set; }

        /// <summary>
        /// The WiFi channel the client is connected to
        /// </summary>
        [JsonProperty(PropertyName = "channel")]
        public int? Channel { get; set; }

        [JsonProperty(PropertyName = "essid")]
        public string EssId { get; set; }

        /// <summary>
        /// The raw numeric value defining when the client was first seen on the UniFi network. Use FirstSeen to get a TimeSpan version of this same value.
        /// </summary>
        [JsonProperty(PropertyName = "first_seen")]
        public long? FirstSeenRaw { get; set; }

        /// <summary>
        /// TimeSpan indicating how long ago this client was first seen on the UniFi network
        /// </summary>
        [JsonIgnore]
        public TimeSpan? FirstSeen
        {
            get { return FirstSeenRaw.HasValue ? (TimeSpan?)TimeSpan.FromTicks(FirstSeenRaw.Value) : null; }
            set { FirstSeenRaw = value.HasValue ? (long?)value.Value.Ticks : null; }
        }

        /// <summary>
        /// Hostname as provided by the device
        /// </summary>
        [JsonProperty(PropertyName = "hostname")]
        public string Hostname { get; set; }

        [JsonProperty(PropertyName = "idletime")]
        public long? IdleTimeRaw { get; set; }

        [JsonIgnore]
        public TimeSpan? IdleTime
        {
            get { return IdleTimeRaw.HasValue ? (TimeSpan?)TimeSpan.FromTicks(IdleTimeRaw.Value) : null; }
            set { IdleTimeRaw = value.HasValue ? (long?)value.Value.Ticks : null; }
        }

        /// <summary>
        /// IP Address of the client on the network
        /// </summary>
        [JsonProperty(PropertyName = "ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Boolean indicating if the client is logged in through a guest portal
        /// </summary>
        [JsonProperty(PropertyName = "is_guest")]
        public bool? IsGuest { get; set; }

        /// <summary>
        /// Boolean indicating if the client is currently blocked from accessing the UniFi network
        /// </summary>
        [JsonProperty(PropertyName = "blocked")]
        public bool? IsBlocked { get; set; }

        [JsonProperty(PropertyName = "is_wired")]
        public bool? IsWired { get; set; }

        [JsonProperty(PropertyName = "last_seen")]
        public long? LastSeenRaw { get; set; }

        [JsonIgnore]
        public TimeSpan? LastSeen
        {
            get { return LastSeenRaw.HasValue ? (TimeSpan?)TimeSpan.FromTicks(LastSeenRaw.Value) : null; }
            set { LastSeenRaw = value.HasValue ? (long?)value.Value.Ticks : null; }
        }

        [JsonProperty(PropertyName = "latest_assoc_time")]
        public long? LatestAssociationTimeRaw { get; set; }

        [JsonIgnore]
        public TimeSpan? LatestAssociationTime
        {
            get { return LatestAssociationTimeRaw.HasValue ? (TimeSpan?)TimeSpan.FromTicks(LatestAssociationTimeRaw.Value) : null; }
            set { LatestAssociationTimeRaw = value.HasValue ? (long?)value.Value.Ticks : null; }
        }

        /// <summary>
        /// The MAC Address of the client device
        /// </summary>
        [JsonProperty(PropertyName = "mac")]
        public string MacAddress { get; set; }

        /// <summary>
        /// The friendly name assigned to the device through the Alias option
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string FriendlyName { get; set; }

        [JsonProperty(PropertyName = "noise")]
        public int? Noise { get; set; }

        [JsonProperty(PropertyName = "noted")]
        public bool? IsNoted { get; set; }

        [JsonProperty(PropertyName = "oui")]
        public string Brand { get; set; }

        [JsonProperty(PropertyName = "powersave_enabled")]
        public bool? IsPowersaveEnabled { get; set; }

        [JsonProperty(PropertyName = "qos_policy_applied")]
        public bool? IsQosPolicyApplied { get; set; }

        [JsonProperty(PropertyName = "radio")]
        public string RadioBand { get; set; }

        [JsonProperty(PropertyName = "radio_proto")]
        public string RadioProtocol { get; set; }

        [JsonProperty(PropertyName = "rssi")]
        public int? SignalStrength { get; set; }

        [JsonProperty(PropertyName = "rx_bytes")]
        public long? ReceivedBytesAllTime { get; set; }

        [JsonProperty(PropertyName = "rx_bytes-r")]
        public long? ReceivedBytesSession { get; set; }

        [JsonProperty(PropertyName = "rx_packets")]
        public long? ReceivedPackets { get; set; }

        [JsonProperty(PropertyName = "rx_rate")]
        public long? ReceivedRate { get; set; }

        [JsonProperty(PropertyName = "signal")]
        public long? Signal { get; set; }

        [JsonProperty(PropertyName = "site_id")]
        public string SiteId { get; set; }

        [JsonProperty(PropertyName = "tx_bytes")]
        public long? TransmittedBytesAllTime { get; set; }

        [JsonProperty(PropertyName = "tx_bytes-r")]
        public long? TransmittedBytesSession { get; set; }

        [JsonProperty(PropertyName = "tx_packets")]
        public long? TransmittedPackets { get; set; }

        [JsonProperty(PropertyName = "tx_power")]
        public long? TransmittedPower { get; set; }

        [JsonProperty(PropertyName = "tx_rate")]
        public long? TransmittedRate { get; set; }

        [JsonProperty(PropertyName = "uptime")]
        public long? UptimeRaw { get; set; }

        [JsonIgnore]
        public TimeSpan? Uptime
        {
            get { return UptimeRaw.HasValue ? (TimeSpan?)TimeSpan.FromTicks(UptimeRaw.Value) : null; }
            set { UptimeRaw = value.HasValue ? (long?)value.Value.Ticks : null; }
        }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "vlan")]
        public int? Vlan { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "usergroup_id")]
        public string UserGroupId { get; set; }

        /// <summary>
        /// Returns the friendly name of the client
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
