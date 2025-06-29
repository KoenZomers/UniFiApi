using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KoenZomers.UniFi.Api;

/// <summary>
/// Internal utility class for Http communication with the UniFi Controller
/// </summary>
internal class HttpUtility
{
    #region Fields

    /// <summary>
    /// Cookie container which holds all cookies for sessions towards the UniFi Controller
    /// </summary>
    private CookieContainer _cookieContainer = new();

    /// <summary>
    /// HttpClient used to perform requests
    /// </summary>
    internal HttpClient HttpClient;

    #endregion

    #region Constructors

    public HttpUtility(Uri baseUri, short timeout = 10000, bool ignoreSslValidation = false)
    {
        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer,
            UseCookies = true,
            AllowAutoRedirect = true
        };

        if (ignoreSslValidation)
        {
            httpClientHandler.ServerCertificateCustomValidationCallback = (HttpRequestMessage req, X509Certificate2 cert, X509Chain chain, SslPolicyErrors errors) => true;
        }

        HttpClient = new HttpClient(httpClientHandler)
        {
            BaseAddress = baseUri,
            Timeout = TimeSpan.FromMilliseconds(timeout)
        };
    }

    #endregion

    #region Methods

    public void ClearCookies()
    {
        _cookieContainer = new CookieContainer();
    }

    /// <summary>
    /// Performs a GET request to the provided url to download the page contents
    /// </summary>
    /// <param name="url">Url of the page to retrieve</param>
    /// <returns>Contents of the page</returns>
    public async Task<string> GetRequestResult(Uri url)
    {
        var responseFromServer = await HttpClient.GetStringAsync(url);
        return responseFromServer;
    }

    /// <summary>
    /// Sends a POST request towards UniFi
    /// </summary>
    /// <param name="url">Url to POST the postData to</param>
    /// <param name="postData">Data to send to the UniFi service, typically a JSON payload</param>
    /// <returns>The website contents returned by the webserver after posting the data</returns>
    public async Task<string> PostRequest(Uri url, string postData)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

        // Check if the have a Cross Site Request Forgery cookie and if so, add it as the X-Csrf-Token header which is required by UniFi when making a POST
        var csrfCookie = _cookieContainer.GetAllCookies().FirstOrDefault(c => c.Name == "csrf_token");
        if (csrfCookie != null)
        {
            request.Headers.Add("X-Csrf-Token", csrfCookie.Value);
        }

        request.Content = new StringContent(postData, Encoding.UTF8, "application/json");

        var response = await HttpClient.SendAsync(request);

        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }

    /// <summary>
    /// Sends a POST request with JSON variables to authenticate against UniFi
    /// </summary>
    /// <param name="url">Url to POST the login information to</param>
    /// <param name="username">Username to authenticate with</param>
    /// <param name="password">Password to authenticate with</param>
    /// <returns>The website contents returned by the webserver after posting the data</returns>
    public async Task<string> AuthenticateViaJsonPostMethod(Uri url, string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
        request.Headers.ExpectContinue = false;

        // Construct POST data
        var postData = string.Concat(@"{""username"":""", username, @""",""password"":""", password.Replace("\"", "\\\""), @""",""remember"":false,""strict"":true}");

        request.Content = new StringContent(postData, Encoding.UTF8, "application/json");

        var response = await HttpClient.SendAsync(request);

        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }

    /// <summary>
    /// Sends a POST request to log out from the UniFi Controller
    /// </summary>
    /// <param name="url">Url to POST the logout request to</param>
    /// <returns>The website contents returned by the webserver after posting the data</returns>
    public async Task<string> LogoutViaJsonPostMethod(Uri url)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
        request.Headers.ExpectContinue = false;

        // Construct POST data
        var postData = "{}";

        request.Content = new StringContent(postData, Encoding.UTF8, "application/json");

        var response = await HttpClient.SendAsync(request);

        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }

    #endregion
}