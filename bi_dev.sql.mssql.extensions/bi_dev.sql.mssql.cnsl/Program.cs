
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
            var body = "{ \"url\":\"https://tradernet.ru/portfolios/ajax-get-trades\",\"method\":\"POST\",\"body\":\"skip=0&take=1000&pageSize=500&sort[0][field]=date&sort[0][dir]=desc\",\"headers\":[{ \"name\":\"Content-Type\",\"value\":\"application/x-www-form-urlencoded; charset=UTF-8\"}],\"cookies\":[{ \"name\":\"SID\",\"value\":\"l3kl8gid16oshtjhulm032c4vk\"}]}";
            Utils.WebProcessRequest(body, true);
    }
    }
}
