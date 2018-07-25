using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlExecutor
{
    public enum EDatePart
    {
        Unknown,
        Year,
        Quarter,
        Month,
        DayOfYear,
        Day,
        Week,
        WeekDay,
        Hour,
        Minute,
        Second,
        MilliSecond
    }

    public class DateParts
    {
        private static readonly IDictionary<string, EDatePart> _dateparts = new Dictionary<string, EDatePart>
        {
            { "year", EDatePart.Year },
            { "yyyy", EDatePart.Year },
            { "yy", EDatePart.Year },
            { "quarter", EDatePart.Quarter },
            { "qq", EDatePart.Quarter },
            { "q", EDatePart.Quarter },
            { "month", EDatePart.Month },
            { "mm", EDatePart.Month },
            { "m", EDatePart.Month },
            { "dayofyear", EDatePart.DayOfYear },
            { "dy", EDatePart.DayOfYear },
            { "y", EDatePart.DayOfYear },
            { "day", EDatePart.Day },
            { "dd", EDatePart.Day },
            { "d", EDatePart.Day },
            { "week", EDatePart.Week },
            { "wk", EDatePart.Week },
            { "ww", EDatePart.Week },
            { "weekday", EDatePart.WeekDay },
            { "dw", EDatePart.WeekDay },
            { "hour", EDatePart.Hour },
            { "hh", EDatePart.Hour },
            { "minute", EDatePart.Minute },
            { "mi", EDatePart.Minute },
            { "n", EDatePart.Minute },
            { "second", EDatePart.Second },
            { "ss", EDatePart.Second },
            { "s", EDatePart.Second },
            { "millisecond", EDatePart.MilliSecond },
            { "ms", EDatePart.MilliSecond }
        };

        public static EDatePart GetDatePart(string str)
        {
            if (string.IsNullOrEmpty(str) || !_dateparts.Keys.Contains(str.ToLower()))
            {
                return EDatePart.Unknown;
            }
            return _dateparts[str.ToLower()];
        }
    }
}
