using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.web2
{
    public class WebRequestHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }   
    public class WebRequestCookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
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
        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
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
        public WebRequestArgument Request { get; set; }
        public string ResponseText { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<WebRequestCookie> Cookies { get; set; }
        public IEnumerable<WebRequestHeader> Headers { get; set; }
    }
    public class WebRequestArgument
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }
        public int TimeOutMilliSeconds { get; set; }
        public IEnumerable<WebRequestHeader> Headers { get; set; }
        public IEnumerable<WebRequestCookie> Cookies { get; set; }
        public bool AllowAutoRedirect { get; set; }
        public WebRequestArgumentProxy Proxy { get; set; }

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
        /*
        private static WebRequestResult ProcessWebRequest(WebRequestArgument webRequestArgument, bool ignoreErrors)
        {
            var defaults = WebRequestArgument.GetDefault();
            HttpWebRequest r = WebRequest.Create(webRequestArgument.Url) as HttpWebRequest;
            r.Method = webRequestArgument.Method;
            r.Timeout
            r.Headers[HttpRequestHeader.ContentType] = webRequestArgument.Headers.FirstOrDefault(x => x.Name == "Content-Type")?.Value ?? "applicaion/json";
            r.Headers[HttpRequestHeader.UserAgent] = webRequestArgument.Headers.FirstOrDefault(x => x.Name == "User-Agent")?.Value ?? "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
            r.CookieContainer.Add();
        }
        */
    }
}
