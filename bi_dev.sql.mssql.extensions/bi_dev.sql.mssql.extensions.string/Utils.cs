using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bi_dev.sql.mssql.extensions;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Security.Cryptography;
using System.IO;
using CsvHelper;
using System.Data.SqlTypes;
using System.Collections;

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
        public static string UrlEncode(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Uri.EscapeDataString(value);
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
        [SqlFunction]
        public static string RegexMatch(string value, string regexPattern, int groupNumber, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else
                {
                    return new Regex(regexPattern).Match(value).Groups[groupNumber].Value;
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string StringExceptHTML(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }
                else
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(value);
                    return htmlDoc.DocumentNode.InnerText;
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string GetSha256Hash(string value, bool nullWhenError)
        {
            try
            {
                SHA256Managed sha256 = new SHA256Managed();
                Encoding encoding = Encoding.UTF8;
                byte[] inputByteArray = encoding.GetBytes(value);
                byte[] hashByteArray = sha256.ComputeHash(inputByteArray);
                string hashString = string.Empty;
                foreach (byte x in hashByteArray)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                return hashString;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public class TableType
        {
            public int RowNumber { get; set; }
            public int ColumnIndex { get; set; }
            public string Value { get; set; }
            public TableType()
            {

            }
            public TableType(int rowNumber, int columnIndex, string value)
            {
                this.RowNumber = rowNumber;
                this.ColumnIndex = columnIndex;
                this.Value = value;
            }
        }
        public static void FillRow(Object obj, out SqlInt32 rowType, out SqlInt32 key, out SqlChars value)
        {
            TableType table = (TableType)obj;
            rowType = new SqlInt32(table.RowNumber);
            key = new SqlInt32(table.ColumnIndex);
            value = new SqlChars(table.Value);
        }
        private static List<string[] > parseCsv(string value, string delimiter)
        {
            List<string[]> result = new List<string[]>();
            using (TextReader reader = new StringReader(value))
            {
                CsvParser csv = new CsvParser(reader);
                csv.Configuration.Delimiter = (string.IsNullOrEmpty(delimiter)?";": delimiter);
                while (true)
                {
                    var row = csv.Read();
                    result.Add(row);
                    if (row == null)
                    {
                        break;
                    }
                }
            }
            return result;
        }
        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable ParseCsv(string value, string delimiter, bool nullWhenError)
        {
            try
            {
                List<TableType> l = new List<TableType>();
                var result = parseCsv(value, delimiter);
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < result[i].Length; j++)
                    {
                        l.Add(new TableType(i, j, result[i][j]));
                    }
                }
                return l;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<IEnumerable>(e, nullWhenError);
            }
        }
    }
}
