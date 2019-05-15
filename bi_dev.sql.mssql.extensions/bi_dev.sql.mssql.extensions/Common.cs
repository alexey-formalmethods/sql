using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions
{
    public static class Common
    {
        public static T ThrowIfNeeded<T>(Exception ex, bool nullWhenError)
        {
            if (nullWhenError) return default(T);
            else throw ex;
        }
    }
}
