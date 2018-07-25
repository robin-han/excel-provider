using GrapeCity.Enterprise.Data.DataSource.Common;
using GrapeCity.Enterprise.Data.DataSource.Common.Expression;
using GrapeCity.Enterprise.Data.DataSource.Common.SqlExecutor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public interface ISQLASTVisitor
    {
        bool Visit(AssignExpression assignExpr);
        bool Visit(BetweenExpression betweenExpr);
        bool Visit(ColumnExpression columnExpr);
        bool Visit(FunctionExpression functionExpr);
        bool Visit(KeywordExpression keywordExpr);
        bool Visit(LiteralValueExpression literalExpr);
        bool Visit(OperationExpression operationExpr);
        bool Visit(ListExpression listExpression);
        bool Visit(ParamExpression paramExpression);
        bool Visit(SubQueryExpression subQueryExpression);
    }


    public class ExcelEvalContext
    {
        private IResultSet _resultSet;


        public ExcelEvalContext(IResultSet resultSet)
        {
            if (resultSet == null)
            {
                throw new ArgumentNullException("resultset");
            }

            this._resultSet = resultSet;
        }

        public object GetValue(string tableName, string columnName)
        {
            return this._resultSet.GetValue(tableName, columnName);
        }

        public object GetParameter(string paramName)
        {
            ExcelParameter paramter = (ExcelParameter)this._resultSet.GetParameter(paramName);
            return paramter.Value;
        }

        internal ExcelCommand Command
        {
            get
            {
                return ((ExcelResultSet)this._resultSet).Command;
            }
        }

        //...
    }

    public class ExcelEvalVisitor : ISQLASTVisitor
    {
        private Dictionary<IExpression, object> _evalValues;
        private ExcelEvalContext _evalContext;

        public ExcelEvalVisitor(ExcelEvalContext evalContext)
        {
            if (evalContext == null)
            {
                throw new ArgumentNullException("evalContext");
            }

            this._evalContext = evalContext;
            this._evalValues = new Dictionary<IExpression, object>();
        }

        #region Evaluate
        public object Evaluate(IExpression expr)
        {
            expr.Accept(this);

            if (!this._evalValues.TryGetValue(expr, out object value))
            {
                throw new ExcelException("eval error");
            }
            this._evalValues.Clear();

            return value;
        }

        public bool Visit(AssignExpression assignExpr)
        {
            return false;
        }

        public bool Visit(BetweenExpression betweenExpr)
        {
            var subExprs = betweenExpr.SubExpressions;

            var testExpr = subExprs[0];
            testExpr.Accept(this);
            if (!this._evalValues.TryGetValue(testExpr, out object value))
            {
                return false;
            }

            var beginExpr = subExprs[1];
            beginExpr.Accept(this);
            if (!this._evalValues.TryGetValue(beginExpr, out object begin))
            {
                return false;
            }
            if (LessThan(value, begin))
            {
                this._evalValues.Add(betweenExpr, false); //not true
                return false;
            }

            var endExpr = subExprs[2];
            endExpr.Accept(this);
            if (!this._evalValues.TryGetValue(endExpr, out object end))
            {
                return false;
            }
            if (GreaterThan(value, end))
            {
                this._evalValues.Add(betweenExpr, false); //not true
                return false;
            }

            this._evalValues.Add(betweenExpr, true); //not false
            return false;
        }

        public bool Visit(ColumnExpression columnExpr)
        {
            object value;
            if (columnExpr.IsExpression)
            {
                var expr = columnExpr.SubExpressions[0];
                expr.Accept(this);
                if (!this._evalValues.TryGetValue(expr, out value))
                {
                    return false;
                }
            }
            else
            {
                value = this._evalContext.GetValue(columnExpr.TableName, columnExpr.ColumnName);
            }
            this._evalValues.Add(columnExpr, value);

            return false;
        }

        public bool Visit(ParamExpression paramExpr)
        {
            object value = this._evalContext.GetParameter(paramExpr.ParamName);
            this._evalValues.Add(paramExpr, value);

            return false;
        }

        public bool Visit(FunctionExpression functionExpr)
        {
            var subExprs = functionExpr.SubExpressions;

            object value = null;
            switch (functionExpr.FunctionName)
            {
                #region Math
                case SqlConstants.ABS:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Abs(arg1, out Type t);
                    }
                    break;
                case SqlConstants.ACOS:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Acos(arg1);
                    }
                    break;
                case SqlConstants.ASIN:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Asin(arg1);
                    }
                    break;
                case SqlConstants.ATAN:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Atan(arg1);
                    }
                    break;
                case SqlConstants.ATN2:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[1];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.Atn2(arg1, arg2);
                    }
                    break;
                case SqlConstants.CEILING:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Ceiling(arg1, out Type t);
                    }
                    break;
                case SqlConstants.COS:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Cos(arg1);
                    }
                    break;
                case SqlConstants.COT:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Cot(arg1);
                    }
                    break;
                case SqlConstants.DEGREES:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Degrees(arg1);
                    }
                    break;
                case SqlConstants.EXP:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Exp(arg1);
                    }
                    break;
                case SqlConstants.FLOOR:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Floor(arg1, out Type t);
                    }
                    break;
                case SqlConstants.LOG:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        double? newBase = null;
                        if (subExprs.Count > 1)
                        {
                            var exprArg2 = subExprs[1];
                            exprArg2.Accept(this);
                            if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                            {
                                return false;
                            }
                            newBase = ToDouble(arg2);
                        }

                        value = FunctionHelper.Log(arg1, newBase);
                    }
                    break;
                case SqlConstants.LOG10:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Log10(arg1);
                    }
                    break;
                case SqlConstants.PI:
                    {
                        value = FunctionHelper.PI();
                    }
                    break;
                case SqlConstants.POWER:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[1];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.Power(arg1, arg2);
                    }
                    break;
                case SqlConstants.RADIANS:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Radians(arg1);
                    }
                    break;
                case SqlConstants.RAND:
                    {
                        int? seed = null;
                        if (subExprs.Count > 0)
                        {
                            var exprArg1 = subExprs[0];
                            exprArg1.Accept(this);
                            if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                            {
                                return false;
                            }
                            seed = ToInt(arg1);
                        }

                        value = FunctionHelper.Rand(seed);
                    }
                    break;
                case SqlConstants.ROUND:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[1];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.Round(arg1, ToInt(arg2).Value, out Type t);
                    }
                    break;
                case SqlConstants.SIGN:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Sign(arg1, out Type t);
                    }
                    break;
                case SqlConstants.SIN:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Sin(arg1);
                    }
                    break;
                case SqlConstants.SQRT:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Sqrt(arg1);
                    }
                    break;
                case SqlConstants.SQUARE:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Square(arg1);
                    }
                    break;
                case SqlConstants.TAN:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Tan(arg1);
                    }
                    break;
                #endregion

                #region String
                case SqlConstants.CONCAT:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[1];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        IEnumerable<object> os = null;
                        if (subExprs.Count > 2)
                        {
                            var exprArg3 = subExprs[2];
                            exprArg3.Accept(this);
                            if (!this._evalValues.TryGetValue(exprArg3, out object arg3))
                            {
                                return false;
                            }
                            os = (IEnumerable<object>)arg3;
                        }

                        value = FunctionHelper.Concat(arg1, arg2, os);
                    }
                    break;
                case SqlConstants.FORMAT:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[1];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.Format(arg1, ToString(arg2));
                    }
                    break;
                case SqlConstants.INDEXOF:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[1];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }
                        int start = 0;
                        if (subExprs.Count > 2)
                        {
                            var exprArg3 = subExprs[2];
                            exprArg3.Accept(this);
                            if (!this._evalValues.TryGetValue(exprArg3, out object arg3))
                            {
                                return false;
                            }
                            start = ToInt(arg3).Value;
                        }

                        value = FunctionHelper.IndexOf(arg1, arg2, start);
                    }
                    break;
                case SqlConstants.ISNULL:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        object arg2 = null;
                        if (subExprs.Count > 1)
                        {
                            var exprArg2 = subExprs[1];
                            exprArg2.Accept(this);
                            if (!this._evalValues.TryGetValue(exprArg2, out arg2))
                            {
                                return false;
                            }
                        }

                        value = FunctionHelper.IsNull(arg1, arg2, out Type t);
                    }
                    break;
                case SqlConstants.LEFT:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[1];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.Left(arg1, ToInt(arg2).Value);
                    }
                    break;
                case SqlConstants.LEN:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Len(arg1);
                    }
                    break;
                case SqlConstants.LOWER:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Lower(arg1);
                    }
                    break;
                case SqlConstants.LTRIM:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.LTrim(arg1);
                    }
                    break;
                case SqlConstants.REPLACE:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }
                        var exprArg3 = subExprs[0];
                        exprArg3.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg3, out object arg3))
                        {
                            return false;
                        }

                        value = FunctionHelper.Replace(arg1, ToString(arg2), ToString(arg3));
                    }
                    break;
                case SqlConstants.REPLICATE:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.Replicate(arg1, ToInt(arg2).Value);
                    }
                    break;
                case SqlConstants.REVERSE:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Reverse(arg1);
                    }
                    break;
                case SqlConstants.RIGHT:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.Right(arg1, ToInt(arg2).Value);
                    }
                    break;
                case SqlConstants.RTRIM:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.RTrim(arg1);
                    }
                    break;
                case SqlConstants.SUBSTRING:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }
                        var exprArg3 = subExprs[0];
                        exprArg3.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg3, out object arg3))
                        {
                            return false;
                        }

                        value = FunctionHelper.Substring(arg1, ToInt(arg2).Value, ToInt(arg3).Value);
                    }
                    break;
                case SqlConstants.TOSTRING:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.ToString(arg1);
                    }
                    break;
                case SqlConstants.TRIM:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Trim(arg1);
                    }
                    break;
                case SqlConstants.UPPER:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        value = FunctionHelper.Upper(arg1);
                    }
                    break;
                #endregion

                #region DateTime
                case SqlConstants.DATEADD:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }
                        var exprArg3 = subExprs[0];
                        exprArg3.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg3, out object arg3))
                        {
                            return false;
                        }

                        value = FunctionHelper.DateAdd(ToString(arg1), ToInt(arg2).Value, arg3);
                    }
                    break;
                case SqlConstants.DATEDIFF:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }
                        var exprArg3 = subExprs[0];
                        exprArg3.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg3, out object arg3))
                        {
                            return false;
                        }

                        value = FunctionHelper.DateDiff(ToString(arg1), arg2, arg3);
                    }
                    break;
                case SqlConstants.DATEFROMPARTS:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }
                        var exprArg3 = subExprs[0];
                        exprArg3.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg3, out object arg3))
                        {
                            return false;
                        }

                        value = FunctionHelper.DateFromParts(ToInt(arg1).Value, ToInt(arg2).Value, ToInt(arg3).Value);
                    }
                    break;
                case SqlConstants.DATEPART:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }
                        var exprArg2 = subExprs[0];
                        exprArg2.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                        {
                            return false;
                        }

                        value = FunctionHelper.DatePart(ToString(arg1), arg2);
                    }
                    break;
                case SqlConstants.EOMONTH:
                    {
                        var exprArg1 = subExprs[0];
                        exprArg1.Accept(this);
                        if (!this._evalValues.TryGetValue(exprArg1, out object arg1))
                        {
                            return false;
                        }

                        int mon = 0;
                        if (subExprs.Count > 1)
                        {
                            var exprArg2 = subExprs[0];
                            exprArg2.Accept(this);
                            if (!this._evalValues.TryGetValue(exprArg2, out object arg2))
                            {
                                return false;
                            }

                            mon = ToInt(arg2).Value;
                        }

                        value = FunctionHelper.EOMonth(ToString(arg1), mon);
                    }
                    break;
                case SqlConstants.GETDATE:
                    {
                        value = FunctionHelper.GetDate();
                    }
                    break;
                case SqlConstants.GETUTCDATE:
                    {
                        value = FunctionHelper.GetUtcDate();
                    }
                    break;

                #endregion

                default:
                    throw new NotSupportedException();
            }

            if (value == DBNull.Value) //
            {
                value = null;
            }
            this._evalValues.Add(functionExpr, value);
            return false;
        }

        public bool Visit(KeywordExpression keywordExpr)
        {
            return false;
        }

        public bool Visit(LiteralValueExpression literalExpr)
        {
            object value;
            switch (literalExpr.ValueType)
            {
                case LiteralValueType.Number:
                    value = ToDouble(literalExpr.Value);
                    break;
                case LiteralValueType.String:
                    value = literalExpr.Value;
                    break;
                case LiteralValueType.Boolean:
                    value = ToBool(literalExpr.Value);
                    break;
                case LiteralValueType.Null:
                    value = null;
                    break;
                case LiteralValueType.Blob:
                default:
                    value = literalExpr.Value;
                    break;
            }

            this._evalValues.Add(literalExpr, value);
            return false;
        }

        public bool Visit(ListExpression listExpression)
        {
            List<object> values = new List<object>();
            foreach (var subExpr in listExpression.SubExpressions)
            {
                subExpr.Accept(this);

                if (!this._evalValues.TryGetValue(subExpr, out object value))
                {
                    return false;
                }
                values.Add(value);
            }
            this._evalValues.Add(listExpression, values);

            return false;
        }

        public bool Visit(SubQueryExpression subQueryExpression)
        {
            List<object[]> results = new List<object[]>();

            ExcelResultSet resultSet = new ExcelResultSet(subQueryExpression.Statement, this._evalContext.Command);
            using (ExcelDataReader dataReader = new ExcelDataReader(resultSet))
            {
                int fieldCount = dataReader.FieldCount;
                while (dataReader.Read())
                {
                    object[] rowData = new object[fieldCount];
                    for (int i = 0; i < fieldCount; i++)
                    {
                        rowData[i] = dataReader.GetValue(i);
                    }
                    results.Add(rowData);
                }
            }
            this._evalValues.Add(subQueryExpression, results);

            return false;
        }

        public bool Visit(OperationExpression operationExp)
        {
            if (operationExp.OperationType == OperationType.Binary)
            {
                return this.VisitBinaryOpExpr(operationExp);
            }
            else
            {
                return this.VisitUnaryExpr(operationExp);
            }
        }

        private bool VisitBinaryOpExpr(OperationExpression binaryExpr)
        {
            var leftExpr = binaryExpr.SubExpressions[0];
            var rightExpr = binaryExpr.SubExpressions[1];
            leftExpr.Accept(this);
            rightExpr.Accept(this);

            if (!this._evalValues.TryGetValue(leftExpr, out object leftValue))
            {
                return false;
            }
            if (!this._evalValues.TryGetValue(rightExpr, out object rightValue))
            {
                return false;
            }

            Object value = null;
            switch (binaryExpr.Operator)
            {
                case "AND":
                    {
                        bool first = EqualsTo(leftValue, true);
                        bool second = EqualsTo(rightValue, true);
                        this._evalValues.Add(binaryExpr, first && second);
                    }
                    break;
                case "OR":
                    {
                        bool first = EqualsTo(leftValue, true);
                        bool second = EqualsTo(rightValue, true);
                        this._evalValues.Add(binaryExpr, first || second);
                    }
                    break;
                case "*":
                    value = Multiply(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "/":
                    value = Divide(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "+":
                    value = Add(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "-":
                    value = Subtract(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "%":
                    value = Modulus(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "=":
                    value = EqualsTo(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "!=":
                case "<>":
                    value = NotEqualsTo(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case ">":
                    value = GreaterThan(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "<":
                    value = LessThan(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case ">=":
                    value = GreaterThanOrEqualsTo(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "<=":
                    value = LessThanOrEqualsTo(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "&":
                    value = BitAnd(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "|":
                    value = BitOr(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "<<":
                    value = LeftShift(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case ">>":
                    value = RightShift(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "LIKE":
                    {
                        string input = ToString(leftValue);
                        string pattern = ToString(rightValue);
                        value = Like(input, pattern);
                        this._evalValues.Add(binaryExpr, value);
                    }
                    break;
                case "NOT LIKE":
                    {
                        string input = ToString(leftValue);
                        string pattern = ToString(rightValue);
                        value = NotLike(input, pattern);
                        this._evalValues.Add(binaryExpr, value);
                    }
                    break;
                case "IS":
                    value = leftValue == null ? true : false;
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "IS NOT":
                    value = leftValue == null ? false : true;
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "IN":
                    value = In(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "NOT IN":
                    value = NotIn(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;
                case "||":
                    value = Concat(leftValue, rightValue);
                    this._evalValues.Add(binaryExpr, value);
                    break;

                default:
                    throw new NotSupportedException(binaryExpr.Operator);
            }

            return false;
        }

        private bool VisitUnaryExpr(OperationExpression unaryExpr)
        {
            var expr = unaryExpr.SubExpressions[0];
            expr.Accept(this);

            if (!this._evalValues.TryGetValue(expr, out object val))
            {
                return false;
            }

            if (val == null)
            {
                this._evalValues.Add(unaryExpr, null);
                return false;
            }

            switch (unaryExpr.Operator)
            {
                case "!":
                    val = ToBool(val);
                    this._evalValues.Add(unaryExpr, val);
                    break;

                case "+":
                    this._evalValues.Add(unaryExpr, val);
                    break;
                case "-":
                    this._evalValues.Add(unaryExpr, Multiply(val, -1));
                    break;
                case "~":
                    this._evalValues.Add(unaryExpr, ~ToLong(val));
                    break;
                case "EXISTS":
                    this._evalValues.Add(unaryExpr, ((IList<object[]>)val).Count > 0);
                    break;
                default:
                    throw new NotSupportedException(unaryExpr.Operator);
            }

            return false;
        }
        #endregion

        #region Helper Methods
        public static object Concat(object a, object b)
        {
            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            return ToString(a) + ToString(b);
        }

        public static object Add(object a, object b)
        {
            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            if (a is string && b is string)
            {
                return a.ToString() + b.ToString();
            }

            return ToDouble(a) + ToDouble(b);
        }

        public static object Subtract(object a, object b)
        {
            if (a == null)
            {
                return null;
            }

            if (b == null)
            {
                return a;
            }

            return ToDouble(a) - ToDouble(b);
        }

        public static object Multiply(object a, object b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return ToDouble(a) * ToDouble(b);
        }

        public static object Divide(object a, object b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return ToDouble(a) / ToDouble(b);
        }

        public static object Modulus(object a, object b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return ToDouble(a) % ToDouble(b);
        }

        public static bool Like(string input, string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern is null");
            }
            if(input == null)
            {
                input = string.Empty;
            }

            StringBuilder regexprBuilder = new StringBuilder(pattern.Length + 4);

            const int STAT_NOTSET = 0;
            const int STAT_RANGE = 1;
            const int STAT_LITERAL = 2;

            int stat = STAT_NOTSET;

            int blockStart = -1;
            for (int i = 0; i < pattern.Length; ++i)
            {
                char ch = pattern[i];

                if (stat == STAT_LITERAL && (ch == '%' || ch == '_' || ch == '['))
                {
                    string block = pattern.Substring(blockStart, i - blockStart);
                    regexprBuilder.Append(Regex.Escape(block));
                    blockStart = -1;
                    stat = STAT_NOTSET;
                }

                if (ch == '%')
                {
                    regexprBuilder.Append(".*");
                }
                else if (ch == '_')
                {
                    regexprBuilder.Append('.');
                }
                else if (ch == '[')
                {
                    if (stat == STAT_RANGE)
                    {
                        throw new ArgumentException("illegal pattern : " + pattern);
                    }
                    stat = STAT_RANGE;
                    blockStart = i;
                }
                else if (ch == ']')
                {
                    if (stat != STAT_RANGE)
                    {
                        throw new ArgumentException("illegal pattern : " + pattern);
                    }
                    string block = pattern.Substring(blockStart, i + 1 - blockStart);
                    regexprBuilder.Append(block);

                    blockStart = -1;
                }
                else
                {
                    if (stat == STAT_NOTSET)
                    {
                        stat = STAT_LITERAL;
                        blockStart = i;
                    }

                    if (stat == STAT_LITERAL && i == pattern.Length - 1)
                    {
                        string block = pattern.Substring(blockStart, i + 1 - blockStart);
                        regexprBuilder.Append(Regex.Escape(block));
                    }
                }
            }
            if ("%".Equals(pattern) || "%%".Equals(pattern))
            {
                return true;
            }

            string regexpr = regexprBuilder.ToString();
            return Regex.IsMatch(input, regexpr, RegexOptions.IgnoreCase);
        }

        public static bool NotLike(string input, string pattern)
        {
            return !Like(input, pattern);
        }

        public static bool In(object value, object list)
        {
            IList<object> list0 = (IList<object>)list;

            foreach (object item in list0)
            {
                if (EqualsTo(value, item))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool NotIn(object value, object list)
        {
            return !In(value, list);
        }

        public static bool GreaterThan(object a, object b)
        {
            if (a == null)
            {
                return false;
            }
            if (b == null)
            {
                return true;
            }

            if (IsDate(a) || IsDate(b))
            {
                return ToDate(a) > ToDate(b);
            }

            if (a is string || b is string)
            {
                return a.ToString().CompareTo(b.ToString()) > 0;
            }

            if (IsNumber(a) || IsNumber(b))
            {
                return ToDouble(a) > ToDouble(b);
            }

            throw new InvalidOperationException(a.GetType().Name + " and " + b.GetType().Name);
        }

        public static bool GreaterThanOrEqualsTo(object a, object b)
        {
            if (EqualsTo(a, b))
            {
                return true;
            }

            return GreaterThan(a, b);
        }

        public static bool LessThan(object a, object b)
        {
            if (a == null)
            {
                return true;
            }
            if (b == null)
            {
                return false;
            }

            if (IsDate(a) || IsDate(b))
            {
                return ToDate(a) < ToDate(b);
            }

            if (a is string || b is string)
            {
                return a.ToString().CompareTo(b.ToString()) < 0;
            }

            if (IsNumber(a) || IsNumber(b))
            {
                return ToDouble(a) < ToDouble(b);
            }

            throw new InvalidOperationException(a.GetType().Name + " and " + b.GetType().Name);
        }

        public static bool LessThanOrEqualsTo(object a, object b)
        {
            if (EqualsTo(a, b))
            {
                return true;
            }

            return LessThan(a, b);
        }

        public static bool EqualsTo(object a, object b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            if (a.Equals(b))
            {
                return true;
            }

            if (IsDate(a) || IsDate(b))
            {
                return ToDate(a).Equals(ToDate(b));
            }

            if (a is string || b is string)
            {
                return ToString(a).Equals(ToString(b));
            }

            if (a is bool || b is bool)
            {
                return ToBool(a).Equals(ToBool(b));
            }

            if (IsNumber(a) || IsNumber(b))
            {
                return ToDouble(a).Equals(ToDouble(b));
            }

            throw new InvalidOperationException(a.GetType().Name + " and " + b.GetType().Name);
        }

        public static bool NotEqualsTo(object a, object b)
        {
            return !EqualsTo(a, b);
        }

        public static object BitAnd(object a, object b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return (ToLong(a) & ToLong(b));
        }

        public static object BitOr(object a, object b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return (ToLong(a) | ToLong(b));
        }

        public static object RightShift(object a, object b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return (ToLong(a) >> ToInt(b));
        }

        public static object LeftShift(object a, object b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return (ToLong(a) << ToInt(b));
        }

        private static bool IsNumber(object value)
        {
            return DataTypeUtil.IsNumber(value);
        }

        private static bool IsDate(object value)
        {
            return DataTypeUtil.IsDate(value);
        }

        private static int? ToInt(object value)
        {
            if (value == null) {
                return null; //0
            }

            try
            {
                return DataTypeUtil.ToInt(value);
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        private static long? ToLong(object value)
        {
            if (value == null) {
                return null; //0
            }

            try
            {
                return DataTypeUtil.ToLong(value);
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        private static double? ToDouble(object value)
        {
            if (value == null)
            {
                return null; //0.0
            }

            try
            {
                return DataTypeUtil.ToDouble(value);
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        private static bool? ToBool(object value)
        {
            if (value == null)
            {
                return null; //false
            }

            try
            {
                return DataTypeUtil.ToBoolean(value);
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        private static DateTime? ToDate(object value)
        {
            if (value == null)
            {
                return null; //
            }

            try
            {
                return DataTypeUtil.ToDateTime(value);
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        private static string ToString(object value)
        {
            return DataTypeUtil.ToString(value);
        }

        #endregion
    }

    public static class ExpressionExtensions
    {
        public static void Accept(this IExpression expr, ISQLASTVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }

            if (expr is LiteralValueExpression)
            {
                ((LiteralValueExpression)expr).Accept0(visitor);
            }
            else if (expr is OperationExpression)
            {
                ((OperationExpression)expr).Accept0(visitor);
            }
            else if (expr is AssignExpression)
            {
                ((AssignExpression)expr).Accept0(visitor);
            }
            else if (expr is BetweenExpression)
            {
                ((BetweenExpression)expr).Accept0(visitor);
            }
            else if (expr is ColumnExpression)
            {
                ((ColumnExpression)expr).Accept0(visitor);
            }
            else if (expr is ParamExpression)
            {
                ((ParamExpression)expr).Accept0(visitor);
            }
            else if (expr is ListExpression)
            {
                ((ListExpression)expr).Accept0(visitor);
            }
            else if (expr is FunctionExpression)
            {
                ((FunctionExpression)expr).Accept0(visitor);
            }
            else if (expr is KeywordExpression)
            {
                ((KeywordExpression)expr).Accept0(visitor);
            }
            else if (expr is SubQueryExpression)
            {
                ((SubQueryExpression)expr).Accept0(visitor);
            }
        }


        //
        public static void Accept0(this AssignExpression assignExpr, ISQLASTVisitor visitor)
        {
            throw new NotSupportedException();
        }

        public static void Accept0(this BetweenExpression betweenExpr, ISQLASTVisitor visitor)
        {
            if (visitor.Visit(betweenExpr))
            {
                var subExps = betweenExpr.SubExpressions;
                var testExpr = subExps[0];
                var beginExpr = subExps[1];
                var endExpr = subExps[2];

                testExpr.Accept(visitor);
                beginExpr.Accept(visitor);
                endExpr.Accept(visitor);
            }
        }

        public static void Accept0(this ColumnExpression columnExpr, ISQLASTVisitor visitor)
        {
            visitor.Visit(columnExpr);
        }

        public static void Accept0(this FunctionExpression functionExpr, ISQLASTVisitor visitor)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public static void Accept0(this KeywordExpression keywordExpr, ISQLASTVisitor visitor)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public static void Accept0(this LiteralValueExpression literalExpr, ISQLASTVisitor visitor)
        {
            visitor.Visit(literalExpr);
        }

        public static void Accept0(this OperationExpression operationExpr, ISQLASTVisitor visitor)
        {
            if (visitor.Visit(operationExpr))
            {
                var subExprs = operationExpr.SubExpressions;
                if (subExprs.Count == 2) //Binary
                {
                    var left = subExprs[0];
                    var right = subExprs[1];
                    left.Accept(visitor);
                    right.Accept(visitor);
                }
                else //Unary
                {
                    var expr = subExprs[0];
                    expr.Accept(visitor);
                }
            }
        }

        public static void Accept0(this ListExpression listExpr, ISQLASTVisitor visitor)
        {
            if (visitor.Visit(listExpr))
            {
                foreach (var itemExpr in listExpr.SubExpressions)
                {
                    itemExpr.Accept(visitor);
                }
            }
        }

        public static void Accept0(this ParamExpression paramExpr, ISQLASTVisitor visitor)
        {
            visitor.Visit(paramExpr);
        }

        public static void Accept0(this SubQueryExpression subQueryExpr, ISQLASTVisitor visitor)
        {
            visitor.Visit(subQueryExpr);
        }
    }
}

