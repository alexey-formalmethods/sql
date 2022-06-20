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
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Util;

namespace bi_dev.sql.mssql.extensions.file.excel
{

    public class ExcelArgument
    {
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("range_from")]
        public string RangeFrom { get; set; }

        [JsonProperty("range_to")]
        public string RangeTo { get; set; }

        [JsonProperty("row_from")]
        public int? RowFrom { get; set; }

        [JsonProperty("row_to")]
        public int? RowTo { get; set; }

        [JsonProperty("col_from")]
        public int? ColFrom { get; set; }

        [JsonProperty("col_to")]
        public int? ColTo { get; set; }

        [JsonProperty("sheet_name")]
        public string SheetName { get; set; }

        [JsonProperty("sheet_num")]
        public int? SheetNum { get; set; }

        [JsonProperty("is_first_row_with_column_names")]
        public bool IsFirstRowWithColumnNames { get; set; }
    }
    public static class Utils
    {
     
        private static IWorkbook GetNewWorkBookFromFileName(string fileName, FileMode fileMode)
        {
            IWorkbook workbook;
            if (fileName.EndsWith("xlsx"))
            {
                if (fileMode == FileMode.Open)
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                }
                else workbook = new XSSFWorkbook();
            }
            else if (fileName.EndsWith("xls"))
            {
                if (fileMode== FileMode.Open)
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                }
                else workbook = new HSSFWorkbook();
            }
            else throw new ArgumentException("xls or xlsx extenison must be provided");
            return workbook;
        }
        public class ExcelSheet
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("sheet_index")]
            public int SheetIndex { get; set; }
        }
        public class ExcelSheetResult
        {
            [JsonProperty(PropertyName = "columns")]
            public List<string> Columns { get; set; }

            [JsonProperty(PropertyName = "values")]
            public List<List<string>> Values { get; set; }

            public ExcelSheetResult()
            {
                this.Columns = new List<string>();
                this.Values = new List<List<string>>();
            }
        }
        public static IEnumerable<ExcelSheet> getSheets(string fileName)
        {
            List<ExcelSheet> sheets = new List<ExcelSheet>();
            IWorkbook workbook = GetNewWorkBookFromFileName(fileName, FileMode.Open);
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                sheets.Add(new ExcelSheet() { Name = sheet.SheetName, SheetIndex = i });
            }
            return sheets;
        }

        public static ExcelSheetResult getContent(ExcelArgument request)
        {
            ExcelSheetResult result = new ExcelSheetResult();

            IWorkbook workbook = GetNewWorkBookFromFileName(request.FileName, FileMode.Open);
            ISheet sheet;
            if (!string.IsNullOrEmpty(request.SheetName) && !request.SheetNum.HasValue)
            {
                sheet = workbook.GetSheet(request.SheetName);
            }
            else if (request.SheetNum.HasValue)
            {
                sheet = workbook.GetSheetAt(request.SheetNum.Value);
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }
            //rows
            var rangeFrom = string.IsNullOrWhiteSpace(request.Range) ? request.RangeFrom: request.Range;
            var rangeTo = string.IsNullOrWhiteSpace(request.Range) ? request.RangeTo : request.Range;
            int rowNumberFrom = !string.IsNullOrWhiteSpace(rangeFrom) ? CellRangeAddress.ValueOf(rangeFrom).FirstRow : request.RowFrom ?? 0;
            if (rowNumberFrom < sheet.FirstRowNum) rowNumberFrom = sheet.FirstRowNum;
            int? rowNumberTo = !string.IsNullOrWhiteSpace(rangeTo) ? CellRangeAddress.ValueOf(rangeTo).LastRow : request.RowTo;
            if (!rowNumberTo.HasValue || rowNumberTo > sheet.LastRowNum) rowNumberTo = sheet.LastRowNum;
            // Columns
            List<int> cellNums = new List<int>();
            for (int i = rowNumberFrom; i <= rowNumberTo.Value; i++)
            {
                var row = sheet.GetRow(i);
                if (row != null)
                {
                    cellNums.Add(row.FirstCellNum);
                    cellNums.Add(row.LastCellNum);
                }
            }
            // from
            int minColumnNum = (cellNums.Count == 0 ? 0 : cellNums.Min());
            int maxColumnNum = (cellNums.Count == 0 ? 0 : cellNums.Max()) - 1;
            int columnNumberFrom = !string.IsNullOrWhiteSpace(rangeFrom) ? CellRangeAddress.ValueOf(rangeFrom).FirstColumn : request.ColFrom ?? 0;
            if (columnNumberFrom < minColumnNum) columnNumberFrom = minColumnNum;
            int? columnNumberTo = !string.IsNullOrWhiteSpace(rangeTo) ? CellRangeAddress.ValueOf(rangeTo).LastColumn : request.ColTo;
            if (!columnNumberTo.HasValue || columnNumberTo > maxColumnNum) columnNumberTo = maxColumnNum;

            if (sheet != null)
            {
                for (int i = rowNumberFrom; i <= rowNumberTo.Value; i++)
                {

                    IRow curRow = sheet.GetRow(i);
                    if (curRow != null)
                    {
                        if (!(i == rowNumberFrom && request.IsFirstRowWithColumnNames == true))
                        {
                            result.Values.Add(new List<string>());
                        }
                        for (int j = columnNumberFrom; j <= columnNumberTo.Value; j++)
                        {
                            object cellValue;
                            ICell cell = curRow.GetCell(j);
                            if (cell != null)
                            {
                                    
                                    switch (cell.CellType)
                                    {
                                        case CellType.Blank: { cellValue = null; break; }
                                        case CellType.Boolean: { cellValue = cell.BooleanCellValue; break; }
                                        case CellType.Numeric: { cellValue = cell.NumericCellValue; break; }
                                        case CellType.String: { cellValue = cell.StringCellValue; break; }
                                        case CellType.Error: { cellValue = cell.ErrorCellValue; break; }
                                        default:
                                            {
                                                cell.SetCellType(CellType.String);
                                                DataFormatter fmt = new DataFormatter();
                                                fmt.FormatCellValue(cell);
                                                cellValue = cell.StringCellValue;
                                                break;
                                            }
                                    }
                            }
                            else
                            {
                                cellValue = null;
                            }
                            if (i == rowNumberFrom && request.IsFirstRowWithColumnNames == true)
                            {
                                result.Columns.Add(cellValue?.ToString());
                            }
                            else
                            {
                                result.Values.Last().Add(cellValue?.ToString());
                            }
                        }
                    }
                }
            }
            return result;
        }
        [SqlFunction]
        public static string GetSheets(string fileName, bool nullWhenError)
        {
            try 
            {
                return JsonConvert.SerializeObject(getSheets(fileName));
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        [SqlFunction]
        public static string GetContent(string excelRequestJson, bool nullWhenError)
        {
            try
            {
                return JsonConvert.SerializeObject(getContent(JsonConvert.DeserializeObject<ExcelArgument>(excelRequestJson)));
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
}
    }
    // ------------




}
