using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Details of a UniFi device such as an access point or switch
    /// </summary>
    public class Device : BaseResponse
    {
        /// <summary>
        /// Name of the device
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// MAC address of the device
        /// </summary>
        [JsonProperty(PropertyName = "mac")]
        public string MacAddress { get; set; }

        /// <summary>
        /// Boolean indicating if the device has been adopted by UniFi
        /// </summary>
        [JsonProperty(PropertyName = "adopted")]
        public bool? Adpoted { get; set; }

        /// <summary>
        /// Serial Number of the device
        /// </summary>
        [JsonProperty(PropertyName = "serial")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Device uptime in seconds
        /// </summary>
        [JsonProperty(PropertyName = "uptime")]
        public long? Uptime { get; set; }

        /// <summary>
        /// Device uptime as a TimeSpan
        /// </summary>
        public TimeSpan? UptimeTimeSpan { get { return Uptime.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(Uptime.Value) : null; } }

        /// <summary>
        /// Date and time at which the device started
        /// </summary>
        public DateTime? StartedAt { get { return Uptime.HasValue ? (DateTime?)DateTime.Now.AddSeconds(Uptime.Value * -1) : null; } }

        /// <summary>
        /// Device model
        /// </summary>
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }

        /// <summary>
        /// Device hostname
        /// </summary>
        [JsonProperty(PropertyName = "hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Device Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Device Adopt IP
        /// </summary>
        [JsonProperty(PropertyName = "adopt_ip")]
        public string AdoptIP { get; set; }

        /// <summary>
        /// Unique device ID
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        /// List of ports
        /// </summary>
        [JsonProperty(PropertyName = "port_table")]
        public List<Port> Port_table { get; set; }
    }
}
