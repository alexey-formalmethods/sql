using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Microsoft.SqlServer.Server;

namespace bi_dev.sql.mssql.extensions.file.zip
{
    public static class Utils
    {
        [SqlFunction]
        public static bool CreateZipFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, bool falseWhenError)
        {
            try
            {
                ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName);
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool>(e, falseWhenError, false);
            }
            
            
        }
    }
}
