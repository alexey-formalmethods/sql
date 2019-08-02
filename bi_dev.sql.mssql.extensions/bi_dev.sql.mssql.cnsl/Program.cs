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
                false
            );
            string sessionId = e.Cookies["sessionid"].Value;
            string token = e.Cookies["csrftoken"].Value;
            string __cfduid = e.Cookies["__cfduid"].Value;
            var headers = new Dictionary<string, string>();
            Dictionary<string, string> cookies = new Dictionary<string, string>();
            
            cookies.Add("csrftoken", token);
            cookies.Add("sessionid", sessionId);
            //cookies.Add("__cfduid", __cfduid);
            headers["X-CSRFToken"] = token;
            //var uu = Utils.kaka(token, sessionId);
            var kaka = Utils.ProcessWebRequest(
                "https://app.remonline.ru/do-login",
                "POST",
                "login=vt-sp&password=vt1234567",
                "application/x-www-form-urlencoded",
                65001,
                headers,
                cookies,
                false
            );
            
            
            var pipka = Utils.ProcessWebRequest(
                "https://app.remonline.ru/orders/json/get-timeline/15268162?__meta%5Bbranch%5D=29045",
                "GET",
                null,
                null,
                65001,
                null,
                cookies,
                false
            );
            


        }
    }
}
