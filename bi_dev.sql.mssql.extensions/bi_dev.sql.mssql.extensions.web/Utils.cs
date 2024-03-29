﻿using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bi_dev.sql.mssql.extensions.web
{
    //test2
    public static class Utils
    {
        /// <summary>
        /// Creates new Exception with WebResponse if exists
        /// </summary>
        /// <param name="e">common Exception</param>
        /// <returns>Exception object with inner Exception with WebResponse</returns>
        private static Exception AddWebException(this Exception e)
        {
            if (e.GetType() == typeof(WebException))
            {
                var response = (HttpWebResponse)((WebException)e).Response;
                if (response != null)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                if (reader != null)
                                {
                                    e = new Exception(e.Message, new Exception(reader.ReadToEnd()));
                                }
                            }
                        }
                    }
                }
            }
            return e;
        }
        private static void FixSecurityProtocol()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        }
        
        private static WebClient AddHeaders(this WebClient client, string headersInUrlFormat)
        {
            if (!string.IsNullOrWhiteSpace(headersInUrlFormat))
            {
                var headers = HttpUtility.ParseQueryString(headersInUrlFormat);
                foreach (var header in headers.AllKeys)
                {
                    if (!client.Headers.AllKeys.Contains(header))
                    {
                        client
                        .Headers
                        .Add(header, headers[header]);
                    }
                }
            }
            return client;
        }
        private static string get(WebClient client, string url,  bool nullWhenError)
        {
            try
            {
                
                FixSecurityProtocol();
                return client.DownloadString(url);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e.AddWebException(), nullWhenError);
            }
        }

        [SqlFunction]
        public static string Get(string url, string headersInUrlFormat, bool nullWhenError)
        {
                WebClient wc = new WebClient();
                return get(wc.AddHeaders(headersInUrlFormat), url, nullWhenError);
        }
/*
        private static HttpClient AddHeaders_HC(this HttpClient client, string headersInUrlFormat)
        {
            if (!string.IsNullOrWhiteSpace(headersInUrlFormat))
            {
                var headers = HttpUtility.ParseQueryString(headersInUrlFormat);
                foreach (var header in headers.AllKeys)
                {
                    if (header != "Content-Type" && !client.DefaultRequestHeaders.Contains(header))
                    {
                        client.DefaultRequestHeaders.Add(header, headers[header]);
                    }
                }
            }
            return client;
        }

        private static MultipartFormDataContent GetFilesContent(string filesInUrlFormat)
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();

            if (!string.IsNullOrWhiteSpace(filesInUrlFormat))
            {
                var files = HttpUtility.ParseQueryString(filesInUrlFormat);

                if (files.Count > 0)
                {
                    foreach (var file in files.AllKeys)
                    {
                        FileStream fileStream = new FileStream(files[file], FileMode.Open, FileAccess.Read);
                        formData.Add(new StreamContent(fileStream), file, Path.GetFileName(files[file]));
                    }
                }

            }

            return formData;
        }

        [SqlFunction]
        public static string Post_HC(string url, string headersInUrlFormat, string filesInUrlFormat, bool nullWhenError)
        {
            HttpClient hc = new HttpClient();

            var response = post_HC(hc.AddHeaders_HC(headersInUrlFormat), url, GetFilesContent(filesInUrlFormat), nullWhenError);

            return response.Result;
        }

        private static async Task<string> post_HC(HttpClient client, string url, MultipartFormDataContent content, bool nullWhenError)
        {
            try
            {

                FixSecurityProtocol();

                var response = await client.PostAsync(url, content);
                if (response != null)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return jsonString;
                }

                return null;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e.AddWebException(), nullWhenError);
            }
        }
*/
        [SqlFunction]
        public static string GetWithProxy(string url, string headersInUrlFormat, string proxyUrl, string proxyUser, string proxyPassword, bool nullWhenError)
        {

            WebProxy proxy;
            WebClient wc = new WebClient();
            if (!string.IsNullOrEmpty(proxyUser))
            {
                proxy = new WebProxy(proxyUrl, true, new string[] { }, new NetworkCredential(proxyUser, proxyPassword));
            }
            else
            {
                proxy = new WebProxy(proxyUrl);
            }
            if (proxy != null)
            {
                wc.Proxy = proxy;
            }
            return get(wc.AddHeaders(headersInUrlFormat), url, nullWhenError);
        }
        
        public class ParallelWebRequestUrlInput
        {
            [JsonProperty(PropertyName = "name")]
            public string UrlName { get; set; }

            [JsonProperty(PropertyName = "value")]
            public string UrlValue { get; set; }
        }
        public class ParallelWebRequestOutput
        {
            [JsonProperty(PropertyName = "name")]
            public string UrlName { get; set; }

            [JsonProperty(PropertyName = "status_code")]
            public int StatusCode { get; set; }

            [JsonProperty(PropertyName = "response_text")]
            public string ResponseText { get; set; }

            [JsonProperty(PropertyName = "is_success")]
            public bool IsSuccess { get; set; }


        }
        [SqlFunction]
        public static string GetParallel(string parallelWebRequestUrlInputJson, string headerJson, string cookieJson, bool nullWhenError)
        {
            try
            {
                ParallelWebRequestUrlInput[] urlArray = JsonConvert.DeserializeObject<ParallelWebRequestUrlInput[]>(parallelWebRequestUrlInputJson);
                var bag = new ConcurrentBag<ParallelWebRequestOutput>();
                List<Task<string>> taskList = new List<Task<string>>();
                Parallel.ForEach(urlArray, 
                    (x)=>
                    {
                        try
                        {
                            string url = x.UrlValue;
                            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                            wr.Method = "GET";
                            wr.KeepAlive = true;
                            wr.Timeout = 1000 * 60 * 20;
                            wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                            
                            wr.CookieContainer = new CookieContainer();
                            if (cookieJson != null)
                            {
                                Uri target = new Uri(url);
                                Dictionary<string, string> cookies = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieJson);
                                foreach (var cookie in cookies)
                                {
                                    wr.CookieContainer.Add(new Cookie(cookie.Key, HttpUtility.UrlEncode(cookie.Value), "/", target.Host));
                                }
                            }
                            if (headerJson != null)
                            {
                                var defaultHeaders = Enum.GetValues(typeof(HttpRequestHeader)).Cast<HttpRequestHeader>();
                                Dictionary<string, string> headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(headerJson);
                                foreach (var header in headers)
                                {
                                    var defaultHeader = defaultHeaders.Where(t => t.ToString().ToLower() == header.Key.ToLower());

                                    if (defaultHeader.Count() > 0)
                                    {
                                        if (header.Key.ToLower() == "accept")
                                        {
                                            wr.Accept = header.Value;
                                        }
                                        else
                                        {
                                            wr.Headers[defaultHeader.FirstOrDefault()] = header.Value;
                                        }
                                    }
                                    else
                                    {
                                        wr.Headers[header.Key] = header.Value;
                                    }
                                }
                            }
                            var res = (HttpWebResponse)wr.GetResponse();
                            using (var rs = res.GetResponseStream())
                            {
                                using (var sr = new StreamReader(rs))
                                {
                                    string responseText = sr.ReadToEnd();
                                    bag.Add(new ParallelWebRequestOutput()
                                    {
                                        IsSuccess = true,
                                        ResponseText = responseText,
                                        StatusCode = (int)res.StatusCode,
                                        UrlName = x.UrlName
                                    });
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            bag.Add(new ParallelWebRequestOutput()
                            {
                                IsSuccess = false,
                                ResponseText = e?.AddWebException()?.InnerException?.Message,
                                StatusCode = (e is WebException)?((int)((WebException)e).Status):0,
                                UrlName = x.UrlName
                            });
                        }
                    }

                );
                return JsonConvert.SerializeObject(bag);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }

        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable GetParallelWithProxy(string parallelWebRequestUrlInputJson, string headersInUrlFormat, string proxyUrl, string proxyUser, string proxyPassword, bool nullWhenError)
        {
            try
            {
                ParallelWebRequestUrlInput[] urlArray = JsonConvert.DeserializeObject<ParallelWebRequestUrlInput[]>(parallelWebRequestUrlInputJson);
                var bag = new ConcurrentBag<TableType>(new List<TableType>());
                Parallel.ForEach(urlArray, x => {
                    bag.Add((new TableType(x.UrlName, GetWithProxy(x.UrlValue, headersInUrlFormat, proxyUrl, proxyUser, proxyPassword, nullWhenError))));
                });
                return bag;

            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<IEnumerable>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static string Post(string url, string body,  string headersInUrlFormat, bool nullWhenError)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                FixSecurityProtocol();
                if (!string.IsNullOrWhiteSpace(headersInUrlFormat)) wc.Headers.Add(HttpUtility.ParseQueryString(headersInUrlFormat));
                return wc.UploadString(url, body);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e.AddWebException(), nullWhenError);
            }
        }
        public static string PostWithProxy(string url, string body, string headersInUrlFormat, string proxyUrl, string proxyUser, string proxyPassword, bool nullWhenError)
        {
            try
            {
                WebClient wc = new WebClient();
                if (!string.IsNullOrEmpty(proxyUser))
                {
                    wc.Proxy = new WebProxy(proxyUrl, true, new string[] { }, new NetworkCredential(proxyUser, proxyPassword));
                }
                else
                {
                    wc.Proxy = new WebProxy(proxyUrl);
                }
                wc.Encoding = Encoding.UTF8;
                FixSecurityProtocol();
                if (!string.IsNullOrWhiteSpace(headersInUrlFormat)) wc.Headers.Add(HttpUtility.ParseQueryString(headersInUrlFormat));
                return wc.UploadString(url, body);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e.AddWebException(), nullWhenError);
            }
        }


        public class WebRequestResult
        {

            public string Url { get; set; }
            public string Body { get; set; }
            public byte [] BodyByteArray { get; set; }
            public Dictionary<string, string> RequestHeaders { get; set; }
            public Dictionary<string, string> RequestCookies { get; set; }

            public string ResponseText { get; set; }
            public IEnumerable<string> ResponseRows { get; set; }
            private HttpStatusCode httpStatusCode;
            private bool isStatusCodeHttp = false;
            public HttpStatusCode HttpStatusCode
            {
                get
                {
                    return this.httpStatusCode;
                }
                set
                {
                    this.isStatusCodeHttp = true;
                    this.httpStatusCode = value;
                }
            }

            private FtpStatusCode ftpStatusCode;
            private bool isStatusCodeFtp = false;
            public FtpStatusCode FtpStatusCode
            {
                get
                {
                    return this.ftpStatusCode;
                }
                set
                {
                    this.isStatusCodeFtp = true;
                    this.ftpStatusCode = value;
                }
            }
            public int StatusCode
            {
                get
                {
                    if (this.isStatusCodeHttp)
                    {
                        return (int)this.HttpStatusCode;
                    }
                    if (this.isStatusCodeFtp)
                    {
                        return (int)this.FtpStatusCode;
                    }
                    else return 0;
                }
            }
            public WebHeaderCollection ResponseHeaders { get; set; }
            public CookieCollection ResponseCookies { get; set; }
            public int CodePage { get; set; }
            public WebException WebException { get; set; }
            public long? FileSize { get; set; }

        }
        public static WebRequestResult processFtpRequest(string url, string method, string fileName, string username, string password)
        {
            WebRequestResult result = new WebRequestResult();
            try
            {
                FtpWebRequest r = (FtpWebRequest)WebRequest.Create(url);
                if (username != null)
                {
                    r.Credentials = new NetworkCredential(username, password);
                }
                if (method == WebRequestMethods.Ftp.DownloadFile)
                {
                    r.UseBinary = true;
                    r.Method = method;
                    using (FtpWebResponse response = (FtpWebResponse)r.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                using (StreamWriter destination = new StreamWriter(fileName))
                                {
                                    destination.Write(reader.ReadToEnd());
                                    destination.Flush();
                                }
                            }
                        }
                        result.FtpStatusCode = response.StatusCode;
                    }
                    if (File.Exists(fileName))
                    {
                        result.FileSize = new FileInfo(fileName).Length;
                    }
                }
                if (method == WebRequestMethods.Ftp.Rename)
                {
                    r.UseBinary = true;
                    r.Method = method;
                    r.RenameTo = fileName;
                    using (var response = (FtpWebResponse)r.GetResponse())
                    {
                        result.FtpStatusCode = response.StatusCode;
                    }
                }
                if (method == WebRequestMethods.Ftp.UploadFile)
                {
                    r.Method = method;
                    using (var fileStream = File.OpenRead(fileName))
                    {
                        using (var requestStream = r.GetRequestStream())
                        {
                            fileStream.CopyTo(requestStream);
                        }
                    }
                    using (var response = (FtpWebResponse)r.GetResponse())
                    {
                        result.FtpStatusCode = response.StatusCode;
                    }
                }
                if (method == WebRequestMethods.Ftp.ListDirectory)
                {
                    r.Method = method;
                    using (var response = (FtpWebResponse)r.GetResponse())
                    {
                        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var rows = new List<string>();
                            string line = streamReader.ReadLine();
                            while (!string.IsNullOrEmpty(line))
                            {
                                rows.Add(line);
                                line = streamReader.ReadLine();
                            }
                            result.ResponseRows = rows;
                        }
                        result.FtpStatusCode = response.StatusCode;
                    }   
                }
                return result;
            }
            catch (WebException e)
            {
                var response = (FtpWebResponse)e.Response;
                result.WebException = e;
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        result.ResponseText = reader.ReadToEnd();
                        result.FtpStatusCode = response.StatusCode;
                    }
                }
                return result;
            }
        }
        public static WebRequestResult processWebRequest(string url, string method, string body, string contentType, int? codePage, Dictionary<string, string> headers, Dictionary<string, string> cookies, bool allowAutoRedirect, string fileName, string networkCredentialUser = null, string networkCredentialPassword = null)
        {
            FixSecurityProtocol();
            WebRequestResult result = new WebRequestResult();
            result.RequestCookies = cookies;
            result.RequestHeaders = headers;
            result.Url = url;
            result.Body = body;
            try
            {
                HttpWebRequest r = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrEmpty(networkCredentialUser)) {
                    r.Credentials = new NetworkCredential(networkCredentialUser, networkCredentialPassword);
                }
                r.Timeout = 1000 * 60 * 10;
                r.AllowAutoRedirect = allowAutoRedirect;
                r.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                r.Method = method;
                r.CookieContainer = new CookieContainer();
                if (cookies != null)
                {
                    Uri target = new Uri(url);
                    foreach (var cookie in cookies)
                    {
                        r.CookieContainer.Add(new Cookie(cookie.Key, HttpUtility.UrlEncode(cookie.Value), "/", target.Host));
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
                            if (header.Key.ToLower() == "accept")
                            {
                                r.Accept = header.Value;
                            }
                            else
                            {
                                r.Headers[defaultHeader.FirstOrDefault()] = header.Value;
                            }
                        }
                        else
                        {
                            r.Headers[header.Key] = header.Value;
                        }
                    }
                }
                int currentCodePage = (codePage.HasValue) ? codePage.Value : 65001; // Default Unicode;
                result.CodePage = currentCodePage;
                if (body != null)
                {
                    var encoding = Encoding.GetEncoding(currentCodePage);
                    byte[] bytes = encoding.GetBytes(body);
                    result.BodyByteArray = bytes;
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
                

                using (HttpWebResponse response = (HttpWebResponse)r.GetResponse())
                {
                    result.HttpStatusCode = response.StatusCode;
                    result.ResponseCookies = response.Cookies;
                    result.ResponseHeaders = response.Headers;
                    using (var s = response.GetResponseStream())
                    {
                        
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            System.IO.Directory.CreateDirectory(new FileInfo(fileName).Directory.FullName);
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }
                            using (FileStream os = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                byte[] buff = new byte[102400];
                                int c = 0;
                                if (s.CanRead)
                                {
                                    while ((c = s.Read(buff, 0, 10400)) > 0)
                                    {
                                        os.Write(buff, 0, c);
                                        os.Flush();
                                    }
                                }
                            }
                            if (File.Exists(fileName))
                            {
                                result.FileSize = new FileInfo(fileName).Length;
                            }
                        }
                        else
                        {
                            using (var reader = new StreamReader(s, Encoding.GetEncoding(currentCodePage)))
                            {
                                responseText = reader.ReadToEnd();
                                result.ResponseText = responseText;
                            }
                        }
                    }
                    return result;
                }
            }
            catch (WebException e)
            {
                result.WebException = e;
                var response = (HttpWebResponse)e.Response;
                if (response != null)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        result.ResponseCookies = response.Cookies;
                        using (var reader = new StreamReader(responseStream))
                        {
                            result.ResponseText = reader.ReadToEnd();
                            result.HttpStatusCode = response.StatusCode;
                        }
                    }
                }
                return result;
            }
        }
        // -----------------
        public class TableType
        {
            public string RowType { get; set; }
            public string RowKey { get; set; }
            public string RowValue { get; set; }
            public TableType()
            {

            }
            public TableType(string rowType, string rowValue)
            {
                this.RowType = rowType;
                this.RowValue = rowValue;
            }
            public TableType(string rowType, string rowKey, string rowValue)
            {
                this.RowType = rowType;
                this.RowKey = rowKey;
                this.RowValue = rowValue;
            }
        }
        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable ProcessFtpRequest(
            string url, 
            string method, 
            string fileName, 
            string username, 
            string password,
            bool nullWhenError
        )
        {
            try
            {
                var res = processFtpRequest(
                    url,
                    method,
                    fileName,
                    username,
                    password
                );
                
                List<TableType> l = new List<TableType>();
                l.Add(new TableType("url", url));
                l.Add(new TableType("method", method));
                l.Add(new TableType("body", res.Body));
                l.Add(new TableType("status_code", res.StatusCode.ToString()));
                l.Add(new TableType("response_text", res.ResponseText));
                l.Add(new TableType("file_size", res.FileSize.ToString()));
                if (res.ResponseRows != null)
                {
                    l.AddRange(res.ResponseRows.Select(x => new TableType("files", x)));
                }
                return l;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<IEnumerable>(e, nullWhenError);
            }
        }

        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable ProcessWebRequestWithNetworkCredentials(
            string url,
            string method,
            string body,
            string contentType,
            int? codePage,
            string headersInUrlFormat,
            string cookiesInUrlFormat,
            bool allowAutoRedirect,
            string fileName,
            string networkCredentialUser, 
            string networkCredentialPassword,
            bool nullWhenError
        )
        {
            try
            {
                Dictionary<string, string> headerDict = null;
                Dictionary<string, string> cookieDict = null;
                if (!string.IsNullOrWhiteSpace(headersInUrlFormat))
                {
                    var headers = HttpUtility.ParseQueryString(headersInUrlFormat);
                    headerDict = headers.AllKeys.ToDictionary(x => x, y => Uri.UnescapeDataString(headers[y]));
                }
                if (!string.IsNullOrWhiteSpace(cookiesInUrlFormat))
                {
                    var cookies = HttpUtility.ParseQueryString(cookiesInUrlFormat);
                    cookieDict = cookies.AllKeys.ToDictionary(x => x, y => Uri.UnescapeDataString(cookies[y]));
                }
                var res = processWebRequest(
                    url,
                    method,
                    body,
                    contentType,
                    codePage,
                    headerDict,
                    cookieDict,
                    allowAutoRedirect,
                    fileName,
                    networkCredentialUser,
                    networkCredentialPassword
                );
                
                if (res.WebException != null && !nullWhenError)
                {
                    throw res.WebException;
                }
                List<TableType> l = new List<TableType>();
                l.Add(new TableType("url", url));
                if (res.BodyByteArray != null)
                {
                    l.Add(new TableType("body_byte_array", JsonConvert.SerializeObject(res.BodyByteArray.Select(x => x.ToString()).ToArray())));
                }
                l.Add(new TableType("method", method));
                l.Add(new TableType("body", res.Body));
                l.Add(new TableType("content_type", contentType));
                l.Add(new TableType("code_page", res.CodePage.ToString()));
                l.Add(new TableType("status_code", res.StatusCode.ToString()));
                l.Add(new TableType("response_text", res.ResponseText));
                l.Add(new TableType("file_size", res.FileSize.ToString()));

                l.Add(new TableType("network_credential_user", networkCredentialUser));
                l.Add(new TableType("network_credential_password", networkCredentialPassword));
                if (res.RequestCookies != null)
                {
                    foreach (var cookie in res.RequestCookies)
                    {
                        l.Add(new TableType("request_cookie", cookie.Key, cookie.Value));
                    }
                }
                if (res.ResponseCookies != null)
                {
                    foreach (Cookie cookie in res.ResponseCookies)
                    {
                        l.Add(new TableType("response_cookie", cookie.Name, cookie.Value));
                    }
                }
                if (res.RequestHeaders != null)
                {
                    foreach (var header in res.RequestHeaders)
                    {
                        l.Add(new TableType("request_header", header.Key, header.Value));
                    }
                }
                if (res.ResponseHeaders != null)
                {
                    foreach (var header in res.ResponseHeaders.AllKeys)
                    {
                        l.Add(new TableType("response_header", header, res.ResponseHeaders[header]));
                    }
                }

                return l;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<IEnumerable>(e, nullWhenError);
            }
        }

        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable ProcessWebRequest(
            string url,
            string method,
            string body,
            string contentType,
            int? codePage,
            string headersInUrlFormat,
            string cookiesInUrlFormat,
            bool allowAutoRedirect,
            string fileName,
            bool nullWhenError
        )
        {
            return ProcessWebRequestWithNetworkCredentials(
                url,
                method,
                body,
                contentType,
                codePage,
                headersInUrlFormat,
                cookiesInUrlFormat,
                allowAutoRedirect,
                fileName,
                null,
                null,
                nullWhenError
            );
        }
        public static void FillRow(Object obj, out SqlChars rowType, out SqlChars key, out SqlChars value)
        {
            TableType table = (TableType)obj;
            rowType = new SqlChars(table.RowType);
            key = new SqlChars(table.RowKey);
            value = new SqlChars(table.RowValue);
        }
        // -----------------
    }
    public static class Extensions
    {
        public static Exception GetWebException(this Exception e)
        {
            if (e.GetType() == typeof(WebException))
            {
                var response = (HttpWebResponse)((WebException)e).Response;
                if (response != null)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                if (reader != null)
                                {
                                    var errorMessage = reader.ReadToEnd();
                                    e = new Exception(e.Message, new Exception(errorMessage));
                                }
                            }
                        }
                    }
                }
            }
            return e;
        }
    }
}
