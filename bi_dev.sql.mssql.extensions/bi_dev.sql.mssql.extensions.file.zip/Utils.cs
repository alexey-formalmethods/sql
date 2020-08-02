using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Microsoft.SqlServer.Server;
using System.IO;

namespace bi_dev.sql.mssql.extensions.file.zip
{
    public static class Utils
    {
        [SqlFunction]
        public static bool CreateZipFromDirectory(string directoryName, string destinationFileName, bool falseWhenError)
        {
            try
            {
                System.IO.Directory.CreateDirectory(new FileInfo(destinationFileName).Directory.FullName);
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
