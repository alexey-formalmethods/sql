
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using bi_dev.sql.mssql.extensions.@string;

namespace bi_dev.sql.mssql.cnsl
{
    class Program
    {
        public static void Main()
        {
            var res = Utils.RegexMatches(@"[br] [silver] [] приобрели Taviche. Серебро. Сильный игрок.", @"\[(.*?)\]", false);
        }
    }
}
