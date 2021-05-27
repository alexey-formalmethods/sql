using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.math
{
    public static class Utils
    {
        enum CalculationTypeNum
        {
            Max,
            Min,
            Avg
        }
        private static double? Agg(IEnumerable<double?> values, CalculationTypeNum type, bool nullWhenError)
        {
            try
            {
                switch (type)
                {
                    case CalculationTypeNum.Max: { return values.Max(); }
                    case CalculationTypeNum.Min: { return values.Min(); }
                    case CalculationTypeNum.Avg: { return values.Average(); }
                    default: { return null; }
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<double?>(e, nullWhenError);
            }
        }
        // Max ----------------------
        public static double? MaxOfTwo(double? a, double? b, bool nullWhenError)
        {
            return Agg(new double?[] { a, b }, CalculationTypeNum.Max, nullWhenError);
        }
        public static double? MaxOfThree(double? a, double? b, double? c, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c }, CalculationTypeNum.Max, nullWhenError);
        }
        public static double? MaxOfFour(double? a, double? b, double? c, double? d, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c, d }, CalculationTypeNum.Max, nullWhenError);
        }
        public static double? MaxOfFive(double? a, double? b, double? c, double? d, double? e, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c, d, e }, CalculationTypeNum.Max, nullWhenError);
        }
        // -----------------
        // Min ----------
        public static double? MinOfTwo(double? a, double? b, bool nullWhenError)
        {
            return Agg(new double?[] { a, b }, CalculationTypeNum.Min, nullWhenError);
        }
        public static double? MinOfThree(double? a, double? b, double? c, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c }, CalculationTypeNum.Min, nullWhenError);
        }
        public static double? MinOfFour(double? a, double? b, double? c, double? d, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c, d }, CalculationTypeNum.Min, nullWhenError);
        }
        public static double? MinOfFive(double? a, double? b, double? c, double? d, double? e, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c, d, e }, CalculationTypeNum.Min, nullWhenError);
        }
        // -------------
        // Avg ----------
        public static double? AvgOfTwo(double? a, double? b, bool nullWhenError)
        {
            return Agg(new double?[] { a, b }, CalculationTypeNum.Avg, nullWhenError);
        }
        public static double? AvgOfThree(double? a, double? b, double? c, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c }, CalculationTypeNum.Avg, nullWhenError);
        }
        public static double? AvgOfFour(double? a, double? b, double? c, double? d, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c, d }, CalculationTypeNum.Avg, nullWhenError);
        }
        public static double? AvgOfFive(double? a, double? b, double? c, double? d, double? e, bool nullWhenError)
        {
            return Agg(new double?[] { a, b, c, d, e }, CalculationTypeNum.Avg, nullWhenError);
        }


    }
}
