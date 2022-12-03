using Newtonsoft.Json;

namespace KoenZomers.UniFi.Api.Responses
{
    public class Port_Overrides
    {
        [JsonProperty(PropertyName = "port_idx")]
        public int Port_idx { get; set; }

        [JsonProperty(PropertyName = "portconf_id")]
        public string Portconf_id { get; set; }

        [JsonProperty(PropertyName = "op_mode")]
        public string Op_mode { get; set; }

        [JsonProperty(PropertyName = "aggregate_num_ports")]
        public int aggregate_num_ports { get; set; }

        //[JsonProperty(PropertyName = "port_securtity_mac_adderess")]
        //public List<MAC> Port_securtity_mac_adderess { get; set; }
    }
}