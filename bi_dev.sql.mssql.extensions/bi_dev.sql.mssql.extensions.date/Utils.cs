using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.date
{
    public static class Utils
    {
        public static long? DateToUnixTimestamp(DateTime? value, bool includeMiliSeconds, bool nullWhenError)
        {
            try
            {
                if (!value.HasValue) return null;
                else
                {
                    var dtValue = ((DateTimeOffset)value.Value);
                    return (includeMiliSeconds) ? dtValue.ToUnixTimeMilliseconds() : dtValue.ToUnixTimeSeconds();
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
        public static DateTime? UnixTimestampToDate(long? value, bool includeMiliSeconds, bool nullWhenError)
        {
            try
            {
                if (!value.HasValue) return null;
                else return (includeMiliSeconds)? DateTimeOffset.FromUnixTimeMilliseconds(value.Value).DateTime:DateTimeOffset.FromUnixTimeSeconds(value).DateTime;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<DateTime?>(e, nullWhenError);
            }
        }
    }
}
