using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.file
{
    public class Utils
    {
        [SqlFunction]
        public static string GetFileContent(string fileName, bool nullWhenError)
        {
            try
            {
                return File.ReadAllText(fileName);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static bool Exists(string path, bool nullWhenError)
        {
            try
            {
                return File.Exists(path) || Directory.Exists(path);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool>(e, nullWhenError);
            }
        }
    }
}
