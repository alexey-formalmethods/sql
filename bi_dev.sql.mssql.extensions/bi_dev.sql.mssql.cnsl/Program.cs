using bi_dev.sql.mssql.extensions.web;
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

            var e = Utils.ProcessWebRequest(
                "https://app.remonline.ru/login",
                "GET",
                null,
                null,
                null,
                null,
                null,
                false,
                false
            );
            


        }
    }
}
