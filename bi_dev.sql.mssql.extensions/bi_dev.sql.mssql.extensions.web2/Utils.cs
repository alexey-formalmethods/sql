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
        public Cookie GetCookie(string domain)
        {
            var cookie = new Cookie(this.Name, this.Value);
            cookie.Path = (!string.IsNullOrEmpty(this.Path)) ? this.Path : "/";
            cookie.Domain = (!string.IsNullOrEmpty(this.Domain)) ? this.Domain : domain;
            return cookie;
        }
    }
    public class WebRequestArgumentProxy
    {
        [JsonProperty(PropertyName="address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName="username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName="password")]
        public string Password { get; set; }
        public IWebProxy GetProxy()
        {

            if (!string.IsNullOrEmpty(this.Username))
            {
                ICredentials credentials = new NetworkCredential(this.Username, this.Password);
                return new WebProxy(new Uri(this.Address), true, null, credentials);

            }
            else
            {
                return new WebProxy(new Uri(this.Address));
            }
            
        }
    }
    public class WebRequestResult
    {

        [JsonProperty(PropertyName = "is_success")]
        public bool IsSuccess { get; set; }

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

        [JsonProperty(PropertyName = "file_size")]
        public long FileSize { get; set; }
        public WebRequestResult(WebRequestArgument request)
        {
            this.Request = request;
            this.Headers = new List<WebRequestHeader>();
            this.Cookies = new List<WebRequestCookie>();
        }
    }
    public class WebRequestArgumentCredential
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
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
        public int TimeOutMilliseconds { get; set; }

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

        [JsonProperty(PropertyName = "accepted_codes")]
        public IEnumerable<int> AccesptedResponseCodes { get; set; }

        [JsonProperty(PropertyName = "network_credential")]
        public WebRequestArgumentCredential NetworkCredential { get; set; }
        public WebRequestArgument()
        {
            this.AllowAutoRedirect = true;
            this.Headers = new WebRequestHeader[] {
                new WebRequestHeader() { Name = "Content-Type", Value = "applicaion/json" },
                new WebRequestHeader() { Name = "User-Agent", Value = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36"}
            };
            this.Method = "GET";
            this.TimeOutMilliseconds = 100_000;
            this.Attempts = 1;
            this.MillisecondsToRetry = 0;
            this.CodePage = 65001;
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
                    Address = "http://127.0.0.1:8000/",
                    Username = "(optional) username",
                    Password = "(optional) password"
                },
                TimeOutMilliseconds = 100000
            };
            return e;
        }
    }
    public static class Utils
    {
        private static ICollection<WebRequestCookie> ToWebRequestCookies(this CookieCollection cookieCollection)
        {
            ICollection<WebRequestCookie> cookies = new List<WebRequestCookie>();
            foreach (Cookie responseCookie in cookieCollection)
            {
                cookies.Add(new WebRequestCookie()
                {
                    Domain = responseCookie.Domain,
                    Name = responseCookie.Name,
                    Path = responseCookie.Path,
                    Value = responseCookie.Value
                });
            }
            return cookies;
        }
        private static ICollection<WebRequestHeader> ToWebRequestHeaders(this WebHeaderCollection webHeaderCollection)
        {
            ICollection<WebRequestHeader> headers = new List<WebRequestHeader>();
            for (int i = 0; i < webHeaderCollection.Count; i++)
            {
                string header = webHeaderCollection.GetKey(i);
                foreach (string value in webHeaderCollection.GetValues(i))
                {
                    headers.Add(new WebRequestHeader()
                    {
                        Name = header,
                        Value = value
                    });
                }
            }
            return headers;
        }
        public static WebRequestResult ProcessWebRequest(WebRequestArgument webRequestArgument, bool ignoreResponseErrors, int attempt = 0)
        {
            string[] mainHttpRequestHeaders = new string[]
            {
                "Content-Type",
                "User-Agent",
                "Cookies",
                "Accept"
            };
             
            WebRequestResult result = new WebRequestResult(webRequestArgument);
            result.Request = webRequestArgument;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = 
                     SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            HttpWebRequest r = WebRequest.Create(webRequestArgument.Url) as HttpWebRequest;
            r.Method = webRequestArgument.Method;
            r.Timeout = webRequestArgument.TimeOutMilliseconds;
            r.AllowAutoRedirect = webRequestArgument.AllowAutoRedirect;
            r.ContentType = webRequestArgument.Headers?.FirstOrDefault(x => x.Name.ToLower() == "content-type")?.Value ?? "applicaion/json";
            r.UserAgent = webRequestArgument.Headers?.FirstOrDefault(x => x.Name.ToLower() == "user-agent")?.Value ?? "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
            string accept = webRequestArgument.Headers?.FirstOrDefault(x => x.Name.ToLower() == "accept")?.Value;
            if (!string.IsNullOrWhiteSpace(accept))
            {
                r.Accept = accept;
            }
            if (webRequestArgument.NetworkCredential != null)
            {
                CredentialCache myCredentialCache = new CredentialCache();
                myCredentialCache.Add(new Uri(webRequestArgument.Url), "Basic", 
                new NetworkCredential(
                    webRequestArgument.NetworkCredential.UserName,
                    webRequestArgument.NetworkCredential.Password
                ));
                r.Credentials = myCredentialCache;
            }
            if (webRequestArgument.Proxy != null)
            {
                r.Proxy = webRequestArgument.Proxy.GetProxy();
            }
            r.CookieContainer = new CookieContainer();
            if (webRequestArgument.Cookies != null)
            {
                foreach (var cookie in webRequestArgument.Cookies)
                {
                    r.CookieContainer.Add(cookie.GetCookie(new Uri(webRequestArgument.Url).Host));
                }
            }
            if (webRequestArgument.Headers != null)
            {
                foreach (var header in webRequestArgument.Headers.Where(x => !mainHttpRequestHeaders.Any(t => t == x.Name)))
                {
                    if (!(header.Name.ToLower() == "authorization" && webRequestArgument.NetworkCredential != null))
                    {
                        if (!r.Headers.AllKeys.Contains(header.Name) && !string.IsNullOrEmpty(header.Name))
                        {
                            r.Headers.Add($@"{header.Name}:{header.Value}");
                        }
                    }
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
            else
            {
                r.ContentLength = 0;
            }
            try
            {
                using (var response = r.GetResponse() as HttpWebResponse)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (!string.IsNullOrWhiteSpace(webRequestArgument?.FileName))
                        {
                            System.IO.Directory.CreateDirectory(new FileInfo(webRequestArgument?.FileName).Directory.FullName);
                            if (File.Exists(webRequestArgument?.FileName))
                            {
                                File.Delete(webRequestArgument?.FileName);
                            }
                            using (FileStream os = new FileStream(webRequestArgument?.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                byte[] buff = new byte[102400];
                                int c = 0;
                                if (responseStream.CanRead)
                                {
                                    while ((c = responseStream.Read(buff, 0, 10400)) > 0)
                                    {
                                        os.Write(buff, 0, c);
                                        os.Flush();
                                    }
                                }
                            }
                            if (File.Exists(webRequestArgument?.FileName))
                            {
                                result.FileSize = new FileInfo(webRequestArgument?.FileName).Length;
                            }
                        }
                        else
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                result.ResponseText = reader.ReadToEnd();
                            }
                        }
                    }
                    result.StatusCode = (int)response.StatusCode;
                    if (response.Cookies != null)
                    {
                        result.Cookies = response.Cookies.ToWebRequestCookies();
                    }
                    if (response.Headers != null)
                    {
                        result.Headers = response.Headers.ToWebRequestHeaders();
                    }
                }
                result.IsSuccess = true;
            }
            catch (WebException we)
            {
                using (var response = we.Response as HttpWebResponse)
                {
                    bool isCodeAccepted = webRequestArgument.AccesptedResponseCodes?.Any(x => x == (int?)response?.StatusCode) ?? false;
                    if (attempt <= webRequestArgument.Attempts && !isCodeAccepted)
                    {
                        Thread.Sleep(webRequestArgument.MillisecondsToRetry);
                        result = ProcessWebRequest(webRequestArgument, ignoreResponseErrors, ++attempt);
                    }
                    else
                    {
                        if (ignoreResponseErrors || isCodeAccepted)
                        {
                            result.IsSuccess = isCodeAccepted;
                            using (var responseStream = response.GetResponseStream())
                            {
                                using (var responseStreamReader = new StreamReader(responseStream))
                                {
                                    result.ResponseText = responseStreamReader.ReadToEnd();
                                }
                            }
                            result.StatusCode = (int)response.StatusCode;
                            result.Cookies = response.Cookies?.ToWebRequestCookies();
                            result.Headers = response.Headers?.ToWebRequestHeaders();
                        }
                        else
                        {
                            throw we;
                        }
                    }
                }
            }
            return result;
        }
        
        public static string WebProcessRequest(string jsonArgument, bool ignoreResponseErrors)
        {
            try
            {
                WebRequestArgument argument = JsonConvert.DeserializeObject<WebRequestArgument>(jsonArgument);
                return JsonConvert.SerializeObject(ProcessWebRequest(argument, ignoreResponseErrors));
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, ignoreResponseErrors);
            }
        }
        
    }
}
