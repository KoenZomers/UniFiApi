using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace KoenZomers.UniFi.Api
{
    /// <summary>
    /// Internal utility class for working with cookies
    /// </summary>
    internal static class CookieUtility
    {
        /// <summary>
        /// Extracts all the cookies from a cookie container so the contents can be read and used
        /// </summary>
        /// <remarks>Code sample retrieved from https://stackoverflow.com/a/31900670/1271303 </remarks>
        /// <returns>IEnumerable containing all cookies available in the CookieContainer</returns>
        internal static IEnumerable<Cookie> GetAllCookies(this CookieContainer c)
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
    }
}
