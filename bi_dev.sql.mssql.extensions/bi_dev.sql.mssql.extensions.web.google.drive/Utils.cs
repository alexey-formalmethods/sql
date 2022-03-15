using bi_dev.sql.mssql.extensions.web.google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.web.google.drive
{
    public class GoogleDriveFilesResult
    {
        [JsonProperty("files")]
        public IEnumerable<GoogleDriveFile> Files { get; set; }
    }
    public class GoogleDriveFile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parents")]
        public ICollection<string> Parents { get; set; }

        [JsonProperty("created_time")]
        public DateTime? CreatedTime { get; set; }

        [JsonProperty("modified_time")]
        public DateTime? ModifiedTime { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("is_directory")]
        public bool IsDirectory { get; set; }
    }
    public static class Utils
    {
        private static DriveService GetServiceFromAccessToken(string accessToken)
        {

            string[] scopes = { DriveService.Scope.DriveReadonly };
            string ApplicationName = "bi_dev.mssql.google.drive";
            GoogleCredential credential = Auth.GetUserCredential(accessToken);
            // Create Google Sheets API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
        public static IEnumerable<GoogleDriveFile> getFiles(string accessToken)
        {
            List<GoogleDriveFile> result = new List<GoogleDriveFile>();
            using (DriveService ds = GetServiceFromAccessToken(accessToken))
            {
                FilesResource.ListRequest listRequest = ds.Files.List();
                listRequest.PageSize = 100;
                listRequest.Fields = "*";
                listRequest.OrderBy = "modifiedTime";
                for (int i = 0; i < 100; i++) // 
                {
                    var response = listRequest.Execute();
                    if (response?.Files?.Count > 0)
                    {
                        result.AddRange(response.Files.Select(x => new GoogleDriveFile()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Parents = x.Parents,
                            CreatedTime = x.CreatedTime,
                            ModifiedTime = x.ModifiedTime,
                            MimeType = x.MimeType
                        }));
                        listRequest.PageToken = response.NextPageToken;
                    }
                    else { break; }
                    if (response?.NextPageToken == null) { break; }
                }
            }
            return result;
        }
        public static void downloadFile(string accessToken, string fileId, string fileName)
        {
            using (DriveService ds = GetServiceFromAccessToken(accessToken))
            {
                FilesResource.GetRequest fileRequest = ds.Files.Get(fileId);
                using (FileStream file = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    fileRequest.Download(file);
                }
            }
        }
        [SqlFunction]
        public static string GetFiles(string accessToken, bool nullWhenError)
        {
            try
            {
                return JsonConvert.SerializeObject(getFiles(accessToken));
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static bool DownloadFile(string accessToken, string fileId, string fileName, bool falseWhenError)
        {
            try
            {
                downloadFile(accessToken, fileId, fileName);
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool>(e, falseWhenError, false);
            }
        }
    }
}
