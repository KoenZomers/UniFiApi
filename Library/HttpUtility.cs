using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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
        /// Performs a HEAD request to the provided url to have the remote webserver hand out a new sessionId
        /// </summary>
        /// <param name="url">Url to query</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        public static async Task HttpCreateSession(Uri url, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the request
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;

            // Send the request to the webserver
            await request.GetResponseAsync();
        }

        /// <summary>
        /// Performs a GET request to the provided url to download the page contents
        /// </summary>
        /// <param name="url">Url of the page to retrieve</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        /// <returns>Contents of the page</returns>
        public static async Task<string> GetRequestResult(Uri url, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the request
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;

            // Send the request to the webserver
            var response = await request.GetResponseAsync();

            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();
            if (dataStream == null) return null;
            
            // Open the stream using a StreamReader for easy access.
            var reader = new StreamReader(dataStream);
            
            // Read the content returned
            var responseFromServer = await reader.ReadToEndAsync();
            return responseFromServer;
        }

        /// <summary>
        /// Sends a POST request towards UniFi
        /// </summary>
        /// <param name="url">Url to POST the postData to</param>
        /// <param name="postData">Data to send to the UniFi service, typically a JSON payload</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        /// <returns>The website contents returned by the webserver after posting the data</returns>
        public static async Task<string> PostRequest(Uri url, string postData, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the POST request
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json, text/plain, */*";
            request.ContentType = "application/json;charset=UTF-8";
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;

            // Check if the have a Cross Site Request Forgery cookie and if so, add it as the X-Csrf-Token header which is required by UniFi when making a POST
            var csrfCookie = cookieContainer.GetAllCookies().FirstOrDefault(c => c.Name == "csrf_token");
            if(csrfCookie != null)
            {
                request.Headers.Add("X-Csrf-Token", csrfCookie.Value);
            }

            // Convert the POST data to a byte array
            var postDataByteArray = Encoding.UTF8.GetBytes(postData.ToString());

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postDataByteArray.Length;

            // Get the request stream
            var dataStream = await request.GetRequestStreamAsync();

            // Write the POST data to the request stream
            await dataStream.WriteAsync(postDataByteArray, 0, postDataByteArray.Length);

            // Close the Stream object
            dataStream.Close();

            // Receive the response from the webserver
            var response = await request.GetResponseAsync() as HttpWebResponse;

            // Make sure the webserver has sent a response
            if (response == null) return null;

            dataStream = response.GetResponseStream();

            // Make sure the datastream with the response is available
            if (dataStream == null) return null;

            var reader = new StreamReader(dataStream);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Extracts all the cookies from a cookie container so the contents can be read and used
        /// </summary>
        /// <remarks>Code sample retrieved from https://stackoverflow.com/a/31900670/1271303 </remarks>
        /// <returns>IEnumerable containing all cookies available in the CookieContainer</returns>
        private static IEnumerable<Cookie> GetAllCookies(this CookieContainer c)
        {
            Hashtable k = (Hashtable)c.GetType().GetField("m_domainTable", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
            foreach (DictionaryEntry element in k)
            {
                SortedList l = (SortedList)element.Value.GetType().GetField("m_list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(element.Value);
                foreach (var e in l)
                {
                    var cl = (CookieCollection)((DictionaryEntry)e).Value;
                    foreach (Cookie fc in cl)
                    {
                        yield return fc;
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
        public static async Task<string> AuthenticateViaJsonPostMethod(Uri url, string username, string password, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the POST request which performs the login
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json, text/plain, */*";
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;

            // Construct POST data
            var postData = string.Concat(@"{""username"":""", username, @""",""password"":""", password, @""",""remember"":false,""strict"":true}");

            // Convert the POST data to a byte array
            var postDataByteArray = Encoding.UTF8.GetBytes(postData.ToString());

            // Set the ContentType property of the WebRequest
            request.ContentType = "application/json;charset=UTF-8";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postDataByteArray.Length;

            // Get the request stream
            var dataStream = await request.GetRequestStreamAsync();

            // Write the POST data to the request stream
            await dataStream.WriteAsync(postDataByteArray, 0, postDataByteArray.Length);

            // Close the Stream object
            dataStream.Close();

            // Receive the response from the webserver
            var response = await request.GetResponseAsync() as HttpWebResponse;

            // Make sure the webserver has sent a response
            if (response == null) return null;

            dataStream = response.GetResponseStream();

            // Make sure the datastream with the response is available
            if (dataStream == null) return null;

            var reader = new StreamReader(dataStream);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Sends a POST request to log out from the UniFi Controller
        /// </summary>
        /// <param name="url">Url to POST the logout request to</param>
        /// <param name="cookieContainer">Cookies which have been recorded for this session</param>
        /// <param name="timeout">Timeout in milliseconds on how long the request may take. Default = 60000 = 60 seconds.</param>
        /// <returns>The website contents returned by the webserver after posting the data</returns>
        public static async Task<string> LogoutViaJsonPostMethod(Uri url, CookieContainer cookieContainer, int timeout = 60000)
        {
            // Construct the POST request which performs the login
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json, text/plain, */*";
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = cookieContainer;
            request.Timeout = timeout;

            // Construct POST data
            var postData = "{}";

            // Convert the POST data to a byte array
            var postDataByteArray = Encoding.UTF8.GetBytes(postData.ToString());

            // Set the ContentType property of the WebRequest
            request.ContentType = "application/json;charset=UTF-8";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postDataByteArray.Length;

            // Get the request stream
            var dataStream = await request.GetRequestStreamAsync();

            // Write the POST data to the request stream
            await dataStream.WriteAsync(postDataByteArray, 0, postDataByteArray.Length);

            // Close the Stream object
            dataStream.Close();

            // Receive the response from the webserver
            var response = await request.GetResponseAsync() as HttpWebResponse;

            // Make sure the webserver has sent a response
            if (response == null) return null;

            dataStream = response.GetResponseStream();

            // Make sure the datastream with the response is available
            if (dataStream == null) return null;

            var reader = new StreamReader(dataStream);
            return await reader.ReadToEndAsync();
        }
    }
}
