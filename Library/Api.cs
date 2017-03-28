using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace KoenZomers.UniFi.Api
{
    /// <summary>
    /// Api class to communicate with the UniFi Controller
    /// </summary>
    public class Api : IDisposable
    {
        #region Fields

        /// <summary>
        /// Cookie container which holds all cookies for sessions towards the UniFi Controller
        /// </summary>
        private CookieContainer _cookieContainer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the BaseUri of the UniFi Controller. Needs to be set through the constructor.
        /// </summary>
        public Uri BaseUri { get; private set; }

        /// <summary>
        /// Timeout in milliseconds to apply to wait at maximum for the UniFi Controller to respond to a request
        /// </summary>
        public int ConnectionTimeout { get; set; } = 60000;

        /// <summary>
        /// Boolean indicating whether this Api session is authenticated
        /// </summary>
        public bool IsAuthenticated { get; private set; } = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the UniFi API Controller class
        /// </summary>
        /// <param name="baseUri">BaseUri of the UniFi Controller, i.e. https://192.168.0.1:8443</param>
        public Api(Uri baseUri)
        {            
            BaseUri = baseUri;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disables SSL certificate validation in case of using a self signed SSL certificate
        /// </summary>
        public void DisableSslValidation()
        {
            HttpUtility.DisableSslValidation();
        }

        /// <summary>
        /// Authenticate against the UniFi Controller with the provided credentials
        /// </summary>
        /// <param name="username">Username to authenticate with</param>
        /// <param name="password">Password to authenticate with</param>
        /// <returns>Boolean indicating whether the authentication was successful (True) or failed (False)</returns>
        public async Task<bool> Authenticate(string username, string password)
        {
            // Create a new cookie container to contain the authentication cookie
            _cookieContainer = new CookieContainer();

            // Create a session towards the UniFi Controller
            var loginUri = new Uri(BaseUri, "/manage/account/login");
            await HttpUtility.HttpCreateSession(loginUri, _cookieContainer, ConnectionTimeout);

            // Send an authentication request
            var authUri = new Uri(BaseUri, "/api/login");
            var resultString = await HttpUtility.AuthenticateViaJsonPostMethod(authUri, username, password, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.BaseResponse>>(resultString);

            // Verify if the request was successful
            var resultOk = resultJson.meta.ResultCode.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
            IsAuthenticated = resultOk;
            return resultOk;
        }

        /// <summary>
        /// Gets the currently connected clients
        /// </summary>
        /// <returns>List with connected clients</returns>
        public async Task<List<Responses.Clients>> GetActiveClients()
        {
            // Request all connected clients
            var clientsUri = new Uri(BaseUri, "/api/s/default/stat/sta");
            var resultString = await HttpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets the access points
        /// </summary>
        /// <returns>List with access points</returns>
        public async Task<List<Responses.AccessPoint>> GetAccessPoints()
        {
            // Request all connected clients
            var clientsUri = new Uri(BaseUri, "/api/s/default/stat/device");
            var resultString = await HttpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.AccessPoint>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Logs out from the UniFi Controller
        /// </summary>
        /// <returns>True if logout was successful or False if it failed</returns>
        public async Task<bool> Logout()
        {
            // Create a session towards the UniFi Controller
            var logoutUri = new Uri(BaseUri, "/api/logout");
            var resultString = await HttpUtility.LogoutViaJsonPostMethod(logoutUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.BaseResponse>>(resultString);

            // Verify if the request was successful
            var resultOk = resultJson.meta.ResultCode.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
            IsAuthenticated = !resultOk;
            return resultOk;
        }

        /// <summary>
        /// Closes this session
        /// </summary>
        public void Dispose()
        {
            // Check if the session is authenticated
            if (!IsAuthenticated) return;

            // Log out from the session to free up server resources
            Logout().Wait();

            _cookieContainer = null;
        }

        #endregion
    }
}
