using bi_dev.sql.mssql.extensions.@string.csv;
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
            string e = Utils.JsonToCsv("[{\"kaka\":\"2020-02-01T00:00:01\", \"pipka\": \"jopa\"},{\"kaka\":\"2020-02-01T00:00:01\", \"pipka\": \"jopa2\"}]", ": ", "yyyy-MM-dd", false);
            Console.WriteLine(e);
        }
    }
}
