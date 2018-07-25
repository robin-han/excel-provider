using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlParse
{
    internal static class GCSqlVisitorCheck
    {
        internal static System.Collections.ObjectModel.ReadOnlyDictionary<string, ParametersInfo> FunctionsDict
        {
            get
            {
                return functionsDict;
            }
        }

        private static readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, ParametersInfo> functionsDict = new System.Collections.ObjectModel.ReadOnlyDictionary<string, ParametersInfo>
        (
            new Dictionary<string, ParametersInfo>(StringComparer.InvariantCultureIgnoreCase)
            {
                     //STRING
                    { "CONCAT", new ParametersInfo(2,254) },
                    { "FORMAT", new ParametersInfo(2) },
                    { "INDEXOF", new ParametersInfo(2,3) },
                    { "ISNULL", new ParametersInfo(2) },
                    { "LEFT", new ParametersInfo(2) },
                    { "LEN", new ParametersInfo(1) },
                    { "LOWER", new ParametersInfo(1) },
                    { "LTRIM", new ParametersInfo(1) },
                    { "REPLACE", new ParametersInfo(3) },
                    { "REPLICATE", new ParametersInfo(2) },
                    { "REVERSE", new ParametersInfo(1) },
                    { "RIGHT", new ParametersInfo(2) },
                    { "RTRIM", new ParametersInfo(1) },
                    { "SUBSTRING", new ParametersInfo(3) },
                    { "TOSTRING", new ParametersInfo(1) },
                    { "TRIM", new ParametersInfo(1) },
                    { "UPPER", new ParametersInfo(1) },
                    //DATETIME
                    { "DATEADD", new ParametersInfo(3,4, new DatepartDescriptor()) },
                    { "DATEDIFF", new ParametersInfo(3, new DatepartDescriptor()) },
                    { "DATEFROMPARTS", new ParametersInfo(3) },
                    { "DATEPART", new ParametersInfo(2,3, new DatepartDescriptor()) },
                    { "DATETIMEFROMPARTS", new ParametersInfo(5,7) },
                    { "EOMONTH", new ParametersInfo(1,2) },
                    { "GETDATE", new ParametersInfo(0) },
                    { "GETUTCDATE", new ParametersInfo(0) },
                    //MATHEMATICS
                    { "ABS", new ParametersInfo(1) },
                    { "ACOS", new ParametersInfo(1) },
                    { "ASIN", new ParametersInfo(1) },
                    { "ATAN", new ParametersInfo(1) },
                    { "ATN2", new ParametersInfo(2) },
                    { "CEILING", new ParametersInfo(1) },
                    { "COS", new ParametersInfo(1) },
                    { "COT", new ParametersInfo(1) },
                    { "DEGREES", new ParametersInfo(1) },
                    { "EXP", new ParametersInfo(1) },
                    { "FLOOR", new ParametersInfo(1) },
                    { "LOG", new ParametersInfo(1) },
                    { "LOG10", new ParametersInfo(1) },
                    { "PI", new ParametersInfo(0) },
                    { "POWER", new ParametersInfo(2) },
                    { "RADIANS", new ParametersInfo(1) },
                    { "RAND", new ParametersInfo(0,1) },
                    { "ROUND", new ParametersInfo(2,3) },
                    { "SIGN", new ParametersInfo(1) },
                    { "SIN", new ParametersInfo(1) },
                    { "SQRT", new ParametersInfo(1) },
                    { "SQUARE", new ParametersInfo(1) },
                    { "TAN", new ParametersInfo(1) },
                    //AGGRAGATE
                    { "COUNT", new ParametersInfo(0,1) },
                    { "COUNT_DISTINCT", new ParametersInfo(1) },
                    { "AVG", new ParametersInfo(1) },
                    { "MIN", new ParametersInfo(1) },
                    { "MAX", new ParametersInfo(1) },
                    { "SUM", new ParametersInfo(1) },
            }
        );
    }

    class ParametersInfo
    {
        public ParametersInfo(int count)
        {
            Initialize(count, count, null);
        }
        public ParametersInfo(int count, params IParameterDescriptor[] descriptor)
        {
            Initialize(count, count, descriptor);
        }

        public ParametersInfo(int min, int max)
        {
            Initialize(min, max, null);
        }

        public ParametersInfo(int min, int max, params IParameterDescriptor[] descriptor)
        {
            Initialize(min, max, descriptor);
        }

        private void Initialize(int min, int max, IParameterDescriptor[] descriptor)
        {
            Min = min;
            Max = max;
            Descriptors = descriptor;
        }

        public IParameterDescriptor[] Descriptors { get; set; }

        public int Min { get; set; }
        public int Max { get; set; }
    }

    interface IParameterDescriptor
    {
        bool CheckParameter(object parameter);
        ParameterDescriptorType DescriptorType { get; }
    }

    enum ParameterDescriptorType
    {
        Normal,
        Enum,
    }

    abstract internal class EnumParamterDescriptor : IParameterDescriptor
    {
        public abstract bool CheckParameter(object parameter);

        public ParameterDescriptorType DescriptorType
        {
            get
            {
                return ParameterDescriptorType.Enum;
            }
        }

        protected bool CheckEnumParameter(object parameter, HashSet<string> validValueList)
        {
            var str = parameter as string;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return validValueList.Contains(str);
        }

    }

    class DatepartDescriptor : EnumParamterDescriptor
    {
        static readonly HashSet<string> _validPrameters = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                "year", "yy", "yyyy",
                "quarter", "qq", "q",
                "month", "mm", "m",
                "dayofyear", "dy", "y",
                "day", "dd", "d",
                "week", "wk", "ww",
                "weekday", "dw",
                "hour", "hh",
                "minute", "mi", "n",
                "second", "ss", "s",
                "millisecond", "ms",
        };

        public override bool CheckParameter(object parameter)
        {
            return CheckEnumParameter(parameter, _validPrameters);
        }
    }

}
