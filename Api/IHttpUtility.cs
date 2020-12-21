using System;
using System.Net;
using System.Threading.Tasks;

namespace KoenZomers.UniFi.Api
{
    public interface IHttpUtility
    {
        Task<string> AuthenticateViaJsonPostMethod(Uri url, string username, string password, CookieContainer cookieContainer, int timeout = 60000);
        void DisableSslValidation();
        void EnableTls11and12();
        Task<string> GetRequestResult(Uri url, CookieContainer cookieContainer, int timeout = 60000);
        Task<string> LogoutViaJsonPostMethod(Uri url, CookieContainer cookieContainer, int timeout = 60000);
        Task<string> PostRequest(Uri url, string postData, CookieContainer cookieContainer, int timeout = 60000);
    }
}