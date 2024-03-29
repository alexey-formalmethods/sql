﻿using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public static DateTime? MinOfTwo(DateTime? value1, DateTime? value2, bool nullWhenError)
        {
            try
            {
                if (!value1.HasValue && !value2.HasValue) return null;
                else if (value1.HasValue && !value2.HasValue) return value1.Value;
                else if (!value1.HasValue && value2.HasValue) return value2.Value;
                else return ((value1.Value < value2.Value) ? value1.Value : value2.Value);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<DateTime?>(e, nullWhenError);
            }
        }
        public static DateTime? MaxOfTwo(DateTime? value1, DateTime? value2, bool nullWhenError)
        {
            try
            {
                if (!value1.HasValue && !value2.HasValue) return null;
                else if (value1.HasValue && !value2.HasValue) return value1.Value;
                else if (!value1.HasValue && value2.HasValue) return value2.Value;
                else return ((value1.Value > value2.Value) ? value1.Value : value2.Value);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<DateTime?>(e, nullWhenError);
            }
        }
        public static DateTime? FromString(string value, IEnumerable<string> formats)
        {
            DateTime result;
            foreach(var frmt in formats)
            {
                if (frmt.ToLower() == "oadate")
                {
                    double doubleDt;
                    if(double.TryParse(value, out doubleDt))
                    {
                        return DateTime.FromOADate(doubleDt);
                    }
                }
                if (DateTime.TryParseExact(value, frmt, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out result))
                {
                    return result;
                }
            }
            return null;
        }
        [SqlFunction]
        public static DateTime? FromString(string value, string formatsArray, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }
                IEnumerable<string> formats;
                if (!string.IsNullOrWhiteSpace(formatsArray))
                {
                    formats = JsonConvert.DeserializeObject<IEnumerable<string>>(formatsArray);
                }
                else
                {
                    formats = new string[]
                    {
                        "yyyy-MM-dd",
                        "MM/dd/yyyy",
                        "dd.MM.yyyy",
                        "yyyy-MM-ddTHH:mm:ss",
                        "yyyy-MM-dd HH:mm:ss"
                    };
                }
                return FromString(value, formats);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<DateTime?>(e, nullWhenError);
            }
        }
    }
}
