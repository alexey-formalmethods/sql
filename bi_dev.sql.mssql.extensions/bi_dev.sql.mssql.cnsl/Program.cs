
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
            string j = "{\"url\":\"https://ya.ru\",\"method\":\"GET\"}";
            var e = Utils.WebProcessRequest(j, false);
        }
    }
}
