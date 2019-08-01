using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.date
{
    public static class Utils
    {
        public static long DateToUnixTimestamp(DateTime value, bool includeMiliSeconds, bool nullWhenError)
        {
            var dtValue = ((DateTimeOffset)value);
            return (includeMiliSeconds)? dtValue.ToUnixTimeMilliseconds():dtValue.ToUnixTimeSeconds();
        }
        public static DateTime UnixTimestampToDate(long value, bool includeMiliSeconds, bool nullWhenError)
        {
            return (includeMiliSeconds)? DateTimeOffset.FromUnixTimeMilliseconds(value).DateTime:DateTimeOffset.FromUnixTimeSeconds(value).DateTime;
        }
    }
}
