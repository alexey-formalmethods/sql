using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.@string.csv
{
    public static class Constants
    {
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    }
    public static class Utils
    {
        private static List<string[]> parseCsv(string value, string delimiter)
        {
            List<string[]> result = new List<string[]>();
            using (TextReader reader = new StringReader(value))
            {

                CsvParser csv = new CsvParser(
                    reader, 
                    new CsvConfiguration(CultureInfo.InvariantCulture) 
                    { 
                        Delimiter = string.IsNullOrEmpty(delimiter) ? ";" : delimiter 
                    }
                );
                while (csv.Read())
                {
                    result.Add(csv.Record);
                }
                
            }
            return result;
        }

        private static CultureInfo CsvConfiguration(CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
        }

        public static void FillRow(Object obj, out SqlInt32 rowType, out SqlInt32 key, out SqlChars value)
        {
            TableType.FillRow(obj, out rowType, out key,out value);
        }
        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable ParseCsv(string value, string delimiter, bool nullWhenError)
        {
            try
            {
                List<TableType> l = new List<TableType>();
                var result = parseCsv(value, delimiter);
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < result[i].Length; j++)
                    {
                        l.Add(new TableType(i, j, result[i][j]));
                    }
                }
                return l;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<IEnumerable>(e, nullWhenError);
            }
        }
        public static string JsonToCsv(string jsonObject, string delimiter, string dateTimeFormat, bool nullWhenError)
        {
            try
            {
                List<JObject> values = JsonConvert.DeserializeObject<List<JObject>>(jsonObject);
                string result = "";
                using (TextWriter tw = new StringWriter())
                {
                    using (CsvWriter cw = new CsvWriter(tw, new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = string.IsNullOrEmpty(delimiter) ? ";" : delimiter }))
                    {
                        dateTimeFormat = string.IsNullOrWhiteSpace(dateTimeFormat) ? Constants.DateTimeFormat : dateTimeFormat;
                        for (int i = 0; i < values.Count; i++)
                        {
                            List<JProperty> properties = values[i].Properties().ToList();
                            for (int j = 0; j < properties.Count; j++)
                            {
                                var property = properties[j];
                                string valueString;
                                if (property.Value.Type == JTokenType.Date)
                                {
                                    valueString = ((DateTime)property.Value).ToString(dateTimeFormat, CultureInfo.CurrentCulture);
                                }
                                else
                                {
                                    valueString = property.Value.ToString();
                                }
                                cw.WriteField(valueString);
                            }
                            cw.NextRecord();
                        }
                    }
                    result = tw.ToString();

                }
                return result;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        private static CsvReadResult getCsvContent(string value, string delimiter, bool isFirstRowWithColumnNames)
        {
            var result = parseCsv(value, delimiter);
            CsvReadResult csvResult = new CsvReadResult();
            if (result != null && result.Count > 0)
            {
                if (isFirstRowWithColumnNames)
                {
                    csvResult.Columns.AddRange(result[0]);
                }
                csvResult.Values.AddRange(result.Where((x, i) => i > 0).ToList());
            }
            return csvResult;
        }
        [SqlFunction]
        public static string GetCsvContent(string value, string delimiter, bool isFirstRowWithColumnNames, bool nullWhenError)
        {
            try
            {
                return JsonConvert.SerializeObject(getCsvContent(value, delimiter, isFirstRowWithColumnNames));
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
    public class CsvReadResult
    {
        [JsonProperty(PropertyName = "columns")]
        public List<string> Columns { get; set; }

        [JsonProperty(PropertyName = "values")]
        public List<string[]> Values { get; set; }

        public CsvReadResult()
        {
            this.Columns = new List<string>();
            this.Values = new List<string[]>();
        }
    }
}
