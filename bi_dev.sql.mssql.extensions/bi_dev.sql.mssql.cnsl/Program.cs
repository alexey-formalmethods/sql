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
            var e = Utils.processFtpRequest("ftp://ymservice.ru/statuses/20191118205223.xml", "RETR",
                @"C:\a.shamshur\public_projects\sql\bi_dev.sql.mssql.extensions\bi_dev.sql.mssql.extensions.string\kaka.xml",
                "kaka",
                "popa@"
            );
        }
    }
}
