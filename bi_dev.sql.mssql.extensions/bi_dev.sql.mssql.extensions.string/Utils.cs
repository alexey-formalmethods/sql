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
    }
}
