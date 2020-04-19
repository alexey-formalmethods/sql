using Microsoft.SqlServer.Server;
using Mono.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.@string.web
{
    public static class Utils
    {
        [SqlFunction]
        public static string GetQueryParamValue(string value, string paramName, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(paramName))
                {
                    return null;
                }
                else
                {
                    string result = HttpUtility.ParseQueryString(new Uri(value).Query).Get(paramName);
                    return result;
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
}
