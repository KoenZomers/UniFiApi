﻿using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        /// <summary>
        /// Seconds since January 1, 1970 when the client started this session. Use SessionStartedAt for a DateTime representing this value.
        /// </summary>
        [JsonPropertyName("assoc_time")]
        public long? SessionStartedAtRaw { get; set; }

        /// <summary>
        /// DateTime when the client started this session
        /// </summary>
        [JsonIgnore]
        public DateTime? SessionStartedAt
        {
            get { return SessionStartedAtRaw.HasValue ? (DateTime?)new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(SessionStartedAtRaw.Value).ToLocalTime() : null; }
            set { SessionStartedAtRaw = value.HasValue ? (long?)Math.Floor((value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) : null; }
        }

        /// <summary>
        /// DateTime when the client ended this session. If this DateTime is close to the current date and time, it means that the session is still active.
        /// </summary>
        [JsonIgnore]
        public DateTime? SessionEndedAt => SessionStartedAt.HasValue && SessionDuration.HasValue ? (DateTime?) SessionStartedAt.Value.AddSeconds(SessionDuration.Value) : null;

        /// <summary>
        /// Duration of the current session of the client in seconds
        /// </summary>
        [JsonPropertyName("duration")]
        public long? SessionDuration { get; set; }

        /// <summary>
        /// Duration of the current session of the client as a TimeSpan
        /// </summary>
        public TimeSpan? SessionDurationTimeSpan { get { return SessionDuration.HasValue ? (TimeSpan?) TimeSpan.FromSeconds(SessionDuration.Value) : null; } }

        /// <summary>
        /// Amount of bytes received by the client through the UniFi network
        /// </summary>
        [JsonPropertyName("tx_bytes")]
        public long? TransmittedBytes{ get; set; }

        /// <summary>
        /// Amount of bytes uploaded by the client through the UniFi network
        /// </summary>
        [JsonPropertyName("rx_bytes")]
        public long? ReceivedBytes { get; set; }

        /// <summary>
        /// MAC Address of the client that was connected
        /// </summary>
        [JsonPropertyName("mac")]
        public string ClientMacAddress { get; set; }

        /// <summary>
        /// Name of the client as registered in UniFi
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Name of the device as broadcasted by the device itself
        /// </summary>
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Was this client logged on through a guest network
        /// </summary>
        [JsonPropertyName("is_guest")]
        public bool? IsGuest { get; set; }

        /// <summary>
        /// IP Address assigned to the client
        /// </summary>
        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Was this client wired to the UniFi network (true) or wirelessly connected (false)
        /// </summary>
        [JsonPropertyName("is_wired")]
        public bool IsWired { get; set; }

        /// <summary>
        /// MAC Address of the Access Point the client was connected to
        /// </summary>
        [JsonPropertyName("ap_mac")]
        public string AccessPointMacAddress { get; set; }

        /// <summary>
        /// Meaning unknown
        /// </summary>
        [JsonPropertyName("o")]
        public string O { get; set; }

        /// <summary>
        /// Meaning unknown
        /// </summary>
        [JsonPropertyName("oid")]
        public string Oid { get; set; }
    }
}
