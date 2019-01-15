using System.Collections.Generic;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Base envelope package when receiving data from the UniFi Controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseEnvelope<T> where T : BaseResponse
    {
        /// <summary>
        /// List of type T with the data resulting from a request towards the UniFi Controller
        /// </summary>
        public List<T> data { get; set; }
        
        /// <summary>
        /// Metadata regarding the request towards the UniFi Controller
        /// </summary>
        public Meta meta { get; set; }
    }
}
