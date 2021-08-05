using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace bi_dev.sql.mssql.extensions.web2
{
    public class WebRequestHeader
    {
        [JsonProperty(PropertyName="name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName="value")]
        public string Value { get; set; }
    }   
    public class WebRequestCookie
    {
        [JsonProperty(PropertyName="name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName="value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName="domain")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName="path")]
        public string Path { get; set; }
        public Cookie GetCookie()
        {
            var cookie = new Cookie(this.Name, this.Value);
            if (!string.IsNullOrEmpty(this.Path)) { cookie.Path = this.Path; }
            if (!string.IsNullOrEmpty(this.Domain)) { cookie.Path = this.Domain; }
            return cookie;
        }
    }
    public class WebRequestArgumentProxy
    {
        [JsonProperty(PropertyName="address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName="port")]
        public int Port { get; set; }

        [JsonProperty(PropertyName="username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName="password")]
        public string Password { get; set; }
        public IWebProxy GetProxy()
        {
            var proxy =  new WebProxy(this.Address, this.Port);
            if (!string.IsNullOrEmpty(this.Username))
            {
                proxy.Credentials = new NetworkCredential(this.Username, this.Password);
                proxy.UseDefaultCredentials = false;
            }
            return proxy;
        }
    }
    public class WebRequestResult
    {
        [JsonProperty(PropertyName="request")]
        public WebRequestArgument Request { get; set; }

        [JsonProperty(PropertyName="response_text")]
        public string ResponseText { get; set; }

        [JsonProperty(PropertyName="status_code")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName="cookies")]
        public ICollection<WebRequestCookie> Cookies { get; set; }

        [JsonProperty(PropertyName="headers")]
        public ICollection<WebRequestHeader> Headers { get; set; }
    }
    public class WebRequestArgument
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "time_out_milliseconds")]
        public int TimeOutMilliSeconds { get; set; }

        [JsonProperty(PropertyName = "code_page")]
        public int CodePage { get; set; }

        [JsonProperty(PropertyName = "headers")]
        public IEnumerable<WebRequestHeader> Headers { get; set; }

        [JsonProperty(PropertyName = "cookies")]
        public IEnumerable<WebRequestCookie> Cookies { get; set; }

        [JsonProperty(PropertyName = "allow_auto_redirect")]
        public bool AllowAutoRedirect { get; set; }

        [JsonProperty(PropertyName = "proxy")]
        public WebRequestArgumentProxy Proxy { get; set; }

        [JsonProperty(PropertyName = "attempts")]
        public int Attempts { get; set; }

        [JsonProperty(PropertyName = "milliseconds_to_retry")]
        public int MillisecondsToRetry { get; set; }

        public static WebRequestArgument GetDefault()
        {
            WebRequestArgument d = new WebRequestArgument()
            {
                AllowAutoRedirect = true,
                Headers = new WebRequestHeader[] {
                    new WebRequestHeader() { Name = "Content-Type", Value = "applicaion/json" },
                    new WebRequestHeader() { Name = "User-Agent", Value = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36"}
                },
                Method = "GET",
                TimeOutMilliSeconds = 100_000
            };
            return d;
        }
        public static WebRequestArgument GetExammple()
        {
            WebRequestArgument e = new WebRequestArgument()
            {
                AllowAutoRedirect = true,
                Body = "",
                Cookies = new WebRequestCookie[] { new WebRequestCookie() { Name = "cookie_1_name", Value = "cookie_1_value" } },
                Headers = new WebRequestHeader[] { 
                    new WebRequestHeader() { Name = "Content-Type", Value = "applicaion/json" },
                    new WebRequestHeader() { Name = "User-Agent", Value = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36"}
                },
                FileName = "C://storage/hdd01/file_name.json",
                Method = "POST",
                Url = "https://google.com",
                Proxy = new WebRequestArgumentProxy()
                {
                    Address = "127.0.0.1",
                    Port = 83055,
                    Username = "(optional) username",
                    Password = "(optional) password"
                },
                TimeOutMilliSeconds = 100000
            };
            return e;
        }
    }
    public static class Utils
    {
        private static WebRequestResult ProcessWebRequest(WebRequestArgument webRequestArgument)
        {
            string[] mainHttpRequestHeaders = new string[]
            {
                "Content-Type",
                "User-Agent",
                "Cookies"
            };
            WebRequestResult result = new WebRequestResult();
            result.Request = webRequestArgument;
            var defaults = WebRequestArgument.GetDefault();
            HttpWebRequest r = WebRequest.Create(webRequestArgument.Url) as HttpWebRequest;
            r.Method = webRequestArgument.Method;
            r.Timeout = webRequestArgument.TimeOutMilliSeconds;
            r.Headers[HttpRequestHeader.ContentType] = webRequestArgument.Headers.FirstOrDefault(x => x.Name == "Content-Type")?.Value ?? "applicaion/json";
            r.Headers[HttpRequestHeader.UserAgent] = webRequestArgument.Headers.FirstOrDefault(x => x.Name == "User-Agent")?.Value ?? "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
            foreach (var cookie in webRequestArgument.Cookies)
            {
                r.CookieContainer.Add(cookie.GetCookie());
            }
            foreach (var header in webRequestArgument.Headers.Where(x=>mainHttpRequestHeaders.Any(t=>t == x.Name)))
            {
                if (!r.Headers.AllKeys.Contains(header.Name) && string.IsNullOrEmpty(header.Name))
                {
                    r.Headers.Add($@"{header.Name}:{header.Value}");
                }
            }
            if (webRequestArgument.Body != null)
            {
                var encoding = Encoding.GetEncoding(webRequestArgument.CodePage);
                byte[] byteBody = encoding.GetBytes(webRequestArgument.Body);
                r.ContentLength = byteBody.Length;
                using (var requestStream = r.GetRequestStream())
                {
                    requestStream.Write(byteBody, 0, byteBody.Length);
                }
            }
            int attempt = 1;
            while (attempt <= webRequestArgument.Attempts) {
                try
                {
                    using (var response = r.GetResponse() as HttpWebResponse)
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                result.ResponseText = reader.ReadToEnd();
                            }
                        }
                        result.StatusCode = (int)response.StatusCode;
                        result.Cookies = new List<WebRequestCookie>();
                        if (response.Cookies != null)
                        {
                            foreach (Cookie responseCookie in response.Cookies)
                            {
                                result.Cookies.Add(new WebRequestCookie()
                                {
                                    Domain = responseCookie.Domain,
                                    Name = responseCookie.Name,
                                    Path = responseCookie.Path,
                                    Value = responseCookie.Value
                                });
                            }
                        }
                        result.Headers = new List<WebRequestHeader>();
                        if (response.Headers != null)
                        {
                            for (int i = 0; i < response.Headers.Count; i++)
                            {
                                string header = response.Headers.GetKey(i);
                                foreach (string value in response.Headers.GetValues(i))
                                {
                                    result.Headers.Add(new WebRequestHeader()
                                    {
                                        Name = header,
                                        Value = value
                                    });
                                }
                            }
                        }
                    }
                    break;
                }
                catch (WebException we)
                {
                    if (attempt <= webRequestArgument.Attempts)
                    {
                        attempt++;
                        Thread.Sleep(webRequestArgument.MillisecondsToRetry);
                    }
                    else
                    {
                        throw we;
                    }
                }
            }
            return result;
        }
        
        public static string WebProcessRequest(string jsonArgument, bool ignoreResponseErrors)
        {
            WebRequestArgument argument = JsonConvert.DeserializeObject<WebRequestArgument>(jsonArgument);
            return JsonConvert.SerializeObject(ProcessWebRequest(argument));
        }
        
    }
}
