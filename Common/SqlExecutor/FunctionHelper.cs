using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlExecutor
{
    public class FunctionHelper
    {
        #region StringFunctions
        public static object Concat(object o1, object o2, IEnumerable<object> os)
        {
            var strs = new List<string> { IsNull(o1) ? string.Empty : o1.ToString(), IsNull(o2) ? string.Empty : o2.ToString() };
            if (null != os)
            {
                var others = os.Select(o =>
                {
                    if (IsNull(o))
                    {
                        return string.Empty;
                    }
                    else if (o.GetType() == typeof(float) || o.GetType() == typeof(double))
                    {
                        return Convert.ToDouble(o).ToString("R");
                    }
                    else
                    {
                        return o.ToString();
                    }
                }).ToList();
                strs.AddRange(others);
            }
            return string.Concat(strs);
        }
        public static object Format(object o, string format)
        {
            if (IsNull(o))
            {
                return string.Empty;
            }
            var formatter = o as IFormattable;
            return null == formatter ? string.Format(format, o) : formatter.ToString(format, null);
        }
        public static object IndexOf(object o1, object o2, int start = 0)
        {
            if (IsNull(o1) || IsNull(o2))
            {
                return DBNull.Value;
            }
            string src = o1.ToString(), find = o2.ToString();
            if (src.Length == 0 || find.Length == 0)
            {
                return 0;
            }
            if (start > 0)
            {
                if (start >= src.Length)
                {
                    return 0;
                }
                src = src.Substring(start);
            }
            return src.IndexOf(find, StringComparison.OrdinalIgnoreCase) + 1;
        }
        public static object IsNull(object o1, object o2, out Type outType)
        {
            outType = typeof(string);
            if (IsNull(o1))
            {
                if (!IsNull(o2))
                {
                    outType = o2.GetType();
                }
                return o2;
            }
            else
            {
                outType = o1.GetType();
                return o1;
            }
        }
        // Index begin with 1, if start less than 1, len = len - abs(start) - 1
        public static object Substring(object o, int start, int len)
        {
            if (len < 0)
            {
                throw new InvalidOperationException("Invalid length parameter passed to the substring function.");
            }

            if (IsNull(o))
            {
                return DBNull.Value;
            }

            if (start < 1)
            {
                len -= Math.Abs(start) + 1;
                start = 0;
            }
            else
            {
                start -= 1;
            }

            if (len <= 0)
            {
                return string.Empty;
            }

            var str = o.ToString();
            if (start >= str.Length)
            {
                return string.Empty;
            }
            else
            {
                return str.Substring(start, Math.Min(len, str.Length - start));
            }
        }
        public static object Left(object o, int len)
        {
            if (len < 0)
            {
                throw new InvalidOperationException("Invalid length parameter passed to the left function.");
            }
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            var value = o.ToString();
            return value.Substring(0, Math.Min(value.Length, len));
        }
        public static object Right(object o, int len)
        {
            if (len < 0)
            {
                throw new InvalidOperationException("Invalid length parameter passed to the right function.");
            }
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            if (0 == len)
            {
                return string.Empty;
            }
            var str = o.ToString();
            var start = str.Length - len;
            return start <= 0 ? str : str.Substring(start);
        }
        public static object Len(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else if (typeof(float) == o.GetType() || typeof(double) == o.GetType())
            {
                return Convert.ToDouble(o).ToString("R").Length;
            }
            else
            {
                return o.ToString().Length;
            }
        }
        public static object Lower(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return o.ToString().ToLower();
            }
        }
        public static object Upper(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return o.ToString().ToUpper();
            }
        }
        public static object Trim(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return o.ToString().Trim(' ', '\t', '\r', '\n');
            }
        }
        public static object LTrim(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return o.ToString().TrimStart(' ', '\t', '\r', '\n');
            }
        }
        public static object RTrim(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return o.ToString().TrimEnd(' ', '\t', '\r', '\n');
            }
        }
        public static object Replace(object o, string pattern, string replacement)
        {
            if (IsNull(o) || null == pattern || null == replacement)
            {
                return DBNull.Value;
            }
            else
            {
                return o.ToString().Replace(pattern, replacement);
            }
        }
        public static object Replicate(object o, int times)
        {
            if (IsNull(o) || times < 0)
            {
                return DBNull.Value;
            }
            if (0 == times)
            {
                return string.Empty;
            }
            var str = o.ToString();
            var ret = str;
            for (var i = 1; i < times; i++)
            {
                ret = string.Concat(ret, str);
            }
            return ret;
        }
        public static object Reverse(object o)
        {
            char[] arr = null;
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else if (typeof(float) == o.GetType() || typeof(double) == o.GetType())
            {
                arr = Convert.ToDouble(o).ToString("R").ToCharArray();
            }
            else
            {
                arr = o.ToString().ToCharArray();
            }
            Array.Reverse(arr);
            return new string(arr);
        }
        public static object ToString(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else if (typeof(float) == o.GetType() || typeof(double) == o.GetType())
            {
                return Convert.ToDouble(o).ToString("R");
            }
            else
            {
                return o.ToString();
            }
        }
        #endregion // StringFunctions

        #region DatetimeFunctions
        public static object DateAdd(string datepart, int num, object o)
        {
            if (string.IsNullOrEmpty(datepart))
            {
                throw new InvalidOperationException("Datepart can not be empty.");
            }

            var dp = DateParts.GetDatePart(datepart);
            if (EDatePart.Unknown == dp)
            {
                throw new InvalidOperationException($"'{datepart}' is not a recognized dateadd option.");
            }
            if (IsNull(o))
            {
                return DBNull.Value;
            }

            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(o);
            }
            catch
            {
                throw new InvalidOperationException($"Conversion failed when converting date and/or time from '{o.ToString()}'.");
            }

            switch (dp)
            {
                case EDatePart.Year:
                    return dt.AddYears(num);
                case EDatePart.Quarter:
                    return dt.AddMonths(num * 3);
                case EDatePart.Month:
                    return dt.AddMonths(num);
                case EDatePart.DayOfYear:
                case EDatePart.Day:
                case EDatePart.WeekDay:
                    return dt.AddDays(num);
                case EDatePart.Week:
                    return dt.AddDays(num * 7);
                case EDatePart.Hour:
                    return dt.AddHours(num);
                case EDatePart.Minute:
                    return dt.AddMinutes(num);
                case EDatePart.Second:
                    return dt.AddSeconds(num);
                case EDatePart.MilliSecond:
                    return dt.AddMilliseconds(num);
                default:
                    throw new InvalidOperationException($"'{datepart}' is not a recognized dateadd option.");
            }
        }
        public static object DateDiff(string datepart, object o1, object o2)
        {
            if (string.IsNullOrEmpty(datepart))
            {
                throw new InvalidOperationException("Datepart can not be empty.");
            }

            var dp = DateParts.GetDatePart(datepart);
            if (EDatePart.Unknown == dp)
            {
                throw new InvalidOperationException($"'{datepart}' is not a recognized datediff option.");
            }
            if (IsNull(o1) || IsNull(o2))
            {
                return DBNull.Value;
            }

            DateTime dt1;
            try
            {
                dt1 = Convert.ToDateTime(o1);
            }
            catch
            {
                throw new InvalidOperationException($"Conversion failed when converting date and/or time from '{o1.ToString()}'.");
            }

            DateTime dt2;
            try
            {
                dt2 = Convert.ToDateTime(o2);
            }
            catch
            {
                throw new InvalidOperationException($"Conversion failed when converting date and/or time from '{o2.ToString()}'.");
            }

            switch (dp)
            {
                case EDatePart.Year:
                    return dt2.Year - dt1.Year;
                case EDatePart.Quarter:
                    return (dt2.Year - dt1.Year) * 4 + (dt2.Month - dt1.Month) / 3;
                case EDatePart.Month:
                    return (dt2.Year - dt1.Year) * 12 + (dt2.Month - dt1.Month);
                case EDatePart.DayOfYear:
                case EDatePart.Day:
                case EDatePart.WeekDay:
                    var days = (dt2.Date - dt1.Date).TotalDays;
                    return Math.Round(days);
                case EDatePart.Week:
                    var weeks = (dt2.Date - dt1.Date).TotalDays / 7;
                    return Math.Round(weeks);
                case EDatePart.Hour:
                    var hours = (dt2 - dt1).TotalHours;
                    return Math.Round(hours);
                case EDatePart.Minute:
                    var minutes = (dt2 - dt1).TotalMinutes;
                    return Math.Round(minutes);
                case EDatePart.Second:
                    var seconds = (dt2 - dt1).TotalSeconds;
                    return Math.Round(seconds);
                case EDatePart.MilliSecond:
                    return (dt2 - dt1).TotalMilliseconds;
                default:
                    throw new InvalidOperationException($"'{datepart}' is not a recognized datediff option.");
            }
        }
        public static object DateFromParts(int year, int month, int day)
        {
            if (year < 0 || year > 9999 || month < 0 || month > 12 || day < 0 || day > DateTime.DaysInMonth(year, month))
            {
                throw new InvalidOperationException("Cannot construct data type date, some of the arguments have values which are not valid.");
            }
            return new DateTime(year, month, day);
        }
        public static object DatePart(string datepart, object o)
        {
            if (string.IsNullOrEmpty(datepart))
            {
                throw new InvalidOperationException("Datepart can not be empty.");
            }

            var dp = DateParts.GetDatePart(datepart);
            if (EDatePart.Unknown == dp)
            {
                throw new InvalidOperationException($"'{datepart}' is not a recognized datepart option.");
            }
            if (IsNull(o))
            {
                return DBNull.Value;
            }

            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(o);
            }
            catch
            {
                throw new InvalidOperationException($"Conversion failed when converting date and/or time from '{o.ToString()}'.");
            }

            switch (dp)
            {
                case EDatePart.Year:
                    return dt.Year;
                case EDatePart.Quarter:
                    return (dt.Month / 3) + 1;
                case EDatePart.Month:
                    return dt.Month;
                case EDatePart.DayOfYear:
                    return dt.DayOfYear;
                case EDatePart.Day:
                    return dt.Day;
                case EDatePart.WeekDay:
                    return (int)dt.DayOfWeek + 1;
                case EDatePart.Week:
                    return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                case EDatePart.Hour:
                    return dt.Hour;
                case EDatePart.Minute:
                    return dt.Minute;
                case EDatePart.Second:
                    return dt.Second;
                case EDatePart.MilliSecond:
                    return dt.Millisecond;
                default:
                    throw new InvalidOperationException($"'{datepart}' is not a recognized datepart option.");
            }
        }
        public static object EOMonth(object o, int mon = 0)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }

            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(o);
            }
            catch
            {
                throw new InvalidOperationException($"Conversion failed when converting date and/or time from '{o.ToString()}'.");
            }

            dt = dt.AddMonths(mon);
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }
        public static object GetDate()
        {
            return DateTime.Now;
        }
        public static object GetUtcDate()
        {
            return DateTime.UtcNow;
        }
        #endregion // DatetimeFunctions

        #region MathematicsFunctions
        public static object Abs(object o, out Type outType)
        {
            outType = typeof(int);
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                var type = o.GetType();
                outType = type;
                if (type == typeof(sbyte))
                {
                    return Math.Abs(Convert.ToSByte(o));
                }
                else if (type == typeof(short))
                {
                    return Math.Abs(Convert.ToInt16(o));
                }
                else if (type == typeof(int))
                {
                    return Math.Abs(Convert.ToInt32(o));
                }
                else if (type == typeof(long))
                {
                    return Math.Abs(Convert.ToInt64(o));
                }
                else if (type == typeof(float))
                {
                    return Math.Abs(Convert.ToSingle(o));
                }
                else if (type == typeof(double))
                {
                    return Math.Abs(Convert.ToDouble(o));
                }
                else if (type == typeof(decimal))
                {
                    return Math.Abs(Convert.ToDecimal(o));
                }
                else
                {
                    var str = o.ToString();
                    if (int.TryParse(str, out int intValue))
                    {
                        outType = typeof(int);
                        return Math.Abs(intValue);
                    }
                    else if (long.TryParse(str, out long longValue))
                    {
                        outType = typeof(long);
                        return Math.Abs(longValue);
                    }
                    else if (float.TryParse(str, out float floatValue))
                    {
                        outType = typeof(float);
                        return Math.Abs(floatValue);
                    }
                    else if (double.TryParse(str, out double doubleValue))
                    {
                        outType = typeof(double);
                        return Math.Abs(doubleValue);
                    }
                    else if (decimal.TryParse(str, out decimal decimalValue))
                    {
                        outType = typeof(decimal);
                        return Math.Abs(decimalValue);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Error converting data type '{type}' to float.");
                    }
                }
            }
        }
        // Thr argument value should between -1.0 and 1.0
        public static object Acos(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            var value = Convert.ToDouble(o);
            if (value.CompareTo(1.0) > 0 || value.CompareTo(-1.0) < 0)
            {
                throw new InvalidOperationException("The parameter value should between -1.0 and 1.0 in acos function.");
            }
            return Math.Acos(value);
        }
        public static object Asin(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            var value = Convert.ToDouble(o);
            if (value.CompareTo(1.0) > 0 || value.CompareTo(-1.0) < 0)
            {
                throw new InvalidOperationException("The parameter value should between -1.0 and 1.0 in asin function.");
            }
            return Math.Asin(value);
        }
        public static object Atan(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                var value = Convert.ToDouble(o);
                return Math.Atan(value);
            }
        }
        public static object Atn2(object o1, object o2)
        {
            if (IsNull(o1) || IsNull(o2))
            {
                return DBNull.Value;
            }
            return Math.Atan2(Convert.ToDouble(o1), Convert.ToDouble(o2));
        }
        public static object Ceiling(object o, out Type outType)
        {
            outType = typeof(double);
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                try
                {
                    return Math.Ceiling(Convert.ToDouble(o));
                }
                catch
                {
                    outType = typeof(decimal);
                    return Math.Ceiling(Convert.ToDecimal(o));
                }
            }
        }
        public static object Cos(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return Math.Cos(Convert.ToDouble(o));
            }
        }
        public static object Cot(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                double value = Convert.ToDouble(o);
                if (0 == value.CompareTo(0.0d))
                {
                    throw new InvalidOperationException("The parameter value should not be 0 in cot function.");
                }
                else
                {
                    return 1.0d / Math.Tan(value);
                }
            }
        }
        public static object Degrees(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }

            var value = Convert.ToDouble(o);
            if (0 == value.CompareTo(0.0d))
            {
                return 0;
            }
            else
            {
                return value * (180.0d / Math.PI);
            }
        }
        public static object Radians(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return Convert.ToDouble(o) * (Math.PI / 180);
            }
        }
        public static object Exp(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return Math.Exp(Convert.ToDouble(o));
            }
        }
        public static object Floor(object o, out Type outType)
        {
            outType = typeof(double);
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                try
                {
                    return Math.Floor(Convert.ToDouble(o));
                }
                catch
                {
                    outType = typeof(decimal);
                    return Math.Floor(Convert.ToDecimal(o));
                }
            }
        }
        public static object Log(object o, double? newBase = null)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            var value = Convert.ToDouble(o);
            if (value.CompareTo(0.0d) <= 0)
            {
                throw new InvalidOperationException("The parameter value should be more than 0 in log function.");
            }
            if (!newBase.HasValue)
            {
                return Math.Log(value);
            }
            else
            {
                if (newBase.Value.CompareTo(0.0d) <= 0)
                {
                    throw new InvalidOperationException("The parameter value should be more than 0 in log function.");
                }
                return Math.Log(value, newBase.Value);
            }
        }
        public static object Log10(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            var value = Convert.ToDouble(o);
            if (value.CompareTo(0.0d) <= 0)
            {
                throw new InvalidOperationException("The parameter value should be more than 0 in log10 function.");
            }
            return Math.Log10(value);
        }
        public static object PI()
        {
            return Math.PI;
        }
        public static object Power(object x, object y)
        {
            if (IsNull(x) || IsNull(y))
            {
                return DBNull.Value;
            }
            else
            {
                return Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y));
            }
        }
        public static object Rand(int? seed = null)
        {
            Random r = seed.HasValue ? new Random(seed.Value) : new Random();
            return r.NextDouble();
        }
        public static object Round(object o, int len, out Type outType, int function = 1)
        {
            outType = typeof(double);
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            MidpointRounding mr = (0 == function) ? MidpointRounding.ToEven : MidpointRounding.AwayFromZero;
            try
            {
                return Math.Round(Convert.ToDouble(o), len, mr);
            }
            catch
            {
                outType = typeof(decimal);
                return Math.Round(Convert.ToDecimal(o), len, mr);
            }
        }
        public static object Sign(object o, out Type outType)
        {
            outType = typeof(double);
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                var type = o.GetType();
                outType = type;
                if (type == typeof(sbyte))
                {
                    return Math.Sign(Convert.ToSByte(o));
                }
                else if (type == typeof(short))
                {
                    return Math.Sign(Convert.ToInt16(o));
                }
                else if (type == typeof(int))
                {
                    return Math.Sign(Convert.ToInt32(o));
                }
                else if (type == typeof(long))
                {
                    return Math.Sign(Convert.ToInt64(o));
                }
                else if (type == typeof(float))
                {
                    return Math.Sign(Convert.ToSingle(o));
                }
                else if (type == typeof(double))
                {
                    return Math.Sign(Convert.ToDouble(o));
                }
                else if (type == typeof(decimal))
                {
                    return Math.Sign(Convert.ToDecimal(o));
                }
                else
                {
                    var str = o.ToString();
                    if (int.TryParse(str, out int intValue))
                    {
                        outType = typeof(int);
                        return Math.Sign(intValue);
                    }
                    else if (long.TryParse(str, out long longValue))
                    {
                        outType = typeof(long);
                        return Math.Sign(longValue);
                    }
                    else if (float.TryParse(str, out float floatValue))
                    {
                        outType = typeof(float);
                        return Math.Sign(floatValue);
                    }
                    else if (double.TryParse(str, out double doubleValue))
                    {
                        outType = typeof(double);
                        return Math.Sign(doubleValue);
                    }
                    else if (decimal.TryParse(str, out decimal decimalValue))
                    {
                        outType = typeof(decimal);
                        return Math.Sign(decimalValue);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Error converting data type '{type}' to float.");
                    }
                }
            }
        }
        public static object Sin(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return Math.Sin(Convert.ToDouble(o));
            }
        }
        public static object Sqrt(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                var value = Convert.ToDouble(o);
                if (value.CompareTo(0.0d) < 0)
                {
                    throw new InvalidOperationException("The parameter value should be more than or equal to 0 in sqrt function.");
                }
                return Math.Sqrt(Convert.ToDouble(o));
            }
        }
        public static object Square(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return Math.Pow(Convert.ToDouble(o), 2);
            }
        }
        public static object Tan(object o)
        {
            if (IsNull(o))
            {
                return DBNull.Value;
            }
            else
            {
                return Math.Tan(Convert.ToDouble(o));
            }
        }
        #endregion // MathematicsFunctions

        #region Compare
        public static bool Compare(string oprtor, object o1, object o2)
        {
            var type1 = o1.GetType();
            var type2 = o2.GetType();
            if (type1 != type2)
            {
                throw new InvalidOperationException("Data type do not match.");
            }

            switch (oprtor)
            {
                case SqlConstants.GT:
                    return GreaterThan(o1, o2);
                case SqlConstants.GTE:
                    return GreaterThanOrEqual(o1, o2);
                case SqlConstants.LT:
                    return LessThan(o1, o2);
                case SqlConstants.LTE:
                    return LessThanOrEqual(o1, o2);
                case SqlConstants.EQ:
                    return Equal(o1, o2, type1);
                case SqlConstants.NEQ:
                case SqlConstants.NEQ2:
                    return NotEqual(o1, o2, type1);
                default:
                    throw new InvalidOperationException($"Invalid operator '{oprtor}' detected.");
            }
        }
        public static bool GreaterThan(object o1, object o2)
        {
            return ((IComparable)o1).CompareTo(o2) > 0;
        }
        public static bool GreaterThanOrEqual(object o1, object o2)
        {
            return ((IComparable)o1).CompareTo(o2) >= 0;
        }
        public static bool LessThan(object o1, object o2)
        {
            return ((IComparable)o1).CompareTo(o2) < 0;
        }
        public static bool LessThanOrEqual(object o1, object o2)
        {
            return ((IComparable)o1).CompareTo(o2) <= 0;
        }
        public static bool Equal(object o1, object o2, Type type)
        {
            if (type == typeof(string))
            {
                string str1 = Convert.ToString(o1), str2 = Convert.ToString(o2);
                return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return ((IComparable)o1).CompareTo(o2) == 0;
            }
        }
        public static bool NotEqual(object o1, object o2, Type type)
        {
            return !Equal(o1, o2, type);
        }
        #endregion // Compare

        #region Calculate
        //public static dynamic Calculate(string oprtor, object o1, object o2)
        //{
        //    dynamic arg1 = (dynamic)o1, arg2 = (dynamic)o2;
        //    switch (oprtor)
        //    {
        //        case SqlConstants.ADD:
        //            return arg1 + arg2;
        //        case SqlConstants.SUB:
        //            return arg1 - arg2;
        //        case SqlConstants.MUL:
        //            return arg1 * arg2;
        //        case SqlConstants.DIV:
        //            if (arg2 == 0)
        //            {
        //                throw new DivideByZeroException();
        //            }
        //            return arg1 / arg2;
        //        case SqlConstants.MOD:
        //            return arg1 % arg2;
        //        default:
        //            throw new InvalidOperationException($"Invalid operator '{oprtor}' detected.");
        //    }
        //}
        #endregion // Calculate

        #region Null/NotNull
        public static bool IsNull(object o)
        {
            return null == o || DBNull.Value == o;
        }
        public static bool IsNotNull(object o)
        {
            return !IsNull(o);
        }
        #endregion // Null/NotNull

        #region Like/NotLike
        public static bool Like(object o, string v)
        {
            if (IsNull(o) || null == v)
            {
                return false;
            }

            string src = Convert.ToString(o);
            if (v.StartsWith("%") && v.EndsWith("%"))
            {
                v = v.Substring(1, v.Length - 2);
                return src.IndexOf(v, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            else if (v.StartsWith("%"))
            {
                return src.EndsWith(v.Substring(1));
            }
            else if (v.EndsWith("%"))
            {
                return src.StartsWith(v.Substring(0, v.Length - 1));
            }
            else
            {
                return false;
            }
        }
        public static bool NotLike(object o, string v)
        {
            if (IsNull(o) || null == v)
            {
                return false;
            }
            return !Like(o, v);
        }
        #endregion // Like/NotLike

        #region In/NotIn
        //public static bool In(object o1, object o2)
        //{
        //    var values = (o2 as IList<IntermediateData>).Select(d => d.Value).ToList();
        //    return values.Contains(o1);
        //}
        //public static bool NotIn(object o1, object o2)
        //{
        //    return !In(o1, o2);
        //}
        #endregion // In/NotIn
    }
}
