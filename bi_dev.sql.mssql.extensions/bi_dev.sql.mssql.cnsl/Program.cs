
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace bi_dev.sql.mssql.cnsl
{
    class Program
    {
        public static void Main()
        {
            var e = bi_dev.sql.mssql.extensions.web2.Utils.ProcessWebRequest(new extensions.web2.WebRequestArgument()
            {
                Url = "https://api.ipify.org?format=json",
                Proxy = new extensions.web2.WebRequestArgumentProxy()
                {
                    Address = "zproxy.lum-superproxy.io",
                    Port = 22225,
                    Username = "lum-customer-c_26959815-zone-residential-country-ru",
                    Password = "ayp5cs0n19cr"
                }
            }, true, 0);
        }
    }
}
