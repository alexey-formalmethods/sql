using bi_dev.sql.mssql.extensions.file.excel;
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
            string fileName = @"C:\a.shamshur\kaka.xlsx";
            //Utils.CreateExcelFile(fileName, "1/2", false);
            
            Utils.Format(fileName, "1", 1, 1, 1, null, 12, true, false, true, false, false, 2, 22, false);
            Utils.Format(fileName, "1", 2, null, 1, null, 9, false, false, true, false, false, 1, null, false);
        }
    }
}
