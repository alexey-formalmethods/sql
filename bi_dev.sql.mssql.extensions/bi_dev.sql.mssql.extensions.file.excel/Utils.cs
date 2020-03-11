using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Microsoft.SqlServer.Server;

namespace bi_dev.sql.mssql.extensions.file.excel
{
    public static class Utils
    {
        [SqlFunction]
        public static long? CreateExcelFile(string fileName, string sheetNamesSlashDelimited, bool nullWhenError)
        {
            try
            {
                sheetNamesSlashDelimited = (string.IsNullOrWhiteSpace(sheetNamesSlashDelimited) ? "Sheet1" : sheetNamesSlashDelimited);
                IWorkbook workbook;
                if (fileName.EndsWith("xlsx"))
                {
                    workbook = new XSSFWorkbook();
                }
                else if (fileName.EndsWith("xls"))
                {
                    workbook = new HSSFWorkbook();
                }
                else throw new ArgumentException("xls or xlsx extenison must be provided");
                string[] sheetNames = sheetNamesSlashDelimited.Split('/');
                foreach(string sheetName in sheetNames)
                {
                    workbook.CreateSheet(sheetName);
                }
                using (var fileData = new FileStream(fileName, FileMode.Create))
                {
                    workbook.Write(fileData);
                }
                FileInfo template = new FileInfo(fileName);
                return template.Length;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
    }
}
