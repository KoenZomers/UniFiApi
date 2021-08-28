using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace KoenZomers.UniFi.Api
{
    /// <summary>
    /// Internal utility class for Http communication with the UniFi Controller
    /// </summary>
    internal static class HttpUtility
    {
        /// <summary>
        /// Disables SSL Validation in case of self signed SSL certificates being used
        /// </summary>
        public static void DisableSslValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        /// <summary>
        /// Enables connecting to a remote server hosting UniFi using a TLS 1.1 or TLS 1.2 certificate
        /// </summary>
        public static void EnableTls11and12()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Performs a GET request to the provided url to download the page contents
        /// </summary>
        /// <param name="url">Url of the page to retrieve</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        /// <returns>Contents of the page</returns>
        public async static Task<string> GetRequestResult(Uri url, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the request
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;
            request.KeepAlive = false;

            // Send the request to the webserver
            using (var response = await request.GetResponseAsync())            
            {
                // Get the stream containing content returned by the server.
                using (var dataStream = response.GetResponseStream())
                {
                    if (dataStream == null) return null;

                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new StreamReader(dataStream))
                    {
                        // Read the content returned
                        var responseFromServer = await reader.ReadToEndAsync();
                        return responseFromServer;
                    }
                }
            }
        }

        /// <summary>
        /// Sends a POST request towards UniFi
        /// </summary>
        /// <param name="url">Url to POST the postData to</param>
        /// <param name="postData">Data to send to the UniFi service, typically a JSON payload</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        /// <returns>The website contents returned by the webserver after posting the data</returns>
        public async static Task<string> PostRequest(Uri url, string postData, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the POST request
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json, text/plain, */*";
            request.ContentType = "application/json;charset=UTF-8";
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;
            request.KeepAlive = false;

            // Check if the have a Cross Site Request Forgery cookie and if so, add it as the X-Csrf-Token header which is required by UniFi when making a POST
            var csrfCookie = cookieContainer.GetAllCookies().FirstOrDefault(c => c.Name == "csrf_token");
            if(csrfCookie != null)
            {
                request.Headers.Add("X-Csrf-Token", csrfCookie.Value);
            }

            // Convert the POST data to a byte array
            var postDataByteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postDataByteArray.Length;

            // Get the request stream
            using (var postDataStream = await request.GetRequestStreamAsync())
            {
                // Write the POST data to the request stream
                await postDataStream.WriteAsync(postDataByteArray, 0, postDataByteArray.Length);

                // Close the Stream object
                postDataStream.Close();
            }

            // Receive the response from the webserver
            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                // Make sure the webserver has sent a response
                if (response == null) return null;

                using (var requestDataStream = response.GetResponseStream())
                {
                    // Make sure the datastream with the response is available
                    if (requestDataStream == null) return null;

                    using (var reader = new StreamReader(requestDataStream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Sends a POST request with JSON variables to authenticate against UniFi
        /// </summary>
        /// <param name="url">Url to POST the login information to</param>
        /// <param name="username">Username to authenticate with</param>
        /// <param name="password">Password to authenticate with</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        /// <returns>The website contents returned by the webserver after posting the data</returns>
        public async static Task<string> AuthenticateViaJsonPostMethod(Uri url, string username, string password, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the POST request which performs the login
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json, text/plain, */*";
            request.ContentType = "application/json;charset=UTF-8";
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;
            request.KeepAlive = false;

            // Construct POST data
            var postData = string.Concat(@"{""username"":""", username, @""",""password"":""", password.Replace("\"", "\\\""), @""",""remember"":false,""strict"":true}");

            // Convert the POST data to a byte array
            var postDataByteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postDataByteArray.Length;

            // Get the request stream
            using (var postDataStream = await request.GetRequestStreamAsync())
            {
                // Write the POST data to the request stream
                await postDataStream.WriteAsync(postDataByteArray, 0, postDataByteArray.Length);

                // Close the Stream object
                postDataStream.Close();
            }

            try
            {
                // Request must be kept alive because in case of an error (i.e. wrong credentials) the response otherwise can't be read anymore
                request.KeepAlive = true;

                // Receive the response from the webserver
                using (var response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    // Make sure the webserver has sent a response
                    if (response == null) return null;

                    using (var requestDataStream = response.GetResponseStream())
                    {
                        // Make sure the datastream with the response is available
                        if (requestDataStream == null) return null;

                        using (var reader = new StreamReader(requestDataStream))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                // A protocolerror typically indicates that the credentials were wrong so we can handle that. Other types could be anything so we rethrow it to the caller to deal with.
                if (e.Status != WebExceptionStatus.ProtocolError)
                {
                    throw;
                }

                // Parse the response from the server
                using (var response = (HttpWebResponse)e.Response)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding("utf-8")))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends a POST request to log out from the UniFi Controller
        /// </summary>
        /// <param name="url">Url to POST the logout request to</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        /// <returns>The website contents returned by the webserver after posting the data</returns>
        public async static Task<string> LogoutViaJsonPostMethod(Uri url, CookieContainer cookieContainer, int timeout = 60000, bool extractToken = false)
        {
            // Construct the POST request which performs the login
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json, text/plain, */*";
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;
            request.KeepAlive = false;

            if (extractToken)
            {
                // Check if the have a Cross Site Request Forgery cookie and if so, add it as the X-Csrf-Token header which is required by UniFi when logging out from UniFi OS devices
                var cookies = cookieContainer.GetCookies(url);
                if (cookies.Count > 0)
                {
                    var decodedToken = new JwtSecurityTokenHandler().ReadToken(cookies["Token"].Value) as JwtSecurityToken;
                    var tokenValue = decodedToken.Claims.FirstOrDefault(c => c.Type == "csrfToken").Value;
                    request.Headers.Add("x-csrf-token", tokenValue);
                }
            }

            // Construct POST data
            var postData = "{}";

            // Convert the POST data to a byte array
            var postDataByteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest
            request.ContentType = "application/json;charset=UTF-8";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postDataByteArray.Length;

            // Get the request stream
            using (var postDataStream = await request.GetRequestStreamAsync())
            {
                // Write the POST data to the request stream
                await postDataStream.WriteAsync(postDataByteArray, 0, postDataByteArray.Length);

                // Close the Stream object
                postDataStream.Close();
            }

            // Receive the response from the webserver
            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                // Make sure the webserver has sent a response
                if (response == null) return null;

                using (var requestDataStream = response.GetResponseStream())
                {
                    // Make sure the datastream with the response is available
                    if (requestDataStream == null) return null;

                    using (var reader = new StreamReader(requestDataStream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
        }
    }
}
