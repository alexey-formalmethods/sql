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

namespace bi_dev.sql.mssql.extensions.file.excel
{
    public class Constants
    {
        public const int FontHeightInPoints = 11;
        public const bool IsBold = true;
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    }
    public class SimpleObject
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        public SimpleObject(string value)
        {
            this.Value = value;
        }
    }
    public enum ExcelFileMode
    {
        Create,
        Open
    }
    public enum FillExcelRowType
    {
        Value,
        Name
    }
    public class CellStyleWithLimited
    {
        public ICellStyle Style { get; set; }
        public LimitedCellStyle LimitedStyle {get; set;}
    }
    public class LimitedCellStyle
    {
        public LimitedCellStyleFont LimitedCellStyleFont { get; set; }
        public bool? BorderTop { get; set; }
        public bool? BorderBottom { get; set; }
        public bool? BorderRight { get; set; }
        public bool? BorderLeft { get; set; }
        public int? BorderStyle { get; set; }
        public short? BackgroundColor { get; set; }
    }
    public class LimitedCellStyleFont
    {
        public int? FontHeightInPoints { get; }
        public bool? IsBold { get; }
        private LimitedCellStyleFont(int fontHeightInPoints, bool isBold)
        {
            this.FontHeightInPoints = fontHeightInPoints;
            this.IsBold = isBold;
        }
        public static LimitedCellStyleFont GetLimitedCellStyleFont(int? fontHeightInPoints, bool? isBold)
        {
            if (!fontHeightInPoints.HasValue && !isBold.HasValue) return null;
            else return new LimitedCellStyleFont(
                fontHeightInPoints.HasValue? fontHeightInPoints.Value:Constants.FontHeightInPoints,
                isBold.HasValue?isBold.Value: Constants.IsBold
            );
        }
    }
    public static class Utils
    {

        private static IWorkbook GetNewWorkBookFromFileName(string fileName, ExcelFileMode excelFileMode)
        {
            IWorkbook workbook;
            if (fileName.EndsWith("xlsx"))
            {
                if (excelFileMode == ExcelFileMode.Open)
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
                if (excelFileMode == ExcelFileMode.Open)
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
        [SqlFunction]
        public static long? CreateExcelFile(string fileName, string sheetNamesSlashDelimited, bool nullWhenError)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    sheetNamesSlashDelimited = (string.IsNullOrWhiteSpace(sheetNamesSlashDelimited) ? "Sheet1" : sheetNamesSlashDelimited);
                    IWorkbook workbook = GetNewWorkBookFromFileName(fileName, ExcelFileMode.Create);
                    string[] sheetNames = sheetNamesSlashDelimited.Split('/');
                    foreach (string sheetName in sheetNames)
                    {
                        var sheet = workbook.CreateSheet(sheetName);
                    }
                    using (var fileData = new FileStream(fileName, FileMode.Create))
                    {
                        workbook.Write(fileData);
                    }
                    FileInfo template = new FileInfo(fileName);
                    return template.Length;
                }
                else return null;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
        private static void SetLimitedStyle(this ICell cell, CellStyleWithLimited cellStyleWithLimited)
        {
            var workBook = cell.Sheet.Workbook;
            if (cellStyleWithLimited.LimitedStyle.LimitedCellStyleFont != null)
            {
                cell.CellStyle.SetFont(cellStyleWithLimited.Style.GetFont(cell.Sheet.Workbook));
            }

            if (cellStyleWithLimited.LimitedStyle.BorderRight.HasValue)
            {
                cell.CellStyle.BorderRight = cellStyleWithLimited.Style.BorderRight;
            }
            if (cellStyleWithLimited.LimitedStyle.BorderLeft.HasValue)
            {
                cell.CellStyle.BorderLeft = cellStyleWithLimited.Style.BorderLeft;
            }
            if (cellStyleWithLimited.LimitedStyle.BorderTop.HasValue)
            {
                cell.CellStyle.BorderTop = cellStyleWithLimited.Style.BorderTop;
            }
            if (cellStyleWithLimited.LimitedStyle.BorderBottom.HasValue)
            {
                cell.CellStyle.BorderTop = cellStyleWithLimited.Style.BorderBottom;
            }
            if (cellStyleWithLimited.LimitedStyle.BackgroundColor.HasValue)
            {
                cell.CellStyle.FillPattern = FillPattern.SolidForeground;
                cell.CellStyle.FillForegroundColor = cellStyleWithLimited.LimitedStyle.BackgroundColor.Value;
            }
            cell.CellStyle = cellStyleWithLimited.Style;

        }
        private static CellStyleWithLimited GetCellStyle(
            IWorkbook workbook,
            LimitedCellStyle limitedCellStyle
        )
        {

            ICellStyle cellStyle = workbook.CreateCellStyle();
            if (limitedCellStyle?.LimitedCellStyleFont != null)
            {
                var font = workbook.CreateFont();
                font.FontHeightInPoints = limitedCellStyle.LimitedCellStyleFont.FontHeightInPoints.HasValue ? limitedCellStyle.LimitedCellStyleFont.FontHeightInPoints.Value : Constants.FontHeightInPoints;
                font.IsBold = limitedCellStyle.LimitedCellStyleFont.IsBold.HasValue ? limitedCellStyle.LimitedCellStyleFont.IsBold.Value : false;
                cellStyle.SetFont(font);
            }

            BorderStyle cellBorderStyle = (limitedCellStyle.BorderStyle.HasValue) ? (BorderStyle)limitedCellStyle.BorderStyle : BorderStyle.None;
            if (limitedCellStyle.BorderLeft.HasValue && limitedCellStyle.BorderLeft.Value) cellStyle.BorderLeft = cellBorderStyle;
            if (limitedCellStyle.BorderBottom.HasValue && limitedCellStyle.BorderBottom.Value) cellStyle.BorderBottom = cellBorderStyle;
            if (limitedCellStyle.BorderTop.HasValue && limitedCellStyle.BorderTop.Value) cellStyle.BorderTop = cellBorderStyle;
            if (limitedCellStyle.BorderRight.HasValue && limitedCellStyle.BorderRight.Value) cellStyle.BorderRight = cellBorderStyle;
            if (limitedCellStyle.BackgroundColor.HasValue)
            {
                // yes, it works
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillForegroundColor = limitedCellStyle.BackgroundColor.Value;
                // colors here: NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index
            }
            return new CellStyleWithLimited() { LimitedStyle = limitedCellStyle, Style = cellStyle };
        }
        public static bool? FormatExcelFile(
            string fileName,
            string sheetName,
            int? rowNumberFrom,
            int? rowNumberTo,
            int? columnNumberFrom,
            int? columnNumberTo,
            int? fontHeightInPoints,
            bool? isBold,
            bool? borderTop,
            bool? borderBottom,
            bool? borderRight,
            bool? borderLeft,
            int? borderStyle,
            short? backgroundColor,
            bool autoSizeColumns,
            bool nullWhenError
        )
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    IWorkbook workbook = GetNewWorkBookFromFileName(fileName, ExcelFileMode.Open);
                    var sheet = workbook.GetSheet(sheetName);
                    LimitedCellStyle lcs = new LimitedCellStyle()
                    {
                        BackgroundColor = backgroundColor,
                        BorderBottom = borderBottom,
                        BorderLeft = borderLeft,
                        BorderRight = borderRight,
                        BorderStyle = borderStyle,
                        BorderTop = borderTop,
                        LimitedCellStyleFont = LimitedCellStyleFont.GetLimitedCellStyleFont(fontHeightInPoints, isBold)
                    };

                    if (rowNumberFrom.HasValue || rowNumberTo.HasValue || columnNumberFrom.HasValue || columnNumberFrom.HasValue)
                    {
                        // rows
                        // from
                        if (!rowNumberFrom.HasValue) rowNumberFrom = 0;
                        // to
                        if (!rowNumberTo.HasValue) rowNumberTo = sheet.LastRowNum;
                        // Columns
                        List<int> lastCellNums = new List<int>();
                        for (int i = rowNumberFrom.Value; i <= rowNumberTo.Value; i++)
                        {
                            var row = sheet.GetRow(i);
                            if (row != null) lastCellNums.Add(row.LastCellNum);
                        }
                        // from
                        if (!columnNumberFrom.HasValue) columnNumberFrom = 0;

                        // to
                        int? lastColumnNumber = (lastCellNums.Count == 0 ? 0 : lastCellNums.Max());
                        // add -1, some bug shit
                        if (lastColumnNumber.HasValue && lastColumnNumber.Value > columnNumberFrom) lastColumnNumber = lastColumnNumber.Value - 1;
                        else lastColumnNumber = columnNumberFrom;


                        if (!columnNumberTo.HasValue) columnNumberTo = lastColumnNumber;
                        for (int i = 0; i <= columnNumberTo; i++)
                        {
                            sheet.AutoSizeColumn(i);
                        }
                        var style = GetCellStyle(workbook, lcs);
                        for (int i = rowNumberFrom.Value; i <= rowNumberTo.Value; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                row = sheet.CreateRow(i);
                            }

                            for (int j = columnNumberFrom.Value; j <= columnNumberTo.Value; j++)
                            {
                                var cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                if (cell != null)
                                {
                                    cell.SetLimitedStyle(style);
                                }
                            }

                        }
                        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(fs);
                        }
                        return true;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, nullWhenError);
            }
        }

        public class ExcelReadResult
        {
            [JsonProperty(PropertyName = "columns")]
            public List<string> Columns { get; set; }

            [JsonProperty(PropertyName = "values")]
            public List<List<object>> Values { get; set; }

            public ExcelReadResult()
            {
                this.Columns = new List<string>();
                this.Values = new List<List<object>>();
            }
        }
        [SqlFunction]
        public static string GetExcelContent(
            string fileName,
            string sheetName,
            int? rowNumberFrom,
            int? rowNumberTo,
            int? columnNumberFrom,
            int? columnNumberTo,
            bool isFirstRowWithColumnNames,
            bool nullWhenError
        )
        {
            try
            {
                ExcelReadResult result = new ExcelReadResult();

                IWorkbook workbook = GetNewWorkBookFromFileName(fileName, ExcelFileMode.Open);
                ISheet sheet;
                if (string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheetAt(0);
                }
                else
                {
                    sheet = workbook.GetSheet(sheetName);
                }
                if (!rowNumberFrom.HasValue || rowNumberFrom < sheet.FirstRowNum) rowNumberFrom = sheet.FirstRowNum;
                // to
                if (!rowNumberTo.HasValue || rowNumberTo > sheet.LastRowNum) rowNumberTo = sheet.LastRowNum;
                // Columns
                List<int> cellNums = new List<int>();
                for (int i = rowNumberFrom.Value; i <= rowNumberTo.Value; i++)
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
                if (!columnNumberFrom.HasValue || columnNumberFrom < minColumnNum) columnNumberFrom = minColumnNum;
                // to
                if (!columnNumberTo.HasValue || columnNumberTo > maxColumnNum) columnNumberTo = maxColumnNum;

                if (sheet != null)
                {
                    for (int i = rowNumberFrom.Value; i <= rowNumberTo.Value; i++)
                    {

                        IRow curRow = sheet.GetRow(i);
                        if (curRow != null)
                        {
                            if (!(i == rowNumberFrom.Value && isFirstRowWithColumnNames))
                            {
                                result.Values.Add(new List<object>());
                            }
                            for (int j = columnNumberFrom.Value; j <= columnNumberTo.Value; j++)
                            {
                                object cellValue;
                                ICell cell = curRow.GetCell(j);
                                if (cell != null)
                                {
                                    if (cell.DateCellValue != null)
                                    {
                                        cellValue = cell.DateCellValue;
                                    }
                                    else 
                                    {
                                        cell.SetCellType(CellType.String);
                                        DataFormatter fmt = new DataFormatter();
                                        fmt.FormatCellValue(cell);
                                        cellValue = cell.StringCellValue;
                                    }
                                }
                                else
                                {
                                    cellValue = null;
                                }
                                if (i == rowNumberFrom.Value && isFirstRowWithColumnNames)
                                {
                                    result.Columns.Add(cellValue?.ToString());
                                }
                                else
                                {
                                    result.Values.Last().Add(cellValue);
                                }
                            }
                        }
                    }
                }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }

        private static void FillRow(this IRow row, int columnNumberFrom, List<JProperty> properties, FillExcelRowType fillExcelRowType, ICellStyle cellStyle)
        {
            
            for (int j = 0; j < properties.Count; j++)
            {
                var property = properties[j];
                var cell = row.GetCell(columnNumberFrom + j, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (fillExcelRowType == FillExcelRowType.Value)
                {
                    JToken value = properties[j].Value;
                    var valueType = value.Type;
                    switch (valueType)
                    { 
                        case JTokenType.Date:
                            {
                                cell.SetCellValue((DateTime)value);
                                if (cellStyle != null) cell.CellStyle = cellStyle;
                                break;
                            }
                        case JTokenType.Float:
                            {
                                cell.SetCellValue((double)value);
                                break;
                            }
                        case JTokenType.Boolean:
                            {
                                cell.SetCellValue((bool)value);
                                break;
                            }
                        case JTokenType.Integer:
                            {
                                cell.SetCellValue((double)value);
                                break;
                            }
                        case JTokenType.String:
                            {
                                cell.SetCellValue((string)value);
                                break;
                            }
                        case JTokenType.Null:
                            {
                                
                                break;
                            }
                        default:
                            {
                                cell.SetCellValue(JsonConvert.SerializeObject(value));
                                break;
                            }
                    }
                }
                else
                {
                    cell.SetCellValue(properties[j].Name);
                }
            }
        }
        
        public static bool? EditExcelFile(string fileName, string sheetName, int rowNumberFrom, int columnNumberFrom, string jsonObject, bool includeColumnNames, string dateTimeFormat, bool nullWhenError)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonObject))
                {
                    if (string.IsNullOrWhiteSpace(dateTimeFormat)) dateTimeFormat = Constants.DateTimeFormat;
                    var workBook = GetNewWorkBookFromFileName(fileName, ExcelFileMode.Open);
                    var sheet = workBook.GetSheet(sheetName);
                    var dataFormat = workBook.CreateDataFormat().GetFormat(dateTimeFormat);
                    ICellStyle cellStyle = workBook.CreateCellStyle();
                    cellStyle.DataFormat = workBook.CreateDataFormat().GetFormat(dateTimeFormat);
                    List<JObject> values;
                    try
                    {
                        values = JsonConvert.DeserializeObject<List<JObject>>(jsonObject);
                    }
                    catch (JsonSerializationException)
                    {
                        values = new List<JObject>() { JsonConvert.DeserializeObject<JObject>(jsonObject) };
                    }
                    catch (JsonReaderException)
                    {
                        values = new List<JObject>() {
                            JsonConvert.DeserializeObject<JObject>(
                                JsonConvert.SerializeObject(
                                    new SimpleObject(jsonObject)
                                )
                            )
                        };
                    }

                    if (values.Count > 0)
                    {
                        IRow row;
                        if (includeColumnNames)
                        {
                            List<JProperty> columnNames = values.FirstOrDefault().Properties().ToList();
                            row = sheet.GetRow(rowNumberFrom);
                            if (row == null) { row = sheet.CreateRow(rowNumberFrom); }
                            row.FillRow(columnNumberFrom, columnNames, FillExcelRowType.Name, cellStyle);
                        }
                        for (int i = 0; i < values.Count; i++)
                        {
                            int rowIndex = rowNumberFrom + i + ((includeColumnNames) ? 1 : 0);
                            row = sheet.GetRow(rowIndex);
                            if (row == null) row = sheet.CreateRow(rowIndex);
                            List<JProperty> properties = values[i].Properties().ToList();
                            row.FillRow(columnNumberFrom, properties, FillExcelRowType.Value, cellStyle);
                        }
                        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            workBook.Write(fs);
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, nullWhenError);
            }
        }
    }
}
