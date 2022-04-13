
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bi_dev.sql.mssql.extensions.web2;
using Newtonsoft.Json;

namespace bi_dev.sql.mssql.cnsl
{
    class Program
    {
        public static void Main()
        {
            var e = Utils.ProcessWebRequest(
                new WebRequestArgument()
                {
                    Url = "https://query1.finance.yahoo.com/v1/finance/quote?symbols=EFO&fields=exchangeTimezoneName,exchangeTimezoneShortName,regularMarketTime,gmtOffSetMilliseconds&region=US&lang=en-US"
                    ,Attempts = 3,
                    MillisecondsToRetry = 1000
                },
                false
            );
        }
    }
}
