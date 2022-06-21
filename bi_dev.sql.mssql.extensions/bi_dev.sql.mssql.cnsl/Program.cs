
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
            var t = Utils.getContents(new ExcelRequest()
            {
                FileName = @"C:\a.shamshur\240422_MTT_rur_Alfa.xlsx",
                SheetRequests = new List<ExcelSheetRequest>()
                {
                    new ExcelSheetRequest()
                    {
                        Range = "C1:C1"
                    },
                    new ExcelSheetRequest()
                    {
                        Range = "C7:C7"
                    }
                }
            }, true);
        }
    }
}
