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
        public static bool CreateZipFromDirectory(string directoryName, string destinationFileName, bool falseWhenError)
        {
            try
            {
                ZipFile.CreateFromDirectory(directoryName, destinationFileName);
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool>(e, falseWhenError, false);
            }
            
            
        }
    }
}
