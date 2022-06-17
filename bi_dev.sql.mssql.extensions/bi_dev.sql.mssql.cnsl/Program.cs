
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
            var k = Utils.GetSheets(@"C:\a.shamshur\240422_MTT_rur_Tochka.xls");
            var e = Utils.GetContent(new ExcelArgument()
            {
                FileName = @"C:\a.shamshur\240422_MTT_rur_Tochka.xls",
                RangeFrom = "B2",
                RangeTo = "E4",
                IsFirstRowWithColumnNames = true,
                SheetNum = 1
            });
        }
    }
}
