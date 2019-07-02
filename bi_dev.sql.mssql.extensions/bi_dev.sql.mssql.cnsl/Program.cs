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
            var e = Utils.RegexMatch(@"6189514040__v3||6189514040||14186393770||смарт+прайс+отзывы||1||premium||none||search||no", @"\|((?:[0-9]{6}|[0-9]{8}|[0-9]{11}))\|", 1, false);
        }
    }
}
