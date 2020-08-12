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
                FileAttributes attr = File.GetAttributes(path);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    File.Delete(path);
                }
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static long? WriteTextToFile(string value, string fileName, bool nullWhenError)
        {
            try
            {
                File.WriteAllText(fileName, value, Encoding.UTF8);
                return new FileInfo(fileName).Length;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static long? WriteTextToFileNoBom(string value, string fileName, bool nullWhenError)
        {
            try
            {
                System.IO.Directory.CreateDirectory(new FileInfo(fileName).Directory.FullName);
                File.WriteAllText(fileName, value, new UTF8Encoding(false));
                return new FileInfo(fileName).Length;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
        public static bool CopyFile(string sourceFileName, string destFileName, bool overwrite, bool falseWhenError)
        {
            try
            {
                Directory.CreateDirectory(new FileInfo(destFileName).DirectoryName);
                File.Copy(sourceFileName, destFileName, overwrite);
                return true;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool>(e, falseWhenError, false);
            }
        }
    }
}
