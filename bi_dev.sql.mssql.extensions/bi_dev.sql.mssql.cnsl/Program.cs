
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using bi_dev.sql.mssql.extensions.aggregation.Utils;

namespace bi_dev.sql.mssql.cnsl
{
    class Program
    {
        static void Main(string[] args)
        {
            var e = new ToJsonArray();
            e.Values = new List<string>(){ "ab", "c" };
            var kaka = e.Terminate();
        }
    }
}
