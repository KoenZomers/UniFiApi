using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace KoenZomers.UniFi.Api
{
    /// <summary>
    /// Api class to communicate with the UniFi Controller
    /// </summary>
    public class Api
    {
        #region Fields

        /// <summary>
        /// Cookie container which holds all cookies for sessions towards the UniFi Controller
        /// </summary>
        private CookieContainer _cookieContainer;

        /// <summary>
        /// Username to use to authenticate
        /// </summary>
        private string _username;

        /// <summary>
        /// Password to use to authenticate
        /// </summary>
        private string _password;

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
        
        /// <summary>
        /// Boolean indicating whether UniFi Controller is Unifi OS
        /// </summary>
        public bool IsUniFiOS { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the UniFi API Controller class against the default UniFi site
        /// </summary>
        /// <param name="baseUri">BaseUri of the UniFi Controller, i.e. https://192.168.0.1:8443</param>
        public Api(Uri baseUri) : this(baseUri, null)
        {            
        }

        /// <summary>
        /// Instantiates a new instance of the UniFi API Controller class
        /// </summary>
        /// <param name="baseUri">BaseUri of the UniFi Controller, i.e. https://192.168.0.1:8443</param>
        /// <param name="siteId">Identifier of the site in UniFi. Set to NULL if you want to use the default site.</param>
        /// <param name="isUniFiOS">Specifies if target controller is UnifiOS based device or stand-alone software controller</param>
        public Api(Uri baseUri, string siteId, bool isUniFiOS = false)
        {
            BaseUri = baseUri;
            if (!string.IsNullOrWhiteSpace(siteId))
            {
                SiteId = siteId;
            }

            IsUniFiOS = isUniFiOS;
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
        /// Reauthenticate against the UniFi Controller with the credentials kept from the initial Authenticate call. Do not use this before Authenticate has been called on this instance.
        /// </summary>
        /// <returns>Boolean indicating whether the authentication was successful (True) or failed (False)</returns>
        public async Task<bool> Reauthenticate()
        {
            if(string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                throw new InvalidOperationException("No cached credentials yet. Call Authenticate first.");
            }

            return await Authenticate(_username, _password);
        }

        /// <summary>
        /// Authenticate against the UniFi Controller with the provided credentials
        /// </summary>
        /// <param name="username">Username to authenticate with</param>
        /// <param name="password">Password to authenticate with</param>
        /// <returns>Boolean indicating whether the authentication was successful (True) or failed (False)</returns>
        public async Task<bool> Authenticate(string username, string password)
        {
            _username = username;
            _password = password;

            // Create a new cookie container to contain the authentication cookie
            _cookieContainer = new CookieContainer();

            // Send an authentication request
            var authUri = new Uri(BaseUri, IsUniFiOS ? "/api/auth/login" : "/api/login");
            var resultString = await HttpUtility.AuthenticateViaJsonPostMethod(authUri, username, password, _cookieContainer, ConnectionTimeout);

            bool resultOk;
            // Verify if the request was successful
            if (IsUniFiOS)
            {
                var resultJson = JsonConvert.DeserializeObject<Responses.LoginResponse>(resultString);
                resultOk = !string.IsNullOrEmpty(resultJson.Unique_Id);
            }
            else
            {
                var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.BaseResponse>>(resultString);
                resultOk = resultJson.meta.ResultCode.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
            }
            IsAuthenticated = resultOk;
            return resultOk;
        }

        /// <summary>
        /// Makes a GET request towards the UniFi Controller while trying to ensure the session is authenticated
        /// </summary>
        /// <param name="uri">Full URL to the UniFi controller to use to retrieve information</param>
        /// <returns>String containing the result from the UniFi service</returns>
        public async Task<string> EnsureAuthenticatedGetRequest(Uri uri)
        {
            // Ensure this session is authenticated
            if (!IsAuthenticated)
            {
                // Ensure we're having cached credentials we can use to try to authenticate
                if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
                {
                    throw new InvalidOperationException("No active connection yet and unable to reauthenticate due to missing credentials. Call Authenticate first.");
                }

                // Try to reauthenticate using cached credentials
                if(!await Reauthenticate())
                {
                    throw new InvalidOperationException("No active connection yet and unable to reauthenticate using cached credentials. Call Authenticate first.");
                }
            }

            try
            {
                // Try to get the data from the UniFi Controller
                var resultString = await HttpUtility.GetRequestResult(uri, _cookieContainer, ConnectionTimeout);
                return resultString;
            }
            catch(WebException e) when (e.Message.Contains("401"))
            {
                // Authenticated failed. Check if this session has authenticated successfully before.
                if (IsAuthenticated)
                {
                    // Session may have expired, try to authenticate again using the cached credentials
                    if (!await Reauthenticate())
                    {
                        throw new InvalidOperationException("Unable to reauthenticate using cached credentials. Call Authenticate first.");
                    }

                    var resultString = await HttpUtility.GetRequestResult(uri, _cookieContainer, ConnectionTimeout);
                    return resultString;
                }

                // Credentials are likely invalid, throw the excepton
                throw;
            }
        }

        /// <summary>
        /// Makes a POST request towards the UniFi Controller while trying to ensure the session is authenticated
        /// </summary>
        /// <param name="uri">Full URL to the UniFi controller to POST data to</param>
        /// <param name="postData">The HTTP POST message body contents</param>
        /// <returns>String containing the result from the UniFi service</returns>
        public async Task<string> EnsureAuthenticatedPostRequest(Uri uri, string postData)
        {
            // Ensure this session is authenticated
            if (!IsAuthenticated)
            {
                // Ensure we're having cached credentials we can use to try to authenticate
                if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
                {
                    throw new InvalidOperationException("No active connection yet and unable to reauthenticate due to missing credentials. Call Authenticate first.");
                }

                // Try to reauthenticate using cached credentials
                if (!await Reauthenticate())
                {
                    throw new InvalidOperationException("No active connection yet and unable to reauthenticate using cached credentials. Call Authenticate first.");
                }
            }

            try
            {
                // Try to get the data from the UniFi Controller
                var resultString = await HttpUtility.PostRequest(uri, postData, _cookieContainer, ConnectionTimeout);
                return resultString;
            }
            catch (WebException e) when (e.Message.Contains("401"))
            {
                // Authenticated failed. Check if this session has authenticated successfully before.
                if (IsAuthenticated)
                {
                    // Session may have expired, try to authenticate again using the cached credentials
                    if (!await Reauthenticate())
                    {
                        throw new InvalidOperationException("Unable to reauthenticate using cached credentials. Call Authenticate first.");
                    }

                    var resultString = await HttpUtility.PostRequest(uri, postData, _cookieContainer, ConnectionTimeout);
                    return resultString;
                }

                // Credentials are likely invalid, throw the excepton
                throw;
            }
        }

        /// <summary>
        /// Gets the currently connected clients
        /// </summary>
        /// <returns>List with connected clients</returns>
        public async Task<List<Responses.Clients>> GetActiveClients()
        {
            var unifiUri = GetUri($"api/s/{SiteId}/stat/sta");
            var resultString = await EnsureAuthenticatedGetRequest(unifiUri);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson.data;
        }

        private Uri GetUri(string target, bool skipThing = false)
        {
            if (IsUniFiOS && skipThing == false)
            {
                return new Uri(BaseUri, "proxy/network/" + target);
            }
            else
            {
                return new Uri(BaseUri, target);
            }

        }

        /// <summary>
        /// Gets all clients known to UniFi. This contains both clients currently connected as well as clients that were connected in the past.
        /// </summary>
        /// <returns>List with all known clients</returns>
        public async Task<List<Responses.Clients>> GetAllClients()
        {
            var unifiUri = GetUri($"/api/s/{SiteId}/stat/alluser");
            var resultString = await EnsureAuthenticatedGetRequest(unifiUri);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets a list with all UniFi devices
        /// </summary>
        /// <returns>List with all UniFi devices</returns>
        public async Task<List<Responses.Device>> GetDevices()
        {
            var unifiUri = GetUri($"api/s/{SiteId}/stat/device");
            var resultString = await EnsureAuthenticatedGetRequest(unifiUri);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Device>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets all sites registered with UniFi
        /// </summary>
        /// <returns>List with all sites</returns>
        public async Task<List<Responses.Site>> GetSites()
        {
            var unifiUri = GetUri($"/api/self/sites");
            var resultString = await EnsureAuthenticatedGetRequest(unifiUri);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Site>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets the connection history of the client with the provided MAC Address
        /// </summary>
        /// <param name="limit">Amount of historic items to retrieve. Most recent connection will be first. Default is last 5 connections to be returned.</param>
        /// <param name="macAddress">MAC Address of the client to retrieve the history for</param>
        /// <returns>List with all connection history for the client</returns>
        public async Task<List<Responses.ClientSession>> GetClientHistory(string macAddress, int limit = 5)
        {
            // Make the POST request towards the UniFi API to request blocking the client with the provided MAC address
            var resultString = await EnsureAuthenticatedPostRequest(GetUri($"/api/s/{SiteId}/stat/session"),
                                                                    "{\"mac\":\"" + macAddress + "\",\"_limit\":" + limit + ",\"_sort\":\"-assoc_time\"}");
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.ClientSession>>(resultString);

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
            var resultString = await EnsureAuthenticatedPostRequest(GetUri($"/api/s/{SiteId}/cmd/stamgr"),
                                                                    "{\"mac\":\"" + macAddress + "\",\"cmd\":\"block-sta\"}");
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson;
        }

        /// <summary>
        /// Authorizes a guest to access the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to provide access to the network</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> AuthorizeGuest(string macAddress)
        {
            // Make the POST request towards the UniFi API to request authorizing the client with the provided MAC address
            var resultString = await EnsureAuthenticatedPostRequest(GetUri($"/api/s/{SiteId}/cmd/stamgr"),
                                                                    "{\"mac\":\"" + macAddress + "\",\"cmd\":\"authorize-guest\"}");
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson;
        }

        /// <summary>
        /// Unauthorizes a guest to access the network
        /// </summary>
        /// <param name="macAddress">The MAC address of the client to revoke its access from the network</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> UnauthorizeGuest(string macAddress)
        {
            // Make the POST request towards the UniFi API to request unauthorizing the client with the provided MAC address
            var resultString = await EnsureAuthenticatedPostRequest(new Uri(BaseUri, $"/api/s/{SiteId}/cmd/stamgr"),
                                                                    "{\"mac\":\"" + macAddress + "\",\"cmd\":\"unauthorize-guest\"}");
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
            var resultString = await EnsureAuthenticatedPostRequest(GetUri($"/api/s/{SiteId}/cmd/stamgr"),
                                                                    "{\"mac\":\"" + macAddress + "\",\"cmd\":\"unblock-sta\"}");
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);

            return resultJson;
        }

        /// <summary>
        /// Rename Client
        /// </summary>
        /// <param name="client">Client to rename</param>
        /// <param name="name">New name</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> RenameClient(Responses.Clients client, string name)
        {
            return await RenameClient(client.Id, name);
        }

        /// <summary>
        /// Rename Client
        /// </summary>
        /// <param name="userId">Client's User Id for client to be renamed</param>
        /// <param name="name">New name</param>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> RenameClient(string userId, string name)
        {
            // Make the POST request towards the UniFi API to rename a client
            var resultString = await EnsureAuthenticatedPostRequest(GetUri($"/api/s/{SiteId}/upd/user/{userId}"),
                                                                    JsonConvert.SerializeObject(new { name }));
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
            var logoutUri = new Uri(BaseUri, IsUniFiOS ? "/api/auth/logout" : "/api/logout");
            var resultString = await HttpUtility.LogoutViaJsonPostMethod(logoutUri, _cookieContainer, ConnectionTimeout, IsUniFiOS);

            bool resultOk;
            if (IsUniFiOS)
            {
                var resultJson = JsonConvert.DeserializeObject<Responses.LogoutResponse>(resultString);
                resultOk = resultJson.Success;
            }
            else
            {
                var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.BaseResponse>>(resultString);
                // Verify if the request was successful
                resultOk = resultJson.meta.ResultCode.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
            }

            IsAuthenticated = !resultOk;
            return resultOk;
        }

        /// <summary>
        /// Gets the currently defined networks
        /// </summary>
        /// <returns>List with defined networks</returns>
        public async Task<List<Responses.Network>> GetNetworks() {
            var unifiUri = GetUri($"/api/s/{SiteId}/rest/networkconf");
            var resultString = await EnsureAuthenticatedGetRequest(unifiUri);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Network>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Gets the currently defined wireless networks
        /// </summary>
        /// <returns>List with defined wireless networks</returns>
        public async Task<List<Responses.WirelessNetwork>> GetWirelessNetworks() {
            var unifiUri = GetUri($"/api/s/{SiteId}/rest/wlanconf");
            var resultString = await EnsureAuthenticatedGetRequest(unifiUri);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.WirelessNetwork>>(resultString);

            return resultJson.data;
        }

        /// <summary>
        /// Removes/forgets the provided clients
        /// </summary>
        /// <param name="macArray">String array with mac addresses of clients to forget</param>
        /// <returns>List with removed clients</returns>
        public async Task<Responses.ResponseEnvelope<Responses.Clients>> RemoveClients(string[] macArray)
        {
            string payload = JsonConvert.SerializeObject(new
            {
                cmd = "forget-sta",
                macs = macArray
            });
            var resultString = await EnsureAuthenticatedPostRequest(GetUri($"/api/s/{SiteId}/cmd/stamgr"), payload);

            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.Clients>>(resultString);
            return resultJson;
        }

        /// <summary>
        /// Reconnects the provided client
        /// </summary>
        /// <param name="client">The client to force to reconnect</param>
        /// <returns>True if the reconnect was successful or False if it failed</returns>
        public async Task<bool> ReconnectClient(Responses.Clients client)
        {
            return await ReconnectClient(client.MacAddress);
        }

        /// <summary>
        /// Reconnects the provided client
        /// </summary>
        /// <param name="macAddress">The MAC address of client to reconnect</param>
        /// <returns>True if the reconnect was successful or False if it failed</returns>
        public async Task<bool> ReconnectClient(string macAddress)
        {
            string payload = JsonConvert.SerializeObject(new
            {
                cmd = "kick-sta",
                mac = macAddress
            });

            var resultString = await EnsureAuthenticatedPostRequest(GetUri($"/api/s/{SiteId}/cmd/stamgr"), payload);
            var resultJson = JsonConvert.DeserializeObject<Responses.ResponseEnvelope<Responses.BaseResponse>>(resultString);

            return resultJson.meta.ResultCode.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion
    }
}
