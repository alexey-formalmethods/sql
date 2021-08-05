
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
            string j = "{\"url\":\"https://query2.finance.yahoo.com/v10/finance/quoteSummary/NFLX\",\"method\":\"POST\",\"cookies\":[{\"name\":\"Y\", \"value\":\"v=1&n=6ntkmjf13gfbb&l=s9bv7ax9e5ec5hdhdcnn7sjo95ogt5bn24bgh5ix/o&p=o2svvru00000000&r=18m&intl=ru\"}]}";
            var e = Utils.WebProcessRequest(j, false);
        }
    }
}
