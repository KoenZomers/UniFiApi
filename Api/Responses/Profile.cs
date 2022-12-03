using Newtonsoft.Json;

namespace KoenZomers.UniFi.Api.Responses
{
    public class Profile : BaseResponse
    {
        [JsonProperty(PropertyName = "_id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}