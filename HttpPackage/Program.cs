using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Primitives;

namespace HttpPackage
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Cookie> cookies = new List<Cookie>();

            string redirectUrl_1= Get(cookies).GetAwaiter().GetResult();
            string redirectUrl_2=Helper.RedirectGet_1(redirectUrl_1,cookies).GetAwaiter().GetResult();
            string redirectUrl_3=Helper.RedirectGet_1(redirectUrl_2,cookies).GetAwaiter().GetResult();

            // string result = Post(obj).GetAwaiter().GetResult();
            // IEnumerable<Cookie> result_1 = Post_2(obj).GetAwaiter().GetResult();
            // string result_2 = Get_2(obj, result_1).GetAwaiter().GetResult();
            // Console.WriteLine(result);
            Console.Read();
        }

        static async Task<string> MainAsync()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(@"https://developer.api.autodesk.com");
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", "Ok22ZsNMG7eChAnAsbOJ9m3uPAvIJOEH"),
                    // new KeyValuePair<string, string>("client_secret", "GPW5h2wyGlPu7N2G"),
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("scope", "data:read")
                });
                var result = await client.PostAsync("/authentication/v1/authenticate", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }
        }


        static async Task<string> Post(ReturnObject obj)
        {
            string result = string.Empty;


            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new System.Net.Http.HttpClient(handler))
            {
                var builder = new UriBuilder("https://accounts.autodesk.com/Authentication/IsExistingUser");
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["resume"] = obj.Value;
                query["ack"] = "Ok22ZsNMG7eChAnAsbOJ9m3uPAvIJOEH";
                builder.Query = query.ToString();
                string url = builder.ToString();

                using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url)))
                {
                    request.Headers.TryAddWithoutValidation("Access-Control-Allow-Origin", "*");
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "zh-CN");
                    request.Headers.TryAddWithoutValidation("Connection", "Keep-Alive");
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    request.Headers.TryAddWithoutValidation("UA-CPU", "AMD64");

                    request.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>(obj.Cookies.Name, obj.Cookies.Value),
                        new KeyValuePair<string, string>("UserName", "wangaofang@126.com")
                    });

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        result = response.ToString();
                    }
                }
            }
            return result;

        }

        static async Task<IEnumerable<Cookie>> Post_2(ReturnObject obj)
        {
            IEnumerable<Cookie> result = null;

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
            handler.CookieContainer = cookies;

            using (var client = new System.Net.Http.HttpClient(handler))
            {
                var builder = new UriBuilder("https://accounts.autodesk.com/Authentication/LogOn");
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["resume"] = obj.Value;
                query["ack"] = "Ok22ZsNMG7eChAnAsbOJ9m3uPAvIJOEH";
                builder.Query = query.ToString();
                string url = builder.ToString();

                using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url)))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "zh-CN");
                    request.Headers.TryAddWithoutValidation("Connection", "Keep-Alive");
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    request.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    request.Headers.TryAddWithoutValidation("UA-CPU", "AMD64");

                    request.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>(obj.Cookies.Name, obj.Cookies.Value),
                        new KeyValuePair<string, string>("Password", "xxx"),
                        new KeyValuePair<string, string>("queryStrings", request.RequestUri.Query),
                        new KeyValuePair<string, string>("RememberMe", "false") ,
                        new KeyValuePair<string, string>("signinThrottledMessage", "您最近尝试登录的次数过多。请稍后重试。") ,
                        new KeyValuePair<string, string>("UserName", "wangaofang@126.com")
                    });

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        result = cookies.GetCookies(response.RequestMessage.RequestUri).Cast<Cookie>();
                    }
                }
            }
            return result;

        }


        static async Task<string> Get(List<Cookie> cookiesAppend)
        {
            var redirectUrl=string.Empty;
            
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
            handler.CookieContainer = cookies;

            using (var client = new System.Net.Http.HttpClient(handler))
            {
                var builder = new UriBuilder("https://developer.api.autodesk.com/authentication/v1/authorize");
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["client_id"] = "Ok22ZsNMG7eChAnAsbOJ9m3uPAvIJOEH";
                query["redirect_uri"] = "http://localhost:3000/api/forge/callback/oauth";
                query["response_type"] = "code";
                query["scope"] = "data:read viewables:read";
                builder.Query = query.ToString();
                string url = builder.ToString();

                using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url)))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                    request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "zh-CN");
                    request.Headers.TryAddWithoutValidation("Connection", "Keep-Alive");
                    request.Headers.TryAddWithoutValidation("UA-CPU", "AMD64");

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        var responseCookies = cookies.GetCookies(response.RequestMessage.RequestUri).Cast<Cookie>();
                        cookiesAppend.AddRange(responseCookies);
                        if((int)response.StatusCode==302)
                        {
                            redirectUrl=response.Headers.Location.AbsoluteUri;
                        }                       
                    }
                }
            }
            return redirectUrl;
        }


        static async Task<string> Get_2(ReturnObject obj, IEnumerable<Cookie> result_1)
        {
            string result = string.Empty;

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new System.Net.Http.HttpClient(handler))
            {
                var builder = new UriBuilder("https://accounts.autodesk.com/authorize");
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["resume"] = obj.Value;
                query["ack"] = "Ok22ZsNMG7eChAnAsbOJ9m3uPAvIJOEH";
                builder.Query = query.ToString();
                string url = builder.ToString();

                using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url)))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                    request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "zh-CN");
                    request.Headers.TryAddWithoutValidation("Connection", "Keep-Alive");
                    request.Headers.TryAddWithoutValidation("UA-CPU", "AMD64");
                    foreach (Cookie cookie in result_1)
                    {
                        request.Headers.TryAddWithoutValidation("Cookie", cookie.Name + "=" + cookie.Value);
                    }
                    request.Headers.TryAddWithoutValidation("Cookie", obj.Cookies.Name + "=" + obj.Cookies.Value);

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        var responseCookies = cookies.GetCookies(response.RequestMessage.RequestUri).Cast<Cookie>();
                        var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(response.RequestMessage.RequestUri.Query);
                        result = queryDictionary.ToString();
                    }
                }
            }
            return result;
        }

        static async Task<string> Get_1()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                // client.BaseAddress = new Uri("https://developer.api.autodesk.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-CN"));
                client.DefaultRequestHeaders.Connection.Add("Keep-Alive");
                // client.DefaultRequestHeaders.Host="developer.api.autodesk.com";
                // client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(@"Mozilla/5.0 (Windows NT 6.1; Win64; x64; Trident/7.0; rv:11.0) like Gecko"));

                var builder = new UriBuilder("http://auth.autodesk.com/as/authorization.oauth2");
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["aud"] = "https://autodesk.com/aud/jwtexp60";
                query["client_id"] = "Ok22ZsNMG7eChAnAsbOJ9m3uPAvIJOEH";
                query["pfidpadapterid"] = "Oauth2OpenTokenIDPAdapter";
                query["redirect_uri"] = "http://localhost:3000/api/forge/callback/oauth";
                query["response_type"] = "code";
                query["scope"] = "data:read viewables:read";
                builder.Query = query.ToString();
                string url = builder.ToString();
                HttpResponseMessage response = await client.GetAsync(url);
                return response.ToString();
            }
        }
    }
}
