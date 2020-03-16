using CsvHelper;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.@string.csv
{
    public static class Utils
    {
        private static List<string[]> parseCsv(string value, string delimiter)
        {
            List<string[]> result = new List<string[]>();
            using (TextReader reader = new StringReader(value))
            {
                CsvParser csv = new CsvParser(reader,CultureInfo.CurrentCulture);
                csv.Configuration.Delimiter = (string.IsNullOrEmpty(delimiter) ? ";" : delimiter);
                while (true)
                {
                    var row = csv.Read();
                    if (row == null)
                    {
                        break;
                    }
                    else
                    {
                        result.Add(row);
                    }
                }
            }
            return result;
        }
        public static void FillRow(Object obj, out SqlInt32 rowType, out SqlInt32 key, out SqlChars value)
        {
            TableType.FillRow(obj, out rowType, out key,out value);
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
