using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KoenZomers.UniFi.Api.Responses;

namespace KoenZomers.UniFi.Api
{
    /// <summary>
    /// Api class to communicate with the UniFi Controller
    /// </summary>
    public class Api : IApi
    {
        #region Fields

        private readonly IHttpUtility httpUtility;

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
        public Api(Uri baseUri) : this(baseUri, null, null)
        {
        }

        /// <summary>
        /// Instantiates a new instance of the UniFi API Controller class
        /// </summary>
        /// <param name="baseUri">BaseUri of the UniFi Controller, i.e. https://192.168.0.1:8443</param>
        /// <param name="siteId">Identifier of the site in UniFi. Set to NULL if you want to use the default site.</param>
        public Api(Uri baseUri, string siteId, IHttpUtility httpUtility)
        {
            BaseUri = baseUri;

            if (!string.IsNullOrWhiteSpace(siteId))
            {
                SiteId = siteId;
            }

            this.httpUtility = httpUtility ?? new HttpUtility();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disables SSL certificate validation in case of using a self signed SSL certificate
        /// </summary>
        public void DisableSslValidation()
        {
            httpUtility.DisableSslValidation();
        }

        /// <summary>
        /// Enables connecting to a remote server hosting UniFi using a TLS 1.1 or TLS 1.2 certificate
        /// </summary>
        public void EnableTls11and12()
        {
            httpUtility.EnableTls11and12();
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

            // Send an authentication request
            var authUri = new Uri(BaseUri, "/api/login");
            var resultString = await httpUtility.AuthenticateViaJsonPostMethod(authUri, username, password, _cookieContainer, ConnectionTimeout);
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
        public async Task<List<Clients>> GetActiveClients(string unifiSiteId)
        {
            var clientsUri = new Uri(BaseUri, $"/api/s/{unifiSiteId}/stat/sta");
            var resultString = await httpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Clients>>(resultString);

            return resultJson.data;
        }

        public async Task<List<Clients>> GetActiveClients()
            => await GetActiveClients(SiteId);

        /// <summary>
        /// Gets all clients known to UniFi. This contains both clients currently connected as well as clients that were connected in the past.
        /// </summary>
        /// <returns>List with all known clients</returns>
        public async Task<List<Clients>> GetAllClients()
        {
            var clientsUri = new Uri(BaseUri, $"/api/s/{SiteId}/stat/alluser");
            var resultString = await httpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Clients>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets a list with all UniFi devices
        /// </summary>
        /// <returns>List with all UniFi devices</returns>
        public async Task<List<Device>> GetDevices()
        {
            var clientsUri = new Uri(BaseUri, $"/api/s/{SiteId}/stat/device");
            var resultString = await httpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Device>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets all sites registered with UniFi
        /// </summary>
        /// <returns>List with all sites</returns>
        public async Task<List<Site>> GetSites()
        {
            var clientsUri = new Uri(BaseUri, $"/api/self/sites");
            var resultString = await httpUtility.GetRequestResult(clientsUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Site>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets the connection history of the client with the provided MAC Address
        /// </summary>
        /// <param name="limit">Amount of historic items to retrieve. Most recent connection will be first. Default is last 5 connections to be returned.</param>
        /// <param name="macAddress">MAC Address of the client to retrieve the history for</param>
        /// <returns>List with all connection history for the client</returns>
        public async Task<List<ClientSession>> GetClientHistory(string macAddress, int limit = 5)
        {
            // Make the POST request towards the UniFi API to request blocking the client with the provided MAC address
            var resultString = await httpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{SiteId}/stat/session"),
                                                             "{\"mac\":\"" + macAddress + "\",\"_limit\":" + limit + ",\"_sort\":\"-assoc_time\"}",
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<ClientSession>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Blocks a client from accessing the network
        /// </summary>
        /// <param name="client">Client to block from getting access to the network</param>
        public async Task<ResponseEnvelope<Clients>> BlockClient(Clients client)
        {
            return await BlockClient(client.MacAddress);
        }

        /// <summary>
        /// Blocks a client from accessing the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to block from getting access to the network</param>
        public async Task<ResponseEnvelope<Clients>> BlockClient(string macAddress)
        {
            // Make the POST request towards the UniFi API to request blocking the client with the provided MAC address
            var resultString = await httpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{SiteId}/cmd/stamgr"),
                                                             "{\"mac\":\"" + macAddress + "\",\"cmd\":\"block-sta\"}",
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Clients>>(resultString);

            return resultJson;
        }

        /// <summary>
        /// Authorizes a guest to access the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to provide access to the network</param>
        public async Task<ResponseEnvelope<Clients>> AuthorizeGuest(string macAddress, string unifiSiteId)
        {
            // Make the POST request towards the UniFi API to request authorizing the client with the provided MAC address
            var resultString = await httpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{unifiSiteId}/cmd/stamgr"),
                                                             "{\"mac\":\"" + macAddress + "\",\"cmd\":\"authorize-guest\"}",
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Clients>>(resultString);

            return resultJson;
        }

        public async Task<ResponseEnvelope<Clients>> AuthorizeGuest(string macAddress)
            => await AuthorizeGuest(macAddress, SiteId);

        /// <summary>
        /// Unauthorizes a guest to access the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to revoke its access from the network</param>
        public async Task<ResponseEnvelope<Clients>> UnauthorizeGuest(string macAddress, string unifiSiteId)
        {
            // Make the POST request towards the UniFi API to request unauthorizing the client with the provided MAC address
            var resultString = await httpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{unifiSiteId}/cmd/stamgr"),
                                                             "{\"mac\":\"" + macAddress + "\",\"cmd\":\"unauthorize-guest\"}",
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Clients>>(resultString);

            return resultJson;
        }

        public async Task<ResponseEnvelope<Clients>> UnauthorizeGuest(string macAddress)
            => await UnauthorizeGuest(macAddress, SiteId);

        /// <summary>
        /// Unblocks a client from accessing the network
        /// </summary>
        /// <param name="client">Client to unblock from getting access to the network</param>
        public async Task<ResponseEnvelope<Clients>> UnblockClient(Clients client)
        {
            return await UnblockClient(client.MacAddress);
        }

        /// <summary>
        /// Unblocks a client from accessing the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to unblock from getting access to the network</param>
        public async Task<ResponseEnvelope<Clients>> UnblockClient(string macAddress)
        {
            // Make the POST request towards the UniFi API to request unblocking the client with the provided MAC address
            var resultString = await httpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{SiteId}/cmd/stamgr"),
                                                             "{\"mac\":\"" + macAddress + "\",\"cmd\":\"unblock-sta\"}",
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Clients>>(resultString);

            return resultJson;
        }

        /// <summary>
        /// Rename Client
        /// </summary>
        /// <param name="client">Client to rename</param>
        /// <param name="name">New name</param>
        public async Task<ResponseEnvelope<Clients>> RenameClient(Clients client, string name)
        {
            return await RenameClient(client.Id, name);
        }

        /// <summary>
        /// Rename Client
        /// </summary>
        /// <param name="userId">Client's User Id for client to be renamed</param>
        /// <param name="name">New name</param>
        public async Task<ResponseEnvelope<Clients>> RenameClient(string userId, string name)
        {
            // Make the POST request towards the UniFi API to rename a client
            var resultString = await httpUtility.PostRequest(new Uri(BaseUri, $"/api/s/{SiteId}/upd/user/{userId}"),
                                                             JsonConvert.SerializeObject(new { name }),
                                                             _cookieContainer,
                                                             ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<Clients>>(resultString);

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
            var resultString = await httpUtility.LogoutViaJsonPostMethod(logoutUri, _cookieContainer, ConnectionTimeout);
            var resultJson = JsonConvert.DeserializeObject<ResponseEnvelope<BaseResponse>>(resultString);

            // Verify if the request was successful
            var resultOk = resultJson.meta.ResultCode.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
            IsAuthenticated = !resultOk;
            return resultOk;
        }

        #endregion
    }
}
