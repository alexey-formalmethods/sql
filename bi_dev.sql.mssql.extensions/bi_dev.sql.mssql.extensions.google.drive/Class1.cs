using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.google.drive
{
    public static class Utils
    {
        public static bool GetFile(string accessToken, string spreadsheetId, string fileName, bool falseWhenError)
        {
            try
            {
                using (DriveService ds = new DriveService())
                {
                    using (var fs = new FileStream(fileName, FileMode.Create)) 
                    {
                        ds.Files.Export(spreadsheetId, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet").Download(fs);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return Common.ThrowIfNeeded<bool>(ex, false);
            }
        }
    }
}
