using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions
{
    public class TableType
    {
        public int RowNumber { get; set; }
        public int ColumnIndex { get; set; }
        public string Value { get; set; }
        public TableType()
        {

        }
        public TableType(int rowNumber, int columnIndex, string value)
        {
            this.RowNumber = rowNumber;
            this.ColumnIndex = columnIndex;
            this.Value = value;
        }
        public static void FillRow(Object obj, out SqlInt32 rowType, out SqlInt32 key, out SqlChars value)
        {
            TableType table = (TableType)obj;
            rowType = new SqlInt32(table.RowNumber);
            key = new SqlInt32(table.ColumnIndex);
            value = new SqlChars(table.Value);
        }
    }

}
