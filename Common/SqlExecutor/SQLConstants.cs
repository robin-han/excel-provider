using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlExecutor
{
    public class SqlConstants
    {
        // Param
        public const string DefaultParamPrefix = "@";

        // Operators
        public const string AND = "AND";
        public const string OR = "OR";
        public const string IS = "IS";
        public const string ISNOT = "IS NOT";
        public const string LIKE = "LIKE";
        public const string NOTLIKE = "NOT LIKE";
        public const string IN = "IN";
        public const string NOTIN = "NOT IN";

        public const string EQ = "=";
        public const string NEQ = "!=";
        public const string NEQ2 = "<>";
        public const string GT = ">";
        public const string GTE = ">=";
        public const string LT = "<";
        public const string LTE = "<=";
        public const string ADD = "+";
        public const string SUB = "-";
        public const string MUL = "*";
        public const string DIV = "/";
        public const string MOD = "%";

        public static IList<string> CompareOperators = new[] { EQ, NEQ, NEQ2, GT, GTE, LT, LTE };
        public static IList<string> CalculateOperators = new[] { ADD, SUB, MUL, DIV, MOD };

        // Unary operators
        public const string MINUS = "-";
        public const string PLUS = "+";
        public const string COMPL = "~";
        public const string NOT = "NOT";

        // Keywords
        public const string ALL = "*";
        public const string NULL = "NULL";

        // Orders
        public const string ASC = "ASC";
        public const string DESC = "DESC";

        #region StringFunctions
        public const string CONCAT = "CONCAT";
        public const string FORMAT = "FORMAT";
        public const string INDEXOF = "INDEXOF";
        public const string ISNULL = "ISNULL";
        public const string LEFT = "LEFT";
        public const string LEN = "LEN";
        public const string LOWER = "LOWER";
        public const string LTRIM = "LTRIM";
        public const string REPLACE = "REPLACE";
        public const string REPLICATE = "REPLICATE";
        public const string REVERSE = "REVERSE";
        public const string RIGHT = "RIGHT";
        public const string RTRIM = "RTRIM";
        public const string SUBSTRING = "SUBSTRING";
        public const string TOSTRING = "TOSTRING";
        public const string TRIM = "TRIM";
        public const string UPPER = "UPPER";
        public static IList<string> StringFunctions = new[] { CONCAT, FORMAT, INDEXOF, ISNULL, LEFT, LEN, LOWER, LTRIM, REPLACE, REPLICATE, REVERSE,
            RIGHT, RTRIM, SUBSTRING, TOSTRING, TRIM, UPPER };
        #endregion // StringFunctions

        #region DateTimeFunctions
        public const string DATEADD = "DATEADD";
        public const string DATEDIFF = "DATEDIFF";
        public const string DATEFROMPARTS = "DATEFROMPARTS";
        public const string DATEPART = "DATEPART";
        public const string EOMONTH = "EOMONTH";
        public const string GETDATE = "GETDATE";
        public const string GETUTCDATE = "GETUTCDATE";
        public static readonly IReadOnlyList<string> DateTimeFunctions = new[] { DATEADD, DATEDIFF, DATEFROMPARTS, DATEPART, EOMONTH, GETDATE, GETUTCDATE };
        #endregion // DateTimeFunctions

        #region MathematicsFunctions
        public const string ABS = "ABS";
        public const string ACOS = "ACOS";
        public const string ASIN = "ASIN";
        public const string ATAN = "ATAN";
        public const string ATN2 = "ATN2";
        public const string CEILING = "CEILING";
        public const string COS = "COS";
        public const string COT = "COT";
        public const string DEGREES = "DEGREES";
        public const string EXP = "EXP";
        public const string FLOOR = "FLOOR";
        public const string LOG = "LOG";
        public const string LOG10 = "LOG10";
        public const string PI = "PI";
        public const string POWER = "POWER";
        public const string RADIANS = "RADIANS";
        public const string RAND = "RAND";
        public const string ROUND = "ROUND";
        public const string SIGN = "SIGN";
        public const string SIN = "SIN";
        public const string SQRT = "SQRT";
        public const string SQUARE = "SQUARE";
        public const string TAN = "TAN";
        public static IReadOnlyList<string> MathematicFunctions = new[] { ABS, ACOS, ASIN, ATAN, ATN2, CEILING, COS, COT, DEGREES, EXP, FLOOR, LOG, LOG10,
            PI, POWER, RADIANS, RAND, ROUND, SIGN, SIN, SQRT, SQUARE, TAN };
        #endregion // MathematicsFunctions

        #region AggregationFunctions
        public const string COUNT = "COUNT";
        public const string SUM = "SUM";
        public const string MIN = "MIN";
        public const string MAX = "MAX";
        public const string AVG = "AVG";
        public static IList<string> AggregationFunctions = new[] { COUNT, SUM, MIN, MAX, AVG };
        #endregion // AggregationFunctions
    }
}
