//The follow code from spreadx to detect a format's type

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    internal enum NumberFormatType
    {
        /// <summary>
        /// [0] Formats the data using the general formatter.
        /// </summary>
        General = 0,

        /// <summary>
        /// [1] Formats the data using the number formatter.
        /// </summary>
        Number = 1,

        /// <summary>
        /// [2] Formats the data using the date-time formatter.
        /// </summary>
        DateTime = 2,

        /// <summary>
        /// [3] Formats the data using the text formatter.
        /// </summary>
        Text = 3,
    }

    internal enum FormatMode
    {
        /// <summary>
        /// [0] Indicates whether to format the value with the Excel-compatible format string.
        /// </summary>
        CustomMode = 0,

        /// <summary>
        /// [1] Indicates whether to format the value with the standard date-time format.
        /// </summary>
        StandardDateTimeMode = 1,

        /// <summary>
        /// [2] Indicates whether to format the value with the standard numeric format.
        /// </summary>
        StandardNumericMode = 2,
    }

    internal interface IFormatter
    {
    }

    internal class GeneralFormatter
    {
        private Collection<IFormatter> formatters = null;
        private FormatMode formatModeType = FormatMode.CustomMode;
        private bool isSingleFormatterInfo = true;

        private bool isDefault = true;
        private string formatCached = null;
        private bool isConstructed = false;

        private CultureInfo customerCulture = null;

        internal GeneralFormatter(string format) : this(FormatMode.CustomMode, format)
        {
        }
        private GeneralFormatter(FormatMode formatMode, string format)
        {
            if (format == null)
            {
                throw new ArgumentException(SharedResourceStrings.FormatIllegalFormatError);
            }

            this.formatCached = format;
            this.formatModeType = formatMode;
            this.isDefault = this.formatCached.ToLower() == NumberFormatBase.General.ToLower();
            this.isConstructed = false;


            this.customerCulture = System.Globalization.CultureInfo.CurrentCulture;
        }


        public NumberFormatType FormatType
        {
            get
            {
                this.Init();

                CustomNumberFormat formatInfo = this.formatters[0] as CustomNumberFormat;
                if (formatInfo.Formatter is NumberFormatDigital)
                {
                    return NumberFormatType.Number;
                }
                else if (formatInfo.Formatter is NumberFormatDateTime)
                {
                    return NumberFormatType.DateTime;
                }
                else if (formatInfo.Formatter is NumberFormatText)
                {
                    return NumberFormatType.Text;
                }
                return NumberFormatType.General;
            }
        }

        public CultureInfo CustomerCulture
        {
            get
            {
                return this.customerCulture;
            }
            set
            {
                this.customerCulture = value;
            }
        }

        private void Init()
        {
            if (!isConstructed)
            {
                this.isConstructed = true;
                switch (this.formatModeType)
                {
                    case FormatMode.CustomMode:
                        this.InitExcelCompatibleMode(this.formatCached);
                        break;
                    case FormatMode.StandardDateTimeMode:
                        //this.InitStandardDateTimeMode(this.formatCached);
                        break;
                    case FormatMode.StandardNumericMode:
                        //this.InitStandardNumericMode(this.formatCached);
                        break;
                }
            }
        }

        private void InitExcelCompatibleMode(string format)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            //if (format == string.Empty)
            //{
            //    throw new FormatException("format is illegal.");
            //}

            this.formatters = new Collection<IFormatter>();
            if (this.isDefault)
            {
                this.formatters.Add(new CustomNumberFormat(System.Globalization.CultureInfo.CurrentCulture));
            }
            else
            {
                //this.isDefault = format.ToLower() == NumberFormatBase.General.ToLower();
                this.isSingleFormatterInfo = !format.Contains(DefaultTokens.FormatSeparator.ToString());
                //format = format.TrimEnd(DefaultTokens.FormatSeparator);
                string[] items = format.Split(DefaultTokens.FormatSeparator);
                if (items == null)
                {
                    throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                }

                if (items.Length < 1 || items.Length > 5)
                {
                    throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                }

                int count = 0;
                foreach (string item in items)
                {
                    count++;
                    if (count > 4)
                    {
                        break;
                    }

                    CustomNumberFormat formatItem = new CustomNumberFormat(item, this.CustomerCulture);
                    if (formatItem != null)
                    {
                        this.formatters.Add(formatItem);
                    }
                }

                if (this.PositiveExpression == null)
                {
                    throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                }
            }
        }

        [DefaultValue(null)]
        internal CustomNumberFormat PositiveExpression
        {
            get
            {
                this.Init();
                if (formatters != null && formatters.Count > 0)
                {
                    return this.formatters[0] as CustomNumberFormat;
                }

                return null;
            }
        }

    }

    internal sealed class CustomNumberFormat : IFormatter
    {
        #region Fields
        private NumberFormatBase numberFormat;

        private string formatCached;

        private StringBuilder formatStringTemplate = null;

        private static readonly string[] CurrencySymbols = new string[]
                      {
                            "$",
                            "Kč",
                            "kr\\.",
                            "Ft",
                            "kr",
                            "zł",
                            "R$",
                            "fr\\.",
                            "RON",
                            "р\\.",
                            "kn",
                            "EUR",
                            "Lek",
                            "Rs",
                            "Rp",
                            "Ls",
                            "Lt",
                            "manat",
                            "R",
                            "RM",
                            //"S",
                            "m\\.",
                            "so'm",
                            "ETB",
                            "DZD",
                            "PhP",
                            "XOF",
                            "$b",
                            "ERN",
                            "RWF",
                            "NT$",
                            "Kč",
                            "Ft",
                            "zł",
                            "RON",
                            "kn",
                            "Lek",
                            "Rs",
                            "Rp",
                            "manat",
                            "Q",
                            "din\\.",
                            //"P",
                            "HK$",
                            "KM",
                            "MOP",
                            "CHF",
                            "RD$",
                            "J$",
                            "Bs\\.F\\.",
                            "EC$",
                            "BZ$",
                            "S\\/\\.",
                            "TT$",
                            "$U",
                            "L\\.",
                            "C$",
                            "B\\/\\."
                      };

        #endregion

        #region Constructor


        public CustomNumberFormat()
        {
            this.formatCached = NumberFormatBase.General;
            this.numberFormat = new NumberFormatGeneral();

        }

        public CustomNumberFormat(CultureInfo culture)
        {
            this.formatCached = NumberFormatBase.General;
            this.numberFormat = new NumberFormatGeneral();
            this.numberFormat.Culture = culture;
        }

        public CustomNumberFormat(string format, CultureInfo culture)
        {
            this.Init(format, culture);
        }

        /// <summary>
        /// Initializes the specified format.
        /// </summary>
        /// <param name="format">Format String</param>
        /// <param name="culture">The culture.</param> 
        private void Init(string format, CultureInfo culture)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            //if (format == string.Empty)
            //{
            //  throw new FormatException("format is illegal.");
            //}

            //this.conditionFormatPart = null;
            //this.colorFormatPart = null;
            //this.localeIDFormatPart = null;
            //this.dbNumberFormatPart = null;
            this.formatCached = format;

            StringBuilder contentToken = null;
            StringBuilder token = null;
            bool isInFormatPart = false;
            bool inEscape = false;
            List<ABSTimeFormatPart> absTimePart = new List<ABSTimeFormatPart>();
            for (int index = 0; index < format.Length; index++)
            {
                char c = format[index];
                if (c == DefaultTokens.LeftSquareBracket && !inEscape)
                {
                    if (isInFormatPart)
                    {
                        throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                    }
                    else
                    {
                        if (token != null)
                        {
                            if (contentToken == null)
                            {
                                contentToken = new StringBuilder();
                            }

                            contentToken.Append(token.ToString());
                            if (formatStringTemplate == null)
                                formatStringTemplate = new StringBuilder();
                            formatStringTemplate.Append(token.ToString());
                            token = null;
                        }

                        token = new StringBuilder();
                        token.Append(c);
                    }
                    isInFormatPart = true;
                }
                else if (c == DefaultTokens.RightSquareBracket && !inEscape)
                {
                    if (isInFormatPart)
                    {
                        if (token != null)
                        {
                            if (token == null)
                            {
                                token = new StringBuilder();
                            }

                            token.Append(c);
                            string part = token.ToString();
                            FormatPartBase partObject = FormatPartBase.Create(part) as FormatPartBase;
                            if (partObject != null && !(partObject is ABSTimeFormatPart))
                            {
                                //this.AddPart(partObject);
                            }
                            else
                            {
                                if (partObject is ABSTimeFormatPart)
                                {
                                    absTimePart.Add(partObject as ABSTimeFormatPart);
                                    if (contentToken == null)
                                    {
                                        contentToken = new StringBuilder();
                                    }

                                    contentToken.Append(token.ToString());
                                    if (formatStringTemplate == null)
                                        formatStringTemplate = new StringBuilder();
                                    formatStringTemplate.Append(token.ToString());
                                }
                                else
                                {
                                    throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                                }
                            }

                            token = null;
                        }
                        else
                        {
                            throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                        }
                    }
                    else
                    {
                        throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                    }

                    isInFormatPart = false;
                }
                else
                {
                    if (token == null)
                    {
                        token = new StringBuilder();
                    }

                    token.Append(c);
                }

                if (c == DefaultTokens.ReverseSolidusSign)
                {
                    inEscape = inEscape ? false : true;
                }
                else
                {
                    inEscape = false;
                }
            }

            if (token != null)
            {
                if (isInFormatPart)
                {
                    throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
                }
                else
                {
                    if (contentToken == null)
                    {
                        contentToken = new StringBuilder();
                    }

                    contentToken.Append(token.ToString());
                    if (formatStringTemplate == null)
                        formatStringTemplate = new StringBuilder();
                    formatStringTemplate.Append(token.ToString());
                }
            }

            string content = contentToken != null ? contentToken.ToString() : string.Empty;
            if (NumberFormatGeneral.EvaluateFormat(content))
            {
                //this.numberFormat = new NumberFormatGeneral(content, this.LocaleIDFormatPart, this.dbNumberFormatPart, culture);
                this.numberFormat = new NumberFormatGeneral();
            }
            else if (NumberFormatDateTime.EvaluateFormat(content) && !ContainsCurrencySymbol(content))
            {
                //ABSTimeFormatPart[] absPartsArray = absTimePart.Count > 0 ? absTimePart.ToArray() : null;
                //this.numberFormat = new NumberFormatDateTime(content, absPartsArray, this.LocaleIDFormatPart, this.dbNumberFormatPart, culture);
                this.numberFormat = new NumberFormatDateTime();
            }
            else if (NumberFormatDigital.EvaluateFormat(content))
            {
                //this.numberFormat = new NumberFormatDigital(format, this.LocaleIDFormatPart, this.dbNumberFormatPart, culture);
                this.numberFormat = new NumberFormatDigital();
            }
            else if (NumberFormatText.EvaluateFormat(content))
            {
                //this.numberFormat = new NumberFormatText(format, this.LocaleIDFormatPart, this.dbNumberFormatPart, culture);
                this.numberFormat = new NumberFormatDigital();
            }
            else
            {
                throw new FormatException(SharedResourceStrings.FormatIllegalFormatError);
            }
        }

        [DefaultValue(null)]
        public NumberFormatBase Formatter
        {
            get
            {
                return this.numberFormat;
            }
        }

        #region Internal Methods
        private bool ContainsCurrencySymbol(string format)
        {
            foreach (var item in CurrencySymbols)
            {
                if (format.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }

    internal abstract class NumberFormatBase
    {
        internal readonly static string General = "General";

        private CultureInfo culture = null;

        protected NumberFormatBase()
        {

        }

        public virtual CultureInfo Culture
        {
            get
            {
                return this.culture;
            }
            internal set
            {
                this.culture = value;
            }
        }

        internal static bool ContainsKeywords(string format, string[] keywords)
        {
            if (format == null || format == string.Empty)
            {
                return false;
            }

            StringBuilder stringOnlyKeywords = new StringBuilder();
            bool inComments = false;
            char last = '\0';
            for (int n = 0; n < format.Length; n++)
            {
                char c = format[n];
                if (c == '\"')
                {
                    inComments = !inComments;
                }
                else
                {
                    if (!inComments)
                    {
                        if (c != DefaultTokens.UnderLine && last != DefaultTokens.UnderLine)
                        {
                            stringOnlyKeywords.Append(c);
                        }
                    }
                }

                last = c;
            }

            string formatTemp = stringOnlyKeywords.ToString().ToLower();
            foreach (string keyword in keywords)
            {
                if (formatTemp.Contains(keyword))
                {
                    return true;
                }
            }

            return false;
        }
    }

    internal sealed class NumberFormatGeneral : NumberFormatBase
    {
        private string fullFormatString = null;

        /// <summary>
        /// Creates a new normal general number format.
        /// </summary>
        public NumberFormatGeneral() : base()
        {
            this.fullFormatString = NumberFormatBase.General;
        }

        /// <summary>
        /// Determines whether the format string is valid.
        /// </summary>
        /// <param name="format">The token to evaluate.</param>
        /// <returns>
        /// 	<c>true</c> if the specified format contains the text; otherwise, <c>false</c>.
        /// </returns>
        public static bool EvaluateFormat(string format)
        {
            if (format == null || format == string.Empty)
            {
                return false;
            }

            return NumberFormatBase.ContainsKeywords(format, new string[] { General.ToLower() });
        }

    }

    internal sealed class NumberFormatDateTime : NumberFormatBase
    {
        #region Fields

        ///// <summary>
        ///// the default converter.
        ///// </summary>
        //private static readonly INumberStringConverter defaultNumberStringConverter = new DefaultDateTimeNumberStringConverter();

        ///// <summary>
        ///// Custom format for short year in excel.
        ///// </summary>
        //private readonly static string YearSingleDigit = "y";

        /// <summary>
        /// Custom format for short year in excel.
        /// </summary>
        internal readonly static string YearTwoDigit = "yy";

        /// <summary>
        /// Custom format for long year in excel.
        /// </summary>
        internal readonly static string YearFourDigit = "yyyy";

        /// <summary>
        /// Custom format for short month in excel.
        /// </summary>
        private readonly static string MonthSingleDigit = "m";

        /// <summary>
        /// Custom format for long month in excel.
        /// </summary>
        private readonly static string MonthTwoDigit = "mm";

        /// <summary>
        /// Custom format for month abbreviation name in excel.
        /// </summary>
        private readonly static string MonthAbbreviation = "mmm";

        /// <summary>
        /// Custom format for month unabbreviated name in excel.
        /// </summary>
        private readonly static string MonthUnabbreviated = "mmmm";

        /// <summary>
        /// Custom format for the first char of month name in excel.
        /// </summary>
        private readonly static string MonthJD = "mmmmm";

        /// <summary>
        /// Custom format for short day in excel.
        /// </summary>
        private readonly static string DaySingleDigit = "d";

        /// <summary>
        /// Custom format for long day in excel.
        /// </summary>
        private readonly static string DayTwoDigit = "dd";

        /// <summary>
        /// Custom format for week day abbreviation name in excel.
        /// </summary>
        private readonly static string DayWeekDayAbbreviation = "aaa";

        /// <summary>
        /// Custom format for week day unabbreviated name in excel.
        /// </summary>
        private readonly static string DayWeekDayUnabbreviated = "aaaa";

        /// <summary>
        /// Custom format for short hours in excel.
        /// </summary>
        private readonly static string HoursSingleDigit = "h";

        /// <summary>
        /// Custom format for long hours in excel.
        /// </summary>
        private readonly static string HoursTwoDigit = "hh";

        /// <summary>
        /// Custom format for short minute in excel.
        /// </summary>
        private readonly static string MinuteSingleDigit = "m";

        /// <summary>
        /// Custom format for long minute in excel.
        /// </summary>
        private readonly static string MinuteTwoDigit = "mm";

        /// <summary>
        /// Custom format for short second in excel.
        /// </summary>
        private readonly static string SecondSingleDigit = "s";

        /// <summary>
        /// Custom format for long second in excel.
        /// </summary>
        private readonly static string SecondTwoDigit = "ss";

        ///// <summary>
        ///// Custom format for short sub-second in excel.
        ///// </summary>
        //private readonly static string SubSecondSingleDigit = ".0";

        ///// <summary>
        ///// Custom format for long sub-second in excel.
        ///// </summary>
        //private readonly static string SubSecondTwoDigit = ".00";

        ///// <summary>
        ///// Custom format for full sub-second in excel.
        ///// </summary>
        //private readonly static string SubSecondThreeDigit = ".000";

        ///// <summary>
        ///// Custom format for era year in excel.
        ///// </summary>
        //private readonly static string EraYear = "e";

        ///// <summary>
        ///// Custom format for default AM/PM symbol in excel.
        ///// </summary>
        //private readonly static string AMPMTwoDigit = "AM/PM";

        ///// <summary>
        ///// Custom format for short AM/PM symbol in excel.
        ///// </summary>
        //private readonly static string AMPMSingleDigit = "A/P";

        ///// <summary>
        ///// Custom format for short year in string.format.
        ///// </summary>
        //private readonly static string StandardYearSingleDigit = "%y";

        ///// <summary>
        ///// Custom format for short month in string.format.
        ///// </summary>
        //private readonly static string StandardMonthSingleDigit = "%M";

        ///// <summary>
        ///// Custom format for long month in string.format.
        ///// </summary>
        //private readonly static string StandardMonthTwoDigit = "MM";

        ///// <summary>
        ///// Custom format for month abbreviation name in string.format.
        ///// </summary>
        //private readonly static string StandardMonthAbbreviation = "MMM";

        ///// <summary>
        ///// Custom format for month unabbreviated name in string.format.
        ///// </summary>
        //private readonly static string StandardMonthUnabbreviated = "MMMM";

        ///// <summary>
        ///// Custom format for AM/PM symbol in string.format.
        ///// </summary>
        //private readonly static string StandardAMPMSingleDigit = "tt";

        ///// <summary>
        ///// Custom format for short minute in string.format.
        ///// </summary>
        //private readonly static string StandardMinuteSingleDigit = "%m";

        ///// <summary>
        ///// Custom format for short hour in string.format.
        ///// </summary>
        //private readonly static string StandardHourSingleDigit = "H";

        ///// <summary>
        ///// Custom format for short hour in string.format.
        ///// </summary>
        //private readonly static string StandardHourTwoDigit = "HH";

        ///// <summary>
        ///// Custom format for short second in string.format.
        ///// </summary>
        //private readonly static string StandardSecondSingleDigit = "%s";

        /////// <summary>
        /////// Custom format for long second in string.format.
        /////// </summary>
        ////private readonly static string StandardSecondTwoDigit = "ss";

        ///// <summary>
        ///// Custom format for short sub second in string.format.
        ///// </summary>
        //private readonly static string StandardSubSecondSingleDigit = ".f";

        ///// <summary>
        ///// Custom format for long sub second in string.format.
        ///// </summary>
        //private readonly static string StandardSubSecondTwoDigit = ".ff";

        ///// <summary>
        ///// Custom format for full sub second in string.format.
        ///// </summary>
        //private readonly static string StandardSubSecondThreeDigit = ".fff";

        ///// <summary>
        ///// Custom format for week day abbreviation name in excel.
        ///// </summary>
        //private readonly static string StandardDayWeekDayAbbreviation = "ddd";

        ///// <summary>
        ///// Custom format for week day unabbreviated name in excel.
        ///// </summary>
        //private readonly static string StandardDayWeekDayUnabbreviated = "dddd";

        private readonly static string JanpanAraYearSymbol1 = "ge";

        private readonly static string JanpanAraYearSymbol2 = "g e";

        ///// <summary>
        ///// the placeholder for month JD.
        ///// </summary>
        //private readonly static string PlaceholderMonthJD = DefaultTokens.ReplacePlaceholder + MonthJD;

        /// <summary>
        /// the absolute time.
        /// </summary>
        private readonly static DateTime defaultAbsoluteTime = new DateTime(1899, 12, 30, 0, 0, 0, 0);

        /// <summary>
        /// the date time keyword
        /// </summary>
        private readonly static string[] keywords = new string[]
        {
          YearTwoDigit, YearFourDigit,
          MonthSingleDigit, MonthTwoDigit, MonthAbbreviation, MonthUnabbreviated, MonthJD,
          DaySingleDigit, DayTwoDigit, DayWeekDayAbbreviation, DayWeekDayUnabbreviated,
          HoursSingleDigit, HoursTwoDigit,
          MinuteSingleDigit, MinuteTwoDigit,
          SecondSingleDigit , SecondTwoDigit ,
          JanpanAraYearSymbol1,JanpanAraYearSymbol2
        };

        ///// <summary>
        ///// the valid date time format string.
        ///// </summary>
        //private string validDateTimeFormatString = null;

        ///// <summary>
        ///// the format string.
        ///// </summary>
        //private string formatString = null;

        ///// <summary>
        ///// is the format has JD.
        ///// </summary>
        //private bool hasJD = false;

        ///// <summary>
        ///// the absolute time.
        ///// </summary>
        //private DateTime? absoluteTime = null;

        ///// <summary>
        ///// the absTimeParts
        ///// </summary>
        //private ABSTimeFormatPart[] absTimeParts = null;

        ///// <summary>
        ///// whether the year formatting is delay.
        ///// </summary>
        //private bool hasYearDelay = false;

        #endregion

        internal NumberFormatDateTime() : base()
        {
            //
        }

        /// <summary>
        /// Determines whether the format string is valid.
        /// </summary>
        /// <param name="format">The token to evaluate.</param>
        /// <returns>
        /// 	<c>true</c> if the specified format contains the text; otherwise, <c>false</c>.
        /// </returns>
        public static bool EvaluateFormat(string format)
        {
            return NumberFormatBase.ContainsKeywords(format, keywords);
        }
    }

    internal sealed class NumberFormatDigital : NumberFormatBase
    {
        #region Fields

        /// <summary>
        /// the date time keyword
        /// </summary>
        private readonly static string[] keywords = new string[]
    {
      DefaultTokens.Exponential1, DefaultTokens.Exponential2,
      DefaultTokens.NumberSign.ToString(), DefaultTokens.DecimalSeparator, DefaultTokens.NumberGroupSeparator,
      DefaultTokens.PercentSymbol,
      DefaultTokens.Zero.ToString(),
      DefaultTokens.SolidusSign.ToString(),
    };
        #endregion

        public NumberFormatDigital()
         : base()
        {
        }

        /// <summary>
        /// Determines whether the format string is valid.
        /// </summary>
        /// <param name="format">The token to evaluate.</param>
        /// <returns>
        /// 	<c>true</c> if the specified format contains the text; otherwise, <c>false</c>.
        /// </returns>
        public static bool EvaluateFormat(string format)
        {
            return NumberFormatBase.ContainsKeywords(format, keywords);
        }
    }

    internal sealed class NumberFormatText : NumberFormatBase
    {
        #region Constructor

        internal NumberFormatText()
            : base()
        {
            //
        }

        #endregion

        /// <summary>
        /// Determines whether the format string is valid.
        /// </summary>
        /// <param name="format">The token to evaluate.</param>
        /// <returns>
        /// 	<c>true</c> if the specified format contains the text; otherwise, <c>false</c>.
        /// </returns>
        public static bool EvaluateFormat(string format)
        {
            return true;
        }
    }

    internal abstract class FormatPartBase
    {
        protected FormatPartBase(string token)
        {
            //originalToken = token;
        }

        /// <summary>
        /// Creates the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Returns the format part object.</returns>
        public static FormatPartBase Create(string token)
        {
            if (ConditionFormatPart.EvaluateFormat(token))
            {
                return new ConditionFormatPart(token);
            }
            else if (DBNumberFormatPart.EvaluateFormat(token))
            {
                return new DBNumberFormatPart(token);
            }
            else if (LocaleIDFormatPart.EvaluateFormat(token))
            {
                return new LocaleIDFormatPart(token);
            }
            else if (ABSTimeFormatPart.EvaluateFormat(token))
            {
                return new ABSTimeFormatPart(token);
            }
            else if (ColorFormatPart.EvaluateFormat(token))
            {
                return new ColorFormatPart(token);
            }
            else
            {
                return null;
            }
        }
    }

    internal sealed class ConditionFormatPart : FormatPartBase
    {
        public ConditionFormatPart(string token)
           : base(token)
        {
        }
        /// <summary>
        /// Determines whether the format string is valid.
        /// </summary>
        /// <param name="token">The token to evaluate.</param>
        /// <returns>
        /// 	<c>true</c> if the specified format contains the text; otherwise, <c>false</c>.
        /// </returns>
        internal static bool EvaluateFormat(string token)
        {
            if (token == null || token == string.Empty)
            {
                return false;
            }

            string content = DefaultTokens.TrimSquareBracket(token);

            if (content == null || content == string.Empty)
            {
                return false;
            }

            return DefaultTokens.IsOperator(content[0]);
        }
    }

    internal sealed class DBNumberFormatPart : FormatPartBase
    {
        public DBNumberFormatPart(string token)
           : base(token)
        {

        }

        internal static bool EvaluateFormat(string token)
        {
            if (token == null || token == string.Empty)
            {
                return false;
            }

            string content = DefaultTokens.TrimSquareBracket(token);

            if (content == null || content == string.Empty)
            {
                return false;
            }

            if (content.StartsWith("DBNum", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

    }

    internal sealed class LocaleIDFormatPart : FormatPartBase
    {
        public LocaleIDFormatPart(string token)
          : base(token)
        {
        }

        internal static bool EvaluateFormat(string token)
        {
            if (token == null || token == string.Empty)
            {
                return false;
            }

            string content = DefaultTokens.TrimSquareBracket(token);

            if (content == null || content == string.Empty)
            {
                return false;
            }

            return DefaultTokens.IsEquals(content[0], DefaultTokens.Dollar, false);
        }
    }

    internal sealed class ABSTimeFormatPart : FormatPartBase
    {
        #region Private Fields

        /// <summary>
        /// Custom format for absolute hours in Excel (started with 1900-1-1 0:0:0).
        /// </summary>
        private readonly static char HoursABSContent = 'h';

        /// <summary>
        /// Custom format for absolute minutes in Excel (started with 1900-1-1 0:0:0).
        /// </summary>
        private readonly static char MinuteABSContent = 'm';

        /// <summary>
        /// Custom format for absolute seconds in Excel (started with 1900-1-1 0:0:0).
        /// </summary>
        private readonly static char SecondABSContent = 's';
        #endregion

        public ABSTimeFormatPart(string token)
          : base(token)
        {
        }

        internal static bool EvaluateFormat(string token)
        {
            if (token == null || token == string.Empty)
            {
                return false;
            }

            string content = DefaultTokens.TrimSquareBracket(token);

            if (content == null || content == string.Empty)
            {
                return false;
            }

            content = content.ToLower();
            char c = '\0';
            for (int n = 0; n < content.Length; n++)
            {
                if (c == '\0')
                {
                    c = content[n];
                }

                if (c != HoursABSContent && c != MinuteABSContent && c != SecondABSContent)
                {
                    return false;
                }

                if (c != content[n])
                {
                    return false;
                }
            }

            return true;

        }
    }

    internal sealed class ColorFormatPart : FormatPartBase
    {
        public ColorFormatPart(string token)
          : base(token)
        {
        }

        internal static bool EvaluateFormat(string token)
        {
            if (token == null || token == string.Empty)
            {
                return false;
            }

            string content = DefaultTokens.TrimSquareBracket(token);

            if (content == null || content == string.Empty)
            {
                return false;
            }

            if (content.Length < 3)
            {
                return false;
            }

            if (char.IsNumber(token[token.Length - 1]))
            {
                if (token.StartsWith("Color", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (token[0] != token[1])
                    return true;

                return false;
                //try
                //{
                //  ColorHelper.FromName(token);
                //  return true;
                //}
                //catch 
                //{
                //  return false;
                //}
            }
        }
    }

    internal class DefaultTokens
    {
        #region Fields

        /// <summary>
        /// Gets the double quote.
        /// </summary>
        public static readonly char DoubleQuote = '\"';

        /// <summary>
        /// Gets the single quote.
        /// </summary>
        public static readonly char SingleQuote = '\'';

        /// <summary>
        /// Gets the tab character.
        /// </summary>
        public static readonly char Tab = '\t';

        /// <summary>
        /// Gets the left square bracket.
        /// </summary>
        public static readonly char LeftSquareBracket = '[';

        /// <summary>
        /// Gets the right square bracket.
        /// </summary>
        public static readonly char RightSquareBracket = ']';

        /// <summary>
        /// Gets the less than sign.
        /// </summary>
        public static readonly char LessThanSign = '<';

        /// <summary>
        /// Gets the greater than sign.
        /// </summary>
        public static readonly char GreaterThanSign = '>';

        /// <summary>
        /// Gets the equals sign.
        /// </summary>
        public static readonly char EqualsThanSign = '=';

        /// <summary>
        /// Gets the plus sign.
        /// </summary>
        public static readonly char PlusSign = '+';

        /// <summary>
        /// Gets the hyphen minus.
        /// </summary>
        public static readonly char HyphenMinus = '-';

        /// <summary>
        /// Gets the under line.
        /// </summary>
        public static readonly char UnderLine = '_';

        /// <summary>
        /// Gets the left parenthesis.
        /// </summary>
        public static readonly char LeftParenthesis = '(';

        /// <summary>
        /// Gets the right parenthesis.
        /// </summary>
        public static readonly char RightParenthesis = ')';

        /// <summary>
        /// Gets the dollar sign.
        /// </summary>
        public static readonly char Dollar = '$';

        /// <summary>
        /// Gets the comma sign.
        /// </summary>
        public static readonly char Comma = ',';

        /// <summary>
        /// Gets the space character.
        /// </summary>
        public static readonly char Space = (char)0x20;

        /// <summary>
        /// Gets the solidus sign.
        /// </summary>
        public static readonly char SolidusSign = '/';

        /// <summary>
        /// Gets the reverse solidus sign.
        /// </summary>
        public static readonly char ReverseSolidusSign = '\\';

        /// <summary>
        /// Gets the zero digit.
        /// </summary>
        public static readonly char Zero = '0';

        /// <summary>
        /// Gets the question mark.
        /// </summary>
        public static readonly char QuestionMark = '?';

        /// <summary>
        /// Gets the colon sign.
        /// </summary>
        public static readonly char Colon = ':';

        /// <summary>
        /// Gets the semicolon sign.
        /// </summary>
        public static readonly char Semicolon = ';';

        /// <summary>
        /// Gets the sharp sign.
        /// </summary>
        public static readonly char Sharp = '#';

        /// <summary>
        /// Gets the commercial at sign.
        /// </summary>
        public static readonly char CommercialAt = '@';

        /// <summary>
        /// Gets the number sign.
        /// </summary>
        public static readonly char NumberSign = '#';

        /// <summary>
        /// Gets the asterisk.
        /// </summary>
        public static readonly char Asterisk = '*';

        /// <summary>
        /// Gets the array end character.
        /// </summary>
        public static readonly char EndCharOfArray = '\0';

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the asterisk wildcard regular expression.
        /// </summary>
        /// <value>The asterisk wildcard regular expression.</value>
        public static string AsteriskWildcardRegularExpression
        {
            get
            {
                return "((.|\\n)*)";
            }
        }

        /// <summary>
        /// Gets the question mark wildcard regular expression.
        /// </summary>
        /// <value>The question mark wildcard regular expression.</value>
        public static string QuestionMarkWildcardRegularExpression
        {
            get
            {
                return ".";
            }
        }

        /// <summary>
        /// Gets the asterisk wildcard.
        /// </summary>
        /// <value>The asterisk wildcard.</value>
        public static string AsteriskWildcard
        {
            get
            {
                return Asterisk.ToString();
            }
        }

        /// <summary>
        /// Gets the question mark wildcard.
        /// </summary>
        /// <value>The question mark wildcard.</value>
        public static string QuestionMarkWildcard
        {
            get
            {
                return QuestionMark.ToString();
            }
        }

        /// <summary>
        /// Gets the prefix of the placeholder.
        /// </summary>
        public static string ReplacePlaceholder
        {
            get
            {
                return "@";
            }
        }

        /// <summary>
        /// Gets the list separator.
        /// </summary>
        /// <value>The list separator.</value>
        public static string ListSeparator
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            }
        }

        /// <summary>
        /// Gets the format separator.
        /// </summary>
        public static char FormatSeparator
        {
            get
            {
                return Semicolon;
            }
        }

        /// <summary>
        /// Gets the number group separator.
        /// </summary>
        /// <remarks>By default, the separator is ",".</remarks>
        public static string NumberGroupSeparator
        {
            get
            {
                var result = NumberFormatInfo.NumberGroupSeparator;
                //for some cultrue like french, it will return not breaking space (char 160), replace it with normal space( (char)32)
                return result.Replace((char)160, (char)32);
            }
        }

        /// <summary>
        /// Gets the percent sign.
        /// </summary>
        /// <remarks>By default, the percent sign is "%".</remarks>
        public static string PercentSymbol
        {
            get
            {
                return NumberFormatInfo.PercentSymbol;
            }
        }

        /// <summary>
        /// Gets the plus sign.
        /// </summary>
        /// <remarks>By default, the positive sign is "+".</remarks>
        public static string PositiveSign
        {
            get
            {
                return NumberFormatInfo.PositiveSign;
            }
        }

        /// <summary>
        /// Gets the minus sign.
        /// </summary>
        /// <remarks>By default, the negative sign is "-".</remarks>
        public static string NegativeSign
        {
            get
            {
                return NumberFormatInfo.NegativeSign;
            }
        }

        /// <summary>
        /// Gets the decimal separator.
        /// </summary>
        /// <remarks>By default, the decimal separator is ".".</remarks>
        public static string DecimalSeparator
        {
            get
            {
                return NumberFormatInfo.NumberDecimalSeparator;
            }
        }

        /// <summary>
        /// Gets the NaN symbol.
        /// </summary>
        /// <value>The NaN symbol.</value>
        public static string NaNSymbol
        {
            get
            {
                return NumberFormatInfo.NaNSymbol;
            }
        }

        /// <summary>
        /// Gets the exponential symbol.
        /// </summary>
        /// <remarks>By default, the exponential symbol is "e".</remarks>
        public static string ExponentialSymbol
        {
            get
            {
                return "E";
            }
        }

        /// <summary>
        /// Gets the exponential positive symbol.
        /// </summary>
        /// <remarks>By default, the exponential positive symbol is "e+".</remarks>
        public static string Exponential1
        {
            get
            {
                return "E+";
            }
        }

        /// <summary>
        /// Gets the exponential negative symbol.
        /// </summary>
        /// <remarks>By default, the exponential negative symbol is "e-".</remarks>
        public static string Exponential2
        {
            get
            {
                return "E-";
            }
        }

        /// <summary>
        /// Gets the custom format for the default AM or PM symbol in Excel.
        /// </summary>
        public static string AMPMTwoDigit
        {
            get
            {
                return "AM/PM";
            }
        }

        /// <summary>
        /// Gets the custom format for the short AM or PM symbol in Excel.
        /// </summary>
        public static string AMPMSingleDigit
        {
            get
            {
                return "A/P";
            }
        }

        /// <summary>
        /// Gets the date time format information.
        /// </summary>
        /// <value>The date time format information.</value>
        public static DateTimeFormatInfo DateTimeFormatInfo
        {
            get
            {
#if XAML
                return CultureInfo.CurrentCulture.DateTimeFormat;
#else
                return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
#endif
            }
        }

        /// <summary>
        /// Gets the number format information.
        /// </summary>
        /// <value>The number format information.</value>
        public static NumberFormatInfo NumberFormatInfo
        {
            get
            {
#if XAML
                return CultureInfo.CurrentCulture.NumberFormat;
#else
                return System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
#endif
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the specified character is an operator.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>
        /// 	<c>true</c> if the specified character is an operator; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOperator(char c)
        {
            return (c == DefaultTokens.LessThanSign || c == DefaultTokens.GreaterThanSign || c == DefaultTokens.EqualsThanSign);
        }

        public static string TrimEscape(string token)
        {
            string s = token;
            int length = s.Length;
            bool inEscape = false;
            StringBuilder sb = new StringBuilder();
            for (int n = 0; n < length; n++)
            {
                char c = s[n];
                if (c == DefaultTokens.ReverseSolidusSign)
                {
                    inEscape = !inEscape;
                    if (!inEscape)
                        sb.Append(c);
                }
                else
                {
                    inEscape = false;
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Trims the square bracket.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Returns the trimmed format string.</returns>
        public static string TrimSquareBracket(string token)
        {
            if (token == null || token == string.Empty)
            {
                return token;
            }

            if (token[0] == DefaultTokens.LeftSquareBracket)
            {
                token = token.TrimStart(DefaultTokens.LeftSquareBracket);
            }

            if (token[token.Length - 1] == DefaultTokens.RightSquareBracket)
            {
                token = token.TrimEnd(DefaultTokens.RightSquareBracket);
            }

            return token;
        }

        /// <summary>
        /// Adds a square bracket.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Returns the formatted string.</returns>
        public static string AddSquareBracket(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            if (token.Length == 0 || token[0] != DefaultTokens.LeftSquareBracket)
            {
                token = token.Insert(0, DefaultTokens.LeftSquareBracket.ToString());
            }

            if (token.Length == 0 || token[token.Length - 1] != DefaultTokens.RightSquareBracket)
            {
                token = token.Insert(token.Length, DefaultTokens.RightSquareBracket.ToString());
            }

            return token;
        }

        /// <summary>
        /// Determines whether the specified characters are equal.
        /// </summary>
        /// <param name="a">The first character.</param>
        /// <param name="b">The second character.</param>
        /// <param name="isIgnoreCase">if set to <c>true</c>, ignore the case when comparing.</param>
        /// <returns>
        /// 	<c>true</c> if the two characters are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEquals(char a, char b, bool isIgnoreCase)
        {
            if (isIgnoreCase)
            {
                return Char.ToLower(a) == Char.ToLower(b);
            }
            else
            {
                return a == b;
            }
        }

        /// <summary>
        /// Determines whether the specified character equals the specified string.
        /// </summary>
        /// <param name="a">The character.</param>
        /// <param name="b">The string.</param>
        /// <param name="isIgnoreCase">if set to <c>true</c>, ignore the case when comparing.</param>
        /// <returns>
        /// 	<c>true</c> if the character is equal to the string; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEquals(char a, string b, bool isIgnoreCase)
        {
            if (b == null)
            {
                return false;
            }

            if (b.Length != 1)
            {
                return false;
            }

            return IsEquals(a, b[0], isIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified string equals the specified character.
        /// </summary>
        /// <param name="a">The string.</param>
        /// <param name="b">The character.</param>
        /// <param name="isIgnoreCase">if set to <c>true</c>, ignore the case when comparing.</param>
        /// <returns>
        /// 	<c>true</c> if the string is equal to the character; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEquals(string a, char b, bool isIgnoreCase)
        {
            if (a == null)
            {
                return false;
            }

            if (a.Length != 1)
            {
                return false;
            }

            return IsEquals(a[0], b, isIgnoreCase);
        }

        public static string[] Split(string s, char spliter)
        {
            List<string> strs = new List<string>();
            char strMark = '\"';
            if (s == null || s == string.Empty)
            {
                return strs.ToArray();
            }

            bool inEscape = false;
            StringBuilder sb = new StringBuilder();
            bool inStr = false;
            for (int n = 0; n < s.Length; n++)
            {
                char c = s[n];
                if (c == strMark && !inEscape)
                    inStr = !inStr;

                if (!inEscape && !inStr && c == spliter)
                {
                    strs.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }

                if (c == DefaultTokens.ReverseSolidusSign)
                {
                    inEscape = inEscape ? false : true;
                }
                else
                {
                    inEscape = false;
                }
            }

            strs.Add(sb.ToString());
            return strs.ToArray();
        }

        /// <summary>
        /// Filters the specified string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="bracketsStart">The start bracket.</param>
        /// <param name="bracketsEnd">The end bracket.</param>
        /// <returns>Returns the filtered string.</returns>
        public static string Filter(string s, char bracketsStart, char bracketsEnd)
        {
            if (s == null || s == string.Empty)
            {
                return s;
            }

            bool inEscape = false;
            StringBuilder sb = new StringBuilder();
            int refCount = 0;
            for (int n = 0; n < s.Length; n++)
            {
                char c = s[n];
                if (c == bracketsStart && !inEscape)
                {
                    refCount++;
                }
                else if (c == bracketsEnd && !inEscape)
                {
                    refCount--;
                    if (refCount < 0)
                    {
                        refCount = 0;
                    }
                }
                else if (refCount == 0)
                {
                    sb.Append(c);
                }

                if (c == DefaultTokens.ReverseSolidusSign)
                {
                    inEscape = inEscape ? false : true;
                }
                else
                {
                    inEscape = false;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Replaces the specified string with a new string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="oldString">The old string.</param>
        /// <param name="newString">The new string.</param>
        /// <returns>Returns the replaced format string.</returns>
        public static string ReplaceKeyword(string s, string oldString, string newString)
        {
            if (s == null || s == string.Empty)
            {
                return s;
            }

            string strTemp = s;
            int start = 0;
            while (true)
            {
                int index = strTemp.IndexOf(oldString, start, StringComparison.CurrentCultureIgnoreCase);
                if (index > -1)
                {
                    strTemp = strTemp.Remove(index, oldString.Length);
                    strTemp = strTemp.Insert(index, newString);
                    start = index + newString.Length;
                }
                else
                {
                    break;
                }
            }

            return strTemp;
        }

        /// <summary>
        /// Determines whether the specified string is a decimal.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="numberFormatInfo">The number format information.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is a decimal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDecimal(string s, NumberFormatInfo numberFormatInfo)
        {
            string decimalSeparator = DefaultTokens.DecimalSeparator;
            if (numberFormatInfo != null)
            {
                decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            }

            return (s.IndexOf(decimalSeparator) > -1);
        }

        public static string ReplaceChar(string s, char oldChar, char newChar)
        {
            if (s == null || s == string.Empty)
                return string.Empty;

            bool inEscape = false;
            bool inStr = false;
            StringBuilder sb = new StringBuilder();
            for (int n = 0; n < s.Length; n++)
            {
                char c = s[n];
                if (c == DefaultTokens.DoubleQuote && !inEscape)
                    inStr = !inStr;

                if (!inEscape && !inStr && c == oldChar)
                {
                    sb.Append(newChar);
                }
                else
                {
                    sb.Append(c);
                }

                if (c == DefaultTokens.ReverseSolidusSign)
                {
                    inEscape = inEscape ? false : true;
                }
                else
                {
                    inEscape = false;
                }
            }

            return sb.ToString();
        }

        #endregion
    }

    internal class SharedResourceStrings
    {
        internal static string FormatIllegalFormatError
        {
            get
            {
                return "The format is illegal.";//ResourceManager.GetString("FormatIllegalFormatError", resourceCulture);
            }
        }
    }
}

#endregion