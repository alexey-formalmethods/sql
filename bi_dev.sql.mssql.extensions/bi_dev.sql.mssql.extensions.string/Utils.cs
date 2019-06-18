using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bi_dev.sql.mssql.extensions;


namespace bi_dev.sql.mssql.extensions.@string
{
    public static class Utils
    {
        [SqlFunction]
        public static long? RemoveNonDigits(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return long.Parse(new string(value.Where(c => char.IsDigit(c)).ToArray()));
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static string UrlDecode(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Uri.UnescapeDataString(value);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static string SplitString(string value, string separator, int index, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return null;
                else return value.Split(new string[] { separator }, StringSplitOptions.None)[index];
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static string UnicodeDecode(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return System.Text.RegularExpressions.Regex.Unescape(value);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
}
