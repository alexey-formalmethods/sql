using Microsoft.SqlServer.Server;
using Mono.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.web
{
    public static class Utils
    {
        [SqlFunction]
        public static string Get(string url, string headersInUrlFormat, bool nullWhenError)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (!string.IsNullOrWhiteSpace(headersInUrlFormat)) wc.Headers.Add(HttpUtility.ParseQueryString(headersInUrlFormat));
                return wc.DownloadString(url);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static string Post(string url, string body,  string headersInUrlFormat, bool nullWhenError)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                if (!string.IsNullOrWhiteSpace(headersInUrlFormat)) wc.Headers.Add(HttpUtility.ParseQueryString(headersInUrlFormat));
                return wc.UploadString(url, body);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public class WebRequestResult
        {

            public string Url { get; set; }
            public string Body { get; set; }
            public string ResponseText { get; set; }
            public HttpStatusCode StatusCode { get; set; } 
            public WebHeaderCollection ResponseHeaders { get; set; }
            public CookieCollection Cookies { get; set; }

        }
        private static WebRequestResult ProcessWebRequest(string url, string method, string body, string contentType, int? codePage, Dictionary<string, string> headers, Dictionary<string, string> cookies, bool allowAutoRedirect)
        {
            WebRequestResult result = new WebRequestResult();
            result.Url = url;
            result.Body = body;
            try
            {
                HttpWebRequest r = (HttpWebRequest)WebRequest.Create(url);
                r.Method = method;
                r.CookieContainer = new CookieContainer();
                if (cookies != null)
                {
                    Uri target = new Uri(url);
                    foreach (var cookie in cookies)
                    {
                        r.CookieContainer.Add(new Cookie(cookie.Key, cookie.Value, "/", target.Host));
                    }
                }
                if (headers != null)
                {
                    var defaultHeaders = Enum.GetValues(typeof(HttpRequestHeader)).Cast<HttpRequestHeader>();
                    foreach (var header in headers)
                    {
                        var defaultHeader = defaultHeaders.Where(x => x.ToString().ToLower() == header.Key.ToLower());

                        if (defaultHeader.Count() > 0)
                        {
                            r.Headers[defaultHeader.FirstOrDefault()] = header.Value;
                        }
                        else
                        {
                            r.Headers[header.Key] = header.Value;
                        }
                    }
                }
                int currentCodePage = (codePage.HasValue) ? codePage.Value : 65001; // Default Unicode;
                if (body != null)
                {
                    var encoding = Encoding.UTF8;//.GetEncoding(currentCodePage);
                    byte[] bytes = encoding.GetBytes(body);
                    //r.ContentLength = bytes.Length;
                    using (var stream = r.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    r.ContentType = contentType;
                }
                string responseText;
                r.AllowAutoRedirect = allowAutoRedirect;

                using (HttpWebResponse response = (HttpWebResponse)r.GetResponse())
                {
                    result.Cookies = response.Cookies;
                    result.ResponseHeaders = response.Headers;
                    using (var s = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(s, Encoding.UTF8))
                        {
                            responseText = reader.ReadToEnd();
                            result.ResponseText = responseText;
                        }
                    }
                    return result;
                }
            }
            catch (WebException e)
            {
                var respone = (HttpWebResponse)e.Response;
                using (var responseStream = respone.GetResponseStream())
                {
                    result.Cookies = respone.Cookies;
                    using (var reader = new StreamReader(responseStream))
                    {
                        result.ResponseText = reader.ReadToEnd();
                        result.StatusCode = respone.StatusCode;
                    }
                }
                return result;
            }
        }
        public class TableType
        {
            public string RowType { get; set; }
            public string RowValue { get; set; }
        }
        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable InitMethod(String logname)
        {
            List<TableType> l = new List<TableType> { new TableType { RowType = "a", RowValue = "b" }, new TableType { RowType = "a1", RowValue = "b2" } };
            return l;
        }
        public static void FillRow(Object obj, out SqlChars rowType, out SqlChars value)
        {
            TableType table = (TableType)obj;
            rowType = new SqlChars(table.RowType);
            value = new SqlChars(table.RowValue);
        }
    }
}
