using System;
using System.Text.Json.Serialization;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Details about one client known to UniFi
    /// </summary>
    public class Clients : BaseResponse
    {
        // TODO: Add comments for each property

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("_is_guest_by_uap")]
        public bool? IsGuestByUap { get; set; }

        [JsonPropertyName("_last_seen_by_uap")]
        public long? LastSeenByUapRaw { get; set; }

        [JsonIgnore]
        public DateTime? LastSeenByUap
        {
            get { return LastSeenByUapRaw.HasValue ? (DateTime?)new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(LastSeenByUapRaw.Value).ToLocalTime() : null; }
            set { LastSeenByUapRaw = value.HasValue ? (long?)Math.Floor((value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) : null; }
        }

        /// <summary>
        /// Seconds the client already has been connected in its current session
        /// </summary>
        [JsonPropertyName("_uptime_by_uap")]
        public long? UptimeByUapRaw { get; set; }

        /// <summary>
        /// TimeSpan returning how long the client has already been connected in its current session
        /// </summary>
        [JsonIgnore]
        public TimeSpan? UptimeByUap
        {
            get { return UptimeByUapRaw.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(UptimeByUapRaw.Value) : null; }
            set { UptimeByUapRaw = value.HasValue ? (long?)value.Value.TotalSeconds : null; }
        }

        /// <summary>
        /// Mac address of the access point to which the client is connected
        /// </summary>
        [JsonPropertyName("ap_mac")]
        public string AccessPointMacAddress { get; set; }

        [JsonPropertyName("assoc_time")]
        public long? AssociatedTimeRaw { get; set; }

        [JsonIgnore]
        public DateTime? AssociatedTime
        {
            get { return AssociatedTimeRaw.HasValue ? (DateTime?)new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(AssociatedTimeRaw.Value).ToLocalTime() : null; }
            set { AssociatedTimeRaw = value.HasValue ? (long?)Math.Floor((value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) : null; }
        }

        /// <summary>
        /// Boolean indicating if the client is authorized on the UniFi network. Only has a meaning when the client is connected to a guest network which requires consent or login first.
        /// </summary>
        [JsonPropertyName("authorized")]
        public bool? IsAuthorized { get; set; }

        /// <summary>
        /// String providing information on what authorized this client
        /// </summary>
        [JsonPropertyName("authorized_by")]
        public string AuthorizedBy{ get; set; }

        [JsonPropertyName("bssid")]
        public string BssId { get; set; }

        [JsonPropertyName("bytes-r")]
        public double? BytesReceived { get; set; }

        [JsonPropertyName("ccq")]
        public int? Ccq { get; set; }

        /// <summary>
        /// The WiFi channel the client is connected to
        /// </summary>
        [JsonPropertyName("channel")]
        public int? Channel { get; set; }

        [JsonPropertyName("essid")]
        public string EssId { get; set; }

        /// <summary>
        /// The raw numeric value in Unix epoch time defining when the client was first seen on the UniFi network. Use FirstSeen to get a DateTime of this same value.
        /// </summary>
        [JsonPropertyName("first_seen")]
        public long? FirstSeenRaw { get; set; }

        /// <summary>
        /// Date/time indicating when this client was first seen on the UniFi network
        /// </summary>
        [JsonIgnore]
        public DateTime? FirstSeen
        {
            get { return FirstSeenRaw.HasValue ? (DateTime?)new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(FirstSeenRaw.Value).ToLocalTime() : null; }
            set { FirstSeenRaw = value.HasValue ? (long?)Math.Floor((value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) : null; }
        }

        /// <summary>
        /// Hostname as provided by the device
        /// </summary>
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Seconds the device has been idle without sending data through UniFi
        /// </summary>
        [JsonPropertyName("idletime")]
        public long? IdleTimeRaw { get; set; }

        /// <summary>
        /// TimeSpan representing the time the device has been idle without sending data through UniFi
        /// </summary>
        [JsonIgnore]
        public TimeSpan? IdleTime
        {
            get { return IdleTimeRaw.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(IdleTimeRaw.Value) : null; }
            set { IdleTimeRaw = value.HasValue ? (long?)value.Value.TotalSeconds : null; }
        }

        /// <summary>
        /// IP Address of the client on the network
        /// </summary>
        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Boolean indicating if the client is logged in through a guest portal
        /// </summary>
        [JsonPropertyName("is_guest")]
        public bool? IsGuest { get; set; }

        /// <summary>
        /// Boolean indicating if the client is currently blocked from accessing the UniFi network
        /// </summary>
        [JsonPropertyName("blocked")]
        public bool? IsBlocked { get; set; }

        /// <summary>
        /// Boolean indicating if this is a wired client (true) or a client connected through WiFi (false)
        /// </summary>
        [JsonPropertyName("is_wired")]
        public bool? IsWired { get; set; }

        /// <summary>
        /// Seconds since January 1, 1970 when this client last communicated with a UniFi device. Use LastSeen for a DateTime representing this value.
        /// </summary>
        [JsonPropertyName("last_seen")]
        public long? LastSeenRaw { get; set; }

        /// <summary>
        /// DateTime when this client last communicated with a UniFi device
        /// </summary>
        [JsonIgnore]
        public DateTime? LastSeen
        {
            get { return LastSeenRaw.HasValue ? (DateTime?)new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(LastSeenRaw.Value).ToLocalTime() : null; }
            set { LastSeenRaw = value.HasValue ? (long?)Math.Floor((value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) : null; }
        }

        /// <summary>
        /// Seconds since January 1, 1970 when this client last initiated a connection to a UniFi device. Use LatestAssociationTime for a DateTime representing this value.
        /// </summary>
        [JsonPropertyName("latest_assoc_time")]
        public long? LatestAssociationTimeRaw { get; set; }

        /// <summary>
        /// DateTime when this client last initiated a connection to a UniFi device
        /// </summary>
        [JsonIgnore]
        public DateTime? LatestAssociationTime
        {
            get { return LatestAssociationTimeRaw.HasValue ? (DateTime?)new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(LatestAssociationTimeRaw.Value).ToLocalTime() : null; }
            set { LatestAssociationTimeRaw = value.HasValue ? (long?)Math.Floor((value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) : null; }
        }

        /// <summary>
        /// The MAC Address of the client device
        /// </summary>
        [JsonPropertyName("mac")]
        public string MacAddress { get; set; }

        /// <summary>
        /// The friendly name assigned to the device through the Alias option
        /// </summary>
        [JsonPropertyName("name")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("noise")]
        public int? Noise { get; set; }

        [JsonPropertyName("noted")]
        public bool? IsNoted { get; set; }

        [JsonPropertyName("oui")]
        public string Brand { get; set; }

        [JsonPropertyName("powersave_enabled")]
        public bool? IsPowersaveEnabled { get; set; }

        [JsonPropertyName("qos_policy_applied")]
        public bool? IsQosPolicyApplied { get; set; }

        [JsonPropertyName("radio")]
        public string RadioBand { get; set; }

        [JsonPropertyName("radio_proto")]
        public string RadioProtocol { get; set; }

        [JsonPropertyName("rssi")]
        public int? SignalStrength { get; set; }

        [JsonPropertyName("rx_bytes")]
        public long? ReceivedBytesAllTime { get; set; }

        [JsonPropertyName("rx_bytes-r")]
        public float? ReceivedBytesSession { get; set; }

        [JsonPropertyName("rx_packets")]
        public long? ReceivedPackets { get; set; }

        [JsonPropertyName("rx_rate")]
        public long? ReceivedRate { get; set; }

        [JsonPropertyName("signal")]
        public long? Signal { get; set; }

        /// <summary>
        /// Seconds since January 1, 1970 when this authorized client becomes valid. Use StartDate for a DateTime representing this value.
        /// </summary>
        [JsonPropertyName("start")]
        public long? Start { get; set; }

        /// <summary>
        /// Date and time at which this client will become authorized
        /// </summary>
        public DateTime? StartDate => Start.HasValue ? new DateTime(1970, 1, 1).AddSeconds(Start.Value) : (DateTime?) null;

        /// <summary>
        /// Seconds since January 1, 1970 when this authorized client will no longer be valid. Use EndDate for a DateTime representing this value.
        /// </summary>
        [JsonPropertyName("end")]
        public long? End { get; set; }

        /// <summary>
        /// Date and time at which this client will no longer be authorized
        /// </summary>
        public DateTime? EndDate => End.HasValue ? new DateTime(1970, 1, 1).AddSeconds(End.Value) : (DateTime?)null;

        /// <summary>
        /// Identifier of the site in UniFi to which this client is connected
        /// </summary>
        [JsonPropertyName("site_id")]
        public string SiteId { get; set; }

        [JsonPropertyName("tx_bytes")]
        public double? TransmittedBytesAllTime { get; set; }

        [JsonPropertyName("tx_bytes-r")]
        public float? TransmittedBytesSession { get; set; }

        [JsonPropertyName("tx_packets")]
        public long? TransmittedPackets { get; set; }

        [JsonPropertyName("tx_power")]
        public long? TransmittedPower { get; set; }

        [JsonPropertyName("tx_rate")]
        public long? TransmittedRate { get; set; }

        /// <summary>
        /// Total seconds the client has been connected in its current session
        /// </summary>
        [JsonPropertyName("uptime")]
        public long? UptimeRaw { get; set; }

        /// <summary>
        /// TimeSpan representing the time the client has been connected in its current session
        /// </summary>
        [JsonIgnore]
        public TimeSpan? Uptime
        {
            get { return UptimeRaw.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(UptimeRaw.Value) : null; }
            set { UptimeRaw = value.HasValue ? (long?)value.Value.TotalSeconds : null; }
        }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("vlan")]
        public int? Vlan { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("usergroup_id")]
        public string UserGroupId { get; set; }

        /// <summary>
        /// Returns the friendly name of the client
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FriendlyName ?? Hostname;
        }
    }
}
