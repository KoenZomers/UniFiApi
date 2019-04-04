using Newtonsoft.Json;
using System;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Defines one client session having been connected to the UniFi network
    /// </summary>
    public class ClientSession : BaseResponse
    {
        /// <summary>
        /// Unique identifier of the session
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        /// Total time in seconds the client has been connected to the network thus far
        /// </summary>
        [JsonProperty(PropertyName = "assoc_time")]
        public long? TotalConnectedTime { get; set; }

        /// <summary>
        /// Total time in a TimeSpan the client has been connected to the network thus far
        /// </summary>
        public TimeSpan? TotalConnectedTimeSpan { get { return TotalConnectedTime.HasValue ? (TimeSpan?) TimeSpan.FromSeconds(TotalConnectedTime.Value) : null; } }

        /// <summary>
        /// Duration of the current session of the client in seconds
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public long? SessionDuration { get; set; }

        /// <summary>
        /// Duration of the current session of the client as a TimeSpan
        /// </summary>
        public TimeSpan? SessionDurationTimeSpan { get { return SessionDuration.HasValue ? (TimeSpan?) TimeSpan.FromSeconds(SessionDuration.Value) : null; } }

        /// <summary>
        /// Amount of bytes received by the client through the UniFi network
        /// </summary>
        [JsonProperty(PropertyName = "tx_bytes")]
        public long? TransmittedBytes{ get; set; }

        /// <summary>
        /// Amount of bytes uploaded by the client through the UniFi network
        /// </summary>
        [JsonProperty(PropertyName = "rx_bytes")]
        public long? ReceivedBytes { get; set; }

        /// <summary>
        /// MAC Address of the client that was connected
        /// </summary>
        [JsonProperty(PropertyName = "mac")]
        public string ClientMacAddress { get; set; }

        /// <summary>
        /// Name of the client as registered in UniFi
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Name of the device as broadcasted by the device itself
        /// </summary>
        [JsonProperty(PropertyName = "hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Was this client logged on through a guest network
        /// </summary>
        [JsonProperty(PropertyName = "is_guest")]
        public bool? IsGuest { get; set; }

        /// <summary>
        /// IP Address assigned to the client
        /// </summary>
        [JsonProperty(PropertyName = "ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Was this client wired to the UniFi network (true) or wirelessly connected (false)
        /// </summary>
        [JsonProperty(PropertyName = "is_wired")]
        public bool IsWired { get; set; }

        /// <summary>
        /// MAC Address of the Access Point the client was connected to
        /// </summary>
        [JsonProperty(PropertyName = "ap_mac")]
        public string AccessPointMacAddress { get; set; }

        /// <summary>
        /// Meaning unknown
        /// </summary>
        [JsonProperty(PropertyName = "o")]
        public string O { get; set; }

        /// <summary>
        /// Meaning unknown
        /// </summary>
        [JsonProperty(PropertyName = "oid")]
        public string Oid { get; set; }
    }
}
