using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.mongo
{
    public class Request
    {
        [JsonProperty("connection_string")]
        public string ConnectionString { get; set; }

        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("collection")]
        public string Collection { get; set; }

        [JsonProperty("filter")]
        public IDictionary Filter { get; set; }

        [JsonProperty("keys")]
        public IDictionary Keys { get; set; }

        //public IDictionary Projection { get; set; }
        public int Limit { get; set; } = 10000;
    }
    public class Response
    {
        public List<BsonDocument> Rows { get; set; } 
    }
    public static class Utils
    {
        public static Response Find(Request request)
        {
            var client = new MongoClient(request.ConnectionString);

            var collection = client.GetDatabase(request.Database).GetCollection<BsonDocument>(request.Collection);
            var fo = new FindOptions<BsonDocument>();
            if (request.Limit > 0) fo.Limit = request.Limit;
            fo.Projection = Newtonsoft.Json.JsonConvert.SerializeObject(request.Keys);
            var filterDoc = request.Filter != null ? new BsonDocument(request.Filter) : new BsonDocument();
            var rows = collection.FindSync(filterDoc, fo).ToList();
            return new Response()
            {
                Rows = rows
            };
        }
        public static string Find(string request, bool nullWhenError)
        {
            try
            {
                var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Request>(request);
                var res = Find(requestObj);
                JsonWriterSettings s = new JsonWriterSettings()
                {
                    OutputMode = JsonOutputMode.Shell
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(
                    new
                    {
                        rows = res.Rows.ConvertAll(BsonTypeMapper.MapToDotNetValue)
                    }
                );
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
}
