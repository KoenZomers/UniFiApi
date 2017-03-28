using Newtonsoft.Json;

namespace KoenZomers.UniFi.Api.Library.Responses
{
    /// <summary>
    /// Access point details
    /// </summary>
    public class AccessPoint : BaseResponse
    {
        /// <summary>
        /// Name of the access point
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// MAC address of the access point
        /// </summary>
        [JsonProperty(PropertyName = "mac")]
        public string MacAddress { get; set; }

        // TODO: Loads of more information returned by the UniFi Controller, need to add them to this class still
    }
}
