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
            Utils.CreateExcelFile(fileName, "1", false);

            Utils.EditExcelFile(fileName, "1", 0, 0, "[{\"omg\": \"test\", \"number\": 1001}, {\"omg\": \"test - 2\", \"number\": 1002}, {\"omg\": \"test - 3\", \"number\": 133332}]", true);
            Utils.FormatExcelFile(fileName, "1", 0, 0, 0, null, 10, true, null, true, false, false, null, 22, true);
        }
    }
}
