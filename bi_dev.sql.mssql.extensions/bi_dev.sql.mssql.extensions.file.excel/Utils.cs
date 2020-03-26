﻿using System;
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
    public class CustomCellStyle
    {

    }
    public static class Utils
    {
        public enum ExcelFileMode
        {
            Create,
            Open
        }
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
                sheetNamesSlashDelimited = (string.IsNullOrWhiteSpace(sheetNamesSlashDelimited) ? "Sheet1" : sheetNamesSlashDelimited);
                IWorkbook workbook = GetNewWorkBookFromFileName(fileName, ExcelFileMode.Create);
                string[] sheetNames = sheetNamesSlashDelimited.Split('/');
                foreach(string sheetName in sheetNames)
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
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<long?>(e, nullWhenError);
            }
        }
        private static ICellStyle GetCellStyle(
            IWorkbook workbook,
            int? fontHeightInPoints,
            bool? isBold,
            bool? borderTop,
            bool? borderBottom,
            bool? borderRight,
            bool? borderLeft,
            int? borderStyle,
            short? backgroundColor
        )
        {

            ICellStyle cellStyle = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.FontHeightInPoints = fontHeightInPoints.HasValue? fontHeightInPoints.Value:11;
            if (isBold.HasValue) {
                if (isBold.HasValue) font.IsBold = isBold.Value;
            }
            cellStyle.SetFont(font);
            var e = IndexedColors.Grey25Percent.Index;
            BorderStyle cellBorderStyle = (borderStyle.HasValue)?(BorderStyle)borderStyle:BorderStyle.None; 
            if (borderLeft.HasValue && borderLeft.Value) cellStyle.BorderLeft = cellBorderStyle;
            if (borderBottom.HasValue && borderBottom.Value) cellStyle.BorderBottom = cellBorderStyle;
            if (borderTop.HasValue && borderTop.Value) cellStyle.BorderTop = cellBorderStyle;
            if (borderRight.HasValue && borderRight.Value) cellStyle.BorderRight = cellBorderStyle;
            if (backgroundColor.HasValue)
            {
                // yes, it works
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillForegroundColor = backgroundColor.Value;
            }
            return cellStyle;
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
            bool nullWhenError
        )
        {
            try
            {
                IWorkbook workbook = GetNewWorkBookFromFileName(fileName, ExcelFileMode.Open);
                var sheet = workbook.GetSheet(sheetName);
                var style = GetCellStyle(workbook, fontHeightInPoints, isBold, borderTop, borderBottom, borderRight, borderLeft, borderStyle, backgroundColor);
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
                    for (int i = rowNumberFrom.Value; i <= rowNumberTo.Value; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null)
                        {
                            row = sheet.CreateRow(i);
                        }

                        for (int j = columnNumberFrom.Value; j <= columnNumberTo.Value; j++)
                        {
                            var cell = row.GetCell(j);
                            if (cell == null) cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            if (cell != null)
                            {
                                cell.CellStyle = style;
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
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<bool?>(e, nullWhenError);
            }
        }
        public static bool? EditExcelFile(string fileName, string sheetName, int rowNumber, int columnNumber, string jsonObject, bool nullWhenError)
        {
            var workBook = GetNewWorkBookFromFileName(fileName, ExcelFileMode.Open);
            var sheet = workBook.GetSheet(sheetName);
            List<JObject> values;
            try
            {
                values = JsonConvert.DeserializeObject<List<JObject>>(jsonObject);
            }
            catch (JsonSerializationException jsex)
            {
                values = new List<JObject>() { JsonConvert.DeserializeObject<JObject>(jsonObject) };
            } 
            for (int i = 0; i < values.Count; i++)
            {
                var row = sheet.GetRow(rowNumber + i);
                if (row == null) row = sheet.CreateRow(i);
                List<JProperty> properties = values[i].Properties().ToList();
                for (int j = 0; j < properties.Count(); j++)
                {
                    var property = properties[j];
                    var value = properties[j].Value;
                    var valueType = value.Type;
                    var cell = row.GetCell(columnNumber + j, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    switch (valueType)
                    {
                        case JTokenType.Date:
                        {
                            cell.SetCellValue((DateTime)value);
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
                        default:
                        {
                            cell.SetCellValue(JsonConvert.SerializeObject(value));
                            break;
                        }
                    }
                }
            }
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                workBook.Write(fs);
            }
            return true;
        }
    }
}
