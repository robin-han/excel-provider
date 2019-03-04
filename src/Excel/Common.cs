using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public enum ExcelTypeDetectionScheme
    {
        None = 0,
        RowScan = 1,
        ColumnFormat = 2
    }

    public enum ExcelDbType  //values same as DbType
    {
        Binary = 1,
        Boolean = 3,

        Date = 5,
        DateTime = 6,
        Time = 17,

        Decimal = 7,
        Double = 8,
        Single = 15,

        SByte = 14,
        Int16 = 10,
        Int32 = 11,
        Int64 = 12,

        Object = 13,
        String = 16,

        //Byte = 2,
        //UInt16 = 18,
        //UInt32 = 19,
        //UInt64 = 20
    }

    internal class DataTypeUtil
    {
        public static Type GetDotNetType(ExcelDbType dataType)
        {
            switch (dataType)
            {
                case ExcelDbType.Binary:
                    return typeof(byte[]);
                case ExcelDbType.Boolean:
                    return typeof(bool);
                case ExcelDbType.Date:
                    return typeof(DateTime);
                case ExcelDbType.DateTime:
                    return typeof(DateTime);
                case ExcelDbType.Time:
                    return typeof(TimeSpan);
                case ExcelDbType.Decimal:
                    return typeof(decimal);
                case ExcelDbType.Double:
                    return typeof(double);
                case ExcelDbType.Single:
                    return typeof(float);
                case ExcelDbType.SByte:
                    return typeof(sbyte);
                case ExcelDbType.Int16:
                    return typeof(short);
                case ExcelDbType.Int32:
                    return typeof(int);
                case ExcelDbType.Int64:
                    return typeof(long);
                case ExcelDbType.Object:
                    return typeof(object);
                case ExcelDbType.String:
                    return typeof(string);
            }

            throw new SystemException("Invalid data type");

        }

        public static string GetTypeName(ExcelDbType dataType)
        {
            switch (dataType)
            {
                case ExcelDbType.Binary:
                    return "Binary";
                case ExcelDbType.Boolean:
                    return "Boolean";
                case ExcelDbType.Date:
                    return "Date";
                case ExcelDbType.DateTime:
                    return "DateTime";
                case ExcelDbType.Time:
                    return "Time";
                case ExcelDbType.Decimal:
                    return "Decimal";
                case ExcelDbType.Double:
                    return "Double";
                case ExcelDbType.Single:
                    return "Single";
                case ExcelDbType.SByte:
                    return "SByte";
                case ExcelDbType.Int16:
                    return "Int16";
                case ExcelDbType.Int32:
                    return "Int32";
                case ExcelDbType.Int64:
                    return "Int64";
                case ExcelDbType.Object:
                    return "Object";
                case ExcelDbType.String:
                    return "String";
            }
            return string.Empty;
        }

        public static DbType GetDbType(Type type)
        {
            if (type == typeof(TimeSpan))
            {
                return DbType.Time;
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    return DbType.Object;

                case TypeCode.DBNull:
                    return DbType.Object;

                case TypeCode.Boolean:
                    return DbType.Boolean;

                case TypeCode.SByte:
                    return DbType.SByte;

                case TypeCode.Byte:
                    return DbType.Byte;

                case TypeCode.Int16:
                    return DbType.Int16;

                case TypeCode.UInt16:
                    return DbType.UInt16;

                case TypeCode.Int32:
                    return DbType.Int32;

                case TypeCode.UInt32:
                    return DbType.UInt32;

                case TypeCode.Int64:
                    return DbType.Int64;

                case TypeCode.UInt64:
                    return DbType.UInt64;

                case TypeCode.Single:
                    return DbType.Single;

                case TypeCode.Double:
                    return DbType.Double;

                case TypeCode.Decimal:
                    return DbType.Decimal;

                case TypeCode.DateTime:
                    return DbType.DateTime;

                case TypeCode.String:
                    return DbType.String;
            }

            throw new SystemException("Invalid data type");
        }

        public static bool IsNumber(object value)
        {
            return value is double
                || value is float
                || value is decimal
                || value is long
                || value is int
                || value is short
                || value is sbyte
                || value is ulong
                || value is uint
                || value is ushort
                || value is byte;
        }

        public static bool IsDate(object value)
        {
            return value is DateTime
                || value is TimeSpan;
        }

        public static bool IsDateTime(object value)
        {
            return value is DateTime;
        }

        public static bool IsTimeSpan(object value)
        {
            return value is TimeSpan;
            
        }

        public static string ToString(object value)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ToString(value);
        }

        public static DateTime ToDateTime(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }

            if (value is TimeSpan)
            {
                return DateTime.FromOADate(((TimeSpan)value).TotalDays);
            }

            if (IsNumber(value))
            {
                return DateTime.FromOADate(ToDouble(value));
            }

            if (value is string)
            {
                if (DateTime.TryParse((string)value, out DateTime date))
                {
                    return date;
                }
            }
            return Convert.ToDateTime(value);
        }

        public static TimeSpan ToTimeSpan(object value)
        {
            if (value is TimeSpan)
            {
                return (TimeSpan)value;
            }

            if(value is string)
            {
                if(TimeSpan.TryParse((string)value, out TimeSpan time))
                {
                    return time;
                }
            }

            return TimeSpan.FromTicks(ToDateTime(value).Ticks);
        }

        public static int ToInt(object value)
        {
           if (value is DateTime)
                return (int)((DateTime)value).ToOADate();
            else if (value is TimeSpan)
                return (int)((TimeSpan)value).TotalDays;
            else
                return Convert.ToInt32(value, CultureInfo.CurrentCulture);
        }

        public static long ToLong(object value)
        {
           if (value is DateTime)
                return (long)((DateTime)value).ToOADate();
            else if (value is TimeSpan)
                return (long)((TimeSpan)value).TotalDays;
            else
                return Convert.ToInt64(value, CultureInfo.CurrentCulture);
        }

        public static Double ToDouble(object value)
        {
            if (value is DateTime)
                return ((DateTime)value).ToOADate();
            else if (value is TimeSpan)
                return ((TimeSpan)value).TotalDays;
            else
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        public static bool ToBoolean(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }

            if (value is string)
            {
                return string.Compare("1", (string)value, true) == 0
                    || string.Compare("true", (string)value, true) == 0;
            }

            return Convert.ToBoolean(value);
        }

    }
}
