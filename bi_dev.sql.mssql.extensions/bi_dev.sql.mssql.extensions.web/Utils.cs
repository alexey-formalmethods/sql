using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
                if (!string.IsNullOrWhiteSpace(headersInUrlFormat)) wc.Headers.Add(HttpUtility.ParseQueryString(headersInUrlFormat));
                return wc.UploadString(url, body);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
}
