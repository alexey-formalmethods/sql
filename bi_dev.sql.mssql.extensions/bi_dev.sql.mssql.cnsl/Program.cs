
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bi_dev.sql.mssql.cnsl
{
    class Program
    {
        public static void Main()
        {
            bi_dev.sql.mssql.extensions.aggregation.Utils.Median m = new extensions.aggregation.Utils.Median();
            m.Init();
            m.Accumulate(new SqlDouble(), true);
            m.Accumulate(100, true);
            m.Accumulate(100, true);
            m.Accumulate(100, true);
            var e = m.Terminate();
        }
    }
}
