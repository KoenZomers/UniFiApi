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
        /// Gets the site identifier of the UniFi Controller. Needs to be set through the constructor.
        /// </summary>
        public string SiteId { get; private set; } = "default";

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
        /// Instantiates a new instance of the UniFi API Controller class against the default UniFi site
        /// </summary>
        /// <param name="baseUri">BaseUri of the UniFi Controller, i.e. https://192.168.0.1:8443</param>
        public Api(Uri baseUri)
        {            
            BaseUri = baseUri;
        }

        /// <summary>
        /// Instantiates a new instance of the UniFi API Controller class
        /// </summary>
        /// <param name="baseUri">BaseUri of the UniFi Controller, i.e. https://192.168.0.1:8443</param>
        /// <param name="siteId">Identifier of the site in UniFi</param>
        public Api(Uri baseUri, string siteId)
        {
            BaseUri = baseUri;
            SiteId = siteId;
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
        /// Enables connecting to a remote server hosting UniFi using a TLS 1.1 or TLS 1.2 certificate
        /// </summary>
        public void EnableTls11and12()
        {
            HttpUtility.EnableTls11and12();
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
            var clientsUri = new Uri(BaseUri, $"/api/s/{SiteId}/stat/sta");
            var resultString = await HttpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets all clients known to UniFi. This contains both clients currently connected as well as clients that were connected in the past.
        /// </summary>
        /// <returns>List with all known clients</returns>
        public async Task<List<Responses.Clients>> GetAllClients()
        {
            // Request all connected clients
            var clientsUri = new Uri(BaseUri, $"/api/s/{SiteId}/stat/alluser");
            var resultString = await HttpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets a list with all UniFi devices
        /// </summary>
        /// <returns>List with all UniFi devices</returns>
        [Obsolete("In contratry what this method names leads to believe, this not only retrieves the UniFi Access Points but all UniFi devices. Therefore a new method GetDevices has been introduced which properly describes its function. Please change your code to use the GetDevices method instead.")]
        public async Task<List<Responses.AccessPoint>> GetAccessPoints()
        {
            return await GetDevices();
        }

        /// <summary>
        /// Gets a list with all UniFi devices
        /// </summary>
        /// <returns>List with all UniFi devices</returns>
        public async Task<List<Responses.AccessPoint>> GetDevices()
        {
            // Request all connected clients
            var clientsUri = new Uri(BaseUri, $"/api/s/{SiteId}/stat/device");
            var resultString = await HttpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.AccessPoint>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Blocks a client from accessing the network
        /// </summary>
        /// <param name="client">Client to block from getting access to the network</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> BlockClient(Responses.Clients client)
        {
            return await BlockClient(client.MacAddress);
        }

        /// <summary>
        /// Blocks a client from accessing the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to block from getting access to the network</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> BlockClient(string macAddress)
        {
            // Make the POST request towards the UniFi API to request blocking the client with the provided MAC address
            var resultString = await HttpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{SiteId}/cmd/stamgr"),
                                                             "{\"mac\":\"" + macAddress + "\",\"cmd\":\"block-sta\"}",
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson;
        }

        /// <summary>
        /// Unblocks a client from accessing the network
        /// </summary>
        /// <param name="client">Client to unblock from getting access to the network</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> UnblockClient(Responses.Clients client)
        {
            return await UnblockClient(client.MacAddress);
        }

        /// <summary>
        /// Unblocks a client from accessing the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to unblock from getting access to the network</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> UnblockClient(string macAddress)
        {
            // Make the POST request towards the UniFi API to request unblocking the client with the provided MAC address
            var resultString = await HttpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{SiteId}/cmd/stamgr"),
                                                             "{\"mac\":\"" + macAddress + "\",\"cmd\":\"unblock-sta\"}",
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson;
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
