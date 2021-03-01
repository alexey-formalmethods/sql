using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bi_dev.sql.mssql.extensions.web.google.sheet;
using CommandLine;
using CommandLine.Text;

namespace bi_dev.sql.mssql.extensions.web.google.cnsl
{

    class Program
    {
        static void Main(string[] args)
        {
            var e = Utils.GetRange(
                "ya29.A0AfH6SMAjxEVBGbDwVD-1NVtAt4rLyv6TJNgYkg8WmJOWSSRC8TuSwz8z1x5Xvarbka0LIZ4GOv6fxcVVGXagrNy8SrVJ-rWPzYCkTwfWI4PDxxRZmxLuHgl1R1RGH9KmUFeRm2LXhMA1DGP_fWfZRbdgCdUm-Q",
                "1hsaZwTwSpF0P4qrwnY3-e5jdqWvJcS0sAKd1FO-k5mc",
                "tst",
                null,
                false);
            
        }
       
    }
}
