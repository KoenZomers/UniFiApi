using System;
using System.Collections.Generic;
using System.Text;

namespace KoenZomers.UniFi.Api.Responses
{
    /// <summary>
    /// Basic stub of login response to use to see if deserialization was successful or not
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Unique id of logged in user
        /// </summary>
        public string Unique_Id { get; set; }
    }
}
