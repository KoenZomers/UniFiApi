using System.Collections.Generic;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Response status of logout against UniFi OS based Controller
    /// </summary>
    public class LogoutResponse
    {
        /// <summary>
        /// Notes if logout was successful or not
        /// </summary>
        public bool Success{ get; set; }
    }
}
