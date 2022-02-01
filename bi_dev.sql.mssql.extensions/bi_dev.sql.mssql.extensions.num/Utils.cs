using Microsoft.SqlServer.Server;
using NickBuhro.NumToWords.Russian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.num
{
    public static class Utils
    {
        [SqlFunction]
        public static string ToRub(long? value, bool nullWhenError)
        {
            try
            {
                if (!value.HasValue)
                {
                    return null;
                }
                else
                {
                    return RussianConverter.Format(value.Value, UnitOfMeasure.Ruble);
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
}
