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
        public static bool? Exists(string path, bool nullWhenError)
        {
            try
            {
                return (string.IsNullOrWhiteSpace(path))?false: (File.Exists(path) || Directory.Exists(path));
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static bool? Delete(string path, bool nullWhenError)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static long? WriteTextToFile(string value, string path, bool nullWhenError)
        {
            try
            {
                File.WriteAllText(path, value, Encoding.UTF8);
                return new FileInfo(path).Length;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
    }
}
