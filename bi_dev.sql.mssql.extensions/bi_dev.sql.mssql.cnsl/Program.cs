
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
            Utils.G(@"C:\a.shamshur\290522_TTI_EUR_Unibank_card1.xls");
            var t = Utils.getContents(new ExcelRequest()
            {
                FileName = @"C:\a.shamshur\290522_TTI_EUR_Unibank_card1.xls",
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
            }, false);
        }
    }
}
