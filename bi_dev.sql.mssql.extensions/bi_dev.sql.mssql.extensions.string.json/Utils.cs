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
        public static string JsonValue(string json, string path, bool nullWhenError)
        {
            try
            {
                JToken o = JContainer.Parse(json);
                var parsedResult = o.SelectTokens(path);
                string result = null;
                if (parsedResult.Count() > 1)
                {
                    result = JsonConvert.SerializeObject(parsedResult);
                }
                else
                {
                    var resultObject = parsedResult.FirstOrDefault();
                    if (resultObject.Type == JTokenType.Object)
                    {
                        result = JsonConvert.SerializeObject(resultObject);
                    }
                    if (resultObject.Type == JTokenType.Date) 
                    {
                        result = ((DateTime)resultObject).ToString("s", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        result = (string)resultObject;
                    }
                }
                
                return result;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }

    }
}
