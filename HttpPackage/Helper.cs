using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpPackage
{
    public static class Helper
    {
        public static async Task<string> RedirectGet_1(string Url, List<Cookie> cookiesAppend)
        {
            var redirectUrl = string.Empty;

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
            handler.CookieContainer = cookies;

            using (var client = new System.Net.Http.HttpClient(handler))
            {

                using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Url)))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                    request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "zh-CN");
                    request.Headers.TryAddWithoutValidation("Connection", "Keep-Alive");
                    request.Headers.TryAddWithoutValidation("UA-CPU", "AMD64");

                    foreach (Cookie cookie in cookiesAppend)
                    {
                        request.Headers.TryAddWithoutValidation("Cookie", cookie.Name + "=" + cookie.Value);
                    }

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        var responseCookies = cookies.GetCookies(response.RequestMessage.RequestUri).Cast<Cookie>();
                        foreach (Cookie cookie in responseCookies)
                        {
                            if (cookiesAppend.SingleOrDefault((item) => item.Name == cookie.Name) != null)
                            {
                                Cookie itemDeleted = cookiesAppend.SingleOrDefault((item) => item.Name == cookie.Name);
                                cookiesAppend.Remove(itemDeleted);
                            }
                            cookiesAppend.Add(cookie);
                        }
                        if ((int)response.StatusCode == 302)
                        {
                            redirectUrl = response.Headers.Location.AbsoluteUri;
                        }
                    }
                }
            }
            return redirectUrl;
        }
    }
}