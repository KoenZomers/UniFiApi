using Newtonsoft.Json;

namespace KoenZomers.UniFi.Api.Responses
{
    public class MacEntry
    {
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "ip")]
        public string IP { get; set; }

        [JsonProperty(PropertyName = "mac")]
        public string Mac { get; set; }

        [JsonProperty(PropertyName = "static")]
        public bool Static { get; set; }

        [JsonProperty(PropertyName = "uptime")]
        public double Uptime { get; set; }

        [JsonProperty(PropertyName = "vlan")]
        public int Vlan { get; set; }

        [JsonProperty(PropertyName = "is_only_station_on_port")]
        public bool Is_only_station_on_port { get; set; }
    }
}