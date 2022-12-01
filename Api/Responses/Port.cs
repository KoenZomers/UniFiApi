using Newtonsoft.Json;
using System.Collections.Generic;

namespace KoenZomers.UniFi.Api.Responses
{
    public class Port
    {
        [JsonProperty(PropertyName = "port_idx")]
        public int Port_idx { get; set; }

        [JsonProperty(PropertyName = "mac_table")]
        public List<MacEntry> Mac_table { get; set; }
    }
}