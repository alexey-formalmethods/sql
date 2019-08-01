using bi_dev.sql.mssql.extensions.date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.cnsl
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var e = Utils.UnixTimestampToDate(1564599670000, true, false);
        }
    }
}
