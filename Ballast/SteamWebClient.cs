using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace Ballast
{
    /// <summary>
    /// For passing cookies to Steam's Store page to bypass age verification.
    /// This doesn't really need to be in a seperate class file but for readability
    /// </summary>
    internal class SteamWebClient
    {
        //Cookies data
        private CookieContainer _cookies = new CookieContainer();

        //In case of needing to clear the cookies
        public void ClearCookies()
        {
            _cookies = new CookieContainer();
        }

        //Sends a web request and returns the HTML with the cookies
        public HtmlDocument GetPage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            Uri uri = new Uri(url);
            _cookies.Add(new Cookie("birthtime", "568022401") { Domain = uri.Host });
            request.CookieContainer = _cookies;


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            using (var reader = new StreamReader(stream))
            {
                string html = reader.ReadToEnd();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
        }
    }
}
