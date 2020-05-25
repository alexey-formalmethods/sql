//using bi_dev.sql.mssql.extensions.web.google;
//using bi_dev.sql.mssql.extensions.web;
using bi_dev.sql.mssql.extensions.web.google.adwords;
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
            
            if (args.Length > 0) {
                // google sheets authorise via browser
                if (args[0] == "gs")
                {   
                    if (args.Length < 2)
                    {
                        throw new Exception("arguments after [0] ds: [1] credentialsPath");
                    }
                    string credentialsPath = args[1];
                    bi_dev.sql.mssql.extensions.web.google.Auth.Do(credentialsPath, bi_dev.sql.mssql.extensions.web.google.sheet.Constants.Scopes);
                } 
            }
        }
    }
}
