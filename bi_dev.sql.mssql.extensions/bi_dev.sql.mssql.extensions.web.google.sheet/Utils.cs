using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Globalization;

namespace bi_dev.sql.mssql.extensions.web.google.sheet
{
    public class Constants
    {
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DateFormat = "yyyy-MM-dd";
        public const string TimeFormat = "HH:mm:ss";
    }
    public class SimpleObject
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        public SimpleObject(string value)
        {
            this.Value = value;
        }
    }
    public static class JPropertyExtensions
    {
        public static object GetValue(this JProperty property)
        {
            JToken value = property?.Value;
            object result;
            if (property == null || value == null)
            {
                result = null;
            }
            else
            {
                var valueType = value.Type;
                switch (valueType)
                {
                    case JTokenType.Date:
                        {
                            string format = ((DateTime)value).TimeOfDay.CompareTo(new TimeSpan(0))==0?Constants.DateFormat:Constants.DateTimeFormat;
                            result = ((DateTime)value).ToString(format, CultureInfo.CurrentCulture);
                            break;
                        }
                    case JTokenType.Float:
                        {
                            result = (double)value;
                            break;
                        }
                    case JTokenType.Boolean:
                        {
                            result = (bool)value;
                            break;
                        }
                    case JTokenType.Integer:
                        {
                            result = (long)value;
                            break;
                        }
                    case JTokenType.String:
                        {
                            result = (string)value;
                            break;
                        }
                    case JTokenType.Null:
                        {
                            result = null;
                            break;
                        }
                    default:
                        {
                            result = JsonConvert.SerializeObject(value);
                            break;
                        }
                }
            }
            return result;
        }
    }
    public static class Utils
    {
        private static SheetsService service;
        private static SheetsService GetService(string credentialsPath)
        {
            
            if (service == null)
            {
                UserCredential credential;
                string[] Scopes = { SheetsService.Scope.Spreadsheets };
                string ApplicationName = "bi_dev.mssql.google.sheets";
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = new FileInfo(credentialsPath).DirectoryName + "\\token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }
                // Create Google Sheets API service.
                service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            return service;
        }
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        
        public static bool? ClearRange(string credentialsJsonPath, string spreadsheetId, string sheetName, string range, bool falseWhenError)
        {
            try
            {
                var service = GetService(credentialsJsonPath);
                string currentRange = sheetName + (!string.IsNullOrWhiteSpace(range) ? "!" + range : "");
                SpreadsheetsResource.ValuesResource.ClearRequest cleaRequest = service.Spreadsheets.Values.Clear(
                        new ClearValuesRequest(), spreadsheetId, currentRange
                );
                var result = cleaRequest.Execute();
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, falseWhenError, false);
            }
        }

        public static bool? UpdateRange(string credentialsJsonPath, string spreadsheetId, string sheetName, string rangeFrom, string jsonObject, bool includeColumnNames, bool cleanBeforeUpdate, bool falseWhenError)
        {
            try
            {
                List<JObject> values;
                try
                {
                    values = JsonConvert.DeserializeObject<List<JObject>>(jsonObject);
                }
                catch (JsonSerializationException)
                {
                    values = new List<JObject>() { JsonConvert.DeserializeObject<JObject>(jsonObject) };
                }
                catch (JsonReaderException)
                {
                    values = new List<JObject>() {
                            JsonConvert.DeserializeObject<JObject>(
                                JsonConvert.SerializeObject(
                                    new SimpleObject(jsonObject)
                                )
                            )
                        };
                }
                string fullSheetRange = $"{sheetName}";

                string currentRange = $"{sheetName}!{rangeFrom}";

                IList<IList<object>> l = new List<IList<object>>();
                if (values.Count > 0)
                {
                    if (includeColumnNames)
                    {
                        List<JProperty> columnNames = values.FirstOrDefault().Properties().ToList();
                        IList<object> columnNamesStringList = columnNames.Select(x => (object)x.Name).ToList();
                        l.Add(columnNamesStringList);
                    }
                    for (int i = 0; i < values.Count; i++)
                    {
                        List<JProperty> properties = values[i].Properties().ToList();
                        IList<object> rangeValues = properties.Select(x => x.GetValue()).ToList();
                        l.Add(rangeValues);
                    }
                }
                ValueRange r = new ValueRange()
                {
                    Range = rangeFrom,
                    Values = l
                };
                var service = GetService(credentialsJsonPath);
                if (cleanBeforeUpdate)
                {
                    ClearRange(credentialsJsonPath, spreadsheetId, sheetName, null, false);
                }
                SpreadsheetsResource.ValuesResource.UpdateRequest request = service.Spreadsheets.Values.Update(
                    r, spreadsheetId, rangeFrom
                );

                //request.ResponseDateTimeRenderOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ResponseDateTimeRenderOptionEnum.SERIALNUMBER;
                request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                request.Execute();
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, falseWhenError, false);
            }
        }
    }
}
