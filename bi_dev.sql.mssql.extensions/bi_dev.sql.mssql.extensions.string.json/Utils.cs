using Newtonsoft.Json;
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
        public static string JsonSelectTokens(string json, string path, bool nullWhenError)
        {
            try
            {
                JToken o = JContainer.Parse(json);
                var parsedResult = o.SelectTokens(path);
                string result = null;
                result = JsonConvert.SerializeObject(parsedResult);
                return result;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string JsonMinify(string json, bool nullWhenError)
        {
            try
            {
                var deserializedJson = JsonConvert.DeserializeObject(json);
                return JsonConvert.SerializeObject(deserializedJson);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }

    }
}
