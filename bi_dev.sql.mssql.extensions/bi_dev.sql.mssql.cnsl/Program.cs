
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
            var e = Utils.ProcessWebRequest(new WebRequestArgument()
            {
                Headers = new List<WebRequestHeader>()
                {
                    new WebRequestHeader() {Name = "authorization", Value = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyIsImtpZCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyJ9.eyJhdWQiOiJodHRwczovL2FuYWx5c2lzLndpbmRvd3MubmV0L3Bvd2VyYmkvYXBpIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMDRmNTM3NWUtNjViMy00ZTdhLTkzYTEtYmNjM2ViNDUyNmVmLyIsImlhdCI6MTY3Mzg2NjMzNiwibmJmIjoxNjczODY2MzM2LCJleHAiOjE2NzM4NzE4NDksImFjY3QiOjAsImFjciI6IjEiLCJhaW8iOiJBVFFBeS84VEFBQUEvM0hzcHYzTjBWOGhaQXhOZ0ptbUNXaXJrMHMxNVl3aGU4TTdkV0lEMllaWWZEdlpDNkdRQTcrTGVkQm51amF6IiwiYW1yIjpbInB3ZCJdLCJhcHBpZCI6Ijg3MWMwMTBmLTVlNjEtNGZiMS04M2FjLTk4NjEwYTdlOTExMCIsImFwcGlkYWNyIjoiMCIsImZhbWlseV9uYW1lIjoiQWRtaW4iLCJnaXZlbl9uYW1lIjoiVG9wVHJhZmZpYyIsImlwYWRkciI6Ijc5LjEzNy4xNzQuMjMxIiwibmFtZSI6IlRvcFRyYWZmaWMgQWRtaW4iLCJvaWQiOiJkOWY5NDY5NS1lNmNlLTQwMWQtOTFlMC1mZTJkOTY4OWY2NjYiLCJwdWlkIjoiMTAwMzIwMDE4OUFGRTc2NSIsInJoIjoiMC5BUXdBWGpmMUJMTmxlazZUb2J6RDYwVW03d2tBQUFBQUFBQUF3QUFBQUFBQUFBQ1dBTjAuIiwic2NwIjoidXNlcl9pbXBlcnNvbmF0aW9uIiwic2lnbmluX3N0YXRlIjpbImttc2kiXSwic3ViIjoiRkYtOXhNcjVxXzBwMWRIUHA5ZmR3VE15WnRLcWdXTlkxXzY3cHRaUzRPSSIsInRpZCI6IjA0ZjUzNzVlLTY1YjMtNGU3YS05M2ExLWJjYzNlYjQ1MjZlZiIsInVuaXF1ZV9uYW1lIjoidGVjaG5pY0B0b3B0cmFmZmljLnJ1IiwidXBuIjoidGVjaG5pY0B0b3B0cmFmZmljLnJ1IiwidXRpIjoiOWRneUluTDh5a0tidFRxSmsyMFBBQSIsInZlciI6IjEuMCIsIndpZHMiOlsiNjJlOTAzOTQtNjlmNS00MjM3LTkxOTAtMDEyMTc3MTQ1ZTEwIiwiYjc5ZmJmNGQtM2VmOS00Njg5LTgxNDMtNzZiMTk0ZTg1NTA5Il19.E3tB21nwq3T0mEY1HQ-zC-OfYK3da08kE5fqsMNKUKA8BGQv_-EIu-zjCF-Azswz17Cu_3xQNlYzehVg7iJwtSqUHDx3FVTG-V5T4P8KJAOv_XMENCzxBn3NcB--g_wcYwu_kpG0K6kyESYUOU5xQnXtJRVpqKGbZcd3WfalDWw68I22V1CINlIorqDoDJcOFU1WpqhMsBQ_Sqr_PfGXpBMUn4UIAltHBN_4PyMsp8d4It3xE1gcE4-40C54a8FekA0PujXEkzEWYS5qFSdipNqVR6SCOF-ywU_9b2MBgJZwtjCgUkGKXGQ6HHjEfh-H9d3o-OkBFYGja25_m9HuPg"}
                },
                Url = "https://wabi-west-europe-e-primary-redirect.analysis.windows.net/powerbi/content/packages/1564067/refresh/",
                Method = "POST"
            }, true, 0);
        }
    }
}
