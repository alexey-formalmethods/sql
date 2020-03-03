using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.@string.json
{
    public static class Utils
    {
        public static string JsonValue(string json, string path, bool nullWhenError)
        {
            try
            {
                JObject o = JObject.Parse(json);
                string result = (string)o.SelectToken(path);
                return result;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
}
