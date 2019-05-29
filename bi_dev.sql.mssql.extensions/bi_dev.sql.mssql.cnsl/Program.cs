using bi_dev.sql.mssql.extensions.@string;
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
            var e = Utils.UrlDecode("v2%7C%7C6189514023%7C%7C14186394606%7C%7Csmartprice%7C%7C1%7C%7Cpremium%7C%7Cnone%7C%7Csearch%7C%7Cno__v3%7C%7C6189514023%7C%7C14186394606%7C%7Csmartprice%7C%7C1%7C%7Cpremium%7C%7Cnone%7C%7Csearch%7C%7Cno", false);
            var kaka = Utils.SplitString("opop: 1: 2: 3", ":", 3, true);
            var kaka2 = Utils.SplitString("opop: 1: 2: 3", ":", 44, true);
            var kaka3 = Utils.SplitString("", "", 44, true);
        }
    }
}
