using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.date
{
    public static class Utils
    {
        public static long? LocalDateToUnixTimestamp(DateTime? value, bool includeMiliSeconds, bool nullWhenError)
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
        public static long? DateToUnixTimestamp(DateTime? value, bool includeMiliSeconds, bool nullWhenError)
        {
            try
            {
                if (!value.HasValue) return null;
                else
                {
                    DateTime? valueWithoutTimeZone = value.Value.ToUniversalTime() + new DateTimeOffset(value.Value).Offset;
                    return LocalDateToUnixTimestamp(valueWithoutTimeZone, includeMiliSeconds, nullWhenError);
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
                else return (includeMiliSeconds)? DateTimeOffset.FromUnixTimeMilliseconds(value.Value).DateTime:DateTimeOffset.FromUnixTimeSeconds(value.Value).DateTime;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<DateTime?>(e, nullWhenError);
            }
        }
        public static DateTime? UnixTimestampToLocalDate(long? value, bool includeMiliSeconds, bool nullWhenError)
        {
            return UnixTimestampToDate(value, includeMiliSeconds, nullWhenError)?.ToLocalTime();
        }
        public static int? GetWeekDayNumRus(DateTime? value, bool nullWhenError)
        {
            try
            {
                if (!value.HasValue) return null;
                else return (value.Value.DayOfWeek==DayOfWeek.Sunday)?7:(int)(value.Value.DayOfWeek);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<int?>(e, nullWhenError);
            }
        }
        public static DateTime? UtcDateToLocalDate(DateTime? value, bool nullWhenError)
        {
            try
            {
                if (!value.HasValue) return null;
                else return value.Value.ToLocalTime();
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<DateTime?>(e, nullWhenError);
            }
        }
    }
}
