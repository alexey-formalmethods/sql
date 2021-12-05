
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bi_dev.sql.mssql.extensions.file.excel;
using Newtonsoft.Json;

namespace bi_dev.sql.mssql.cnsl
{
    class Program
    {
        public static void Main()
        {
            Utils.GetExcelContent(
                @"C:\a.shamshur\Robocash - for publication.xlsx",
                "2021_08_04_Debex_list",
                null,
                null,
                null,
                null,
                true,
                false
            );
        }
    }
}
