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
        public static T ThrowIfNeeded<T>(Exception ex, bool defaultWhenError, T defaultValue)
        {
            if (defaultWhenError) return defaultValue;
            else throw ex;
        }
		/// <summary>
        /// designed to throw in user defined functions
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <exception cref="Exception"></exception>
        public static void ThrowException(string errorMessage)
        {
            throw new Exception(errorMessage);
        }

    }
}
