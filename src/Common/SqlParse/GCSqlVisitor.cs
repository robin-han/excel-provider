using Antlr4.Runtime.Misc;
using GrapeCity.Enterprise.Data.DataSource.Common.Expression;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlParse
{
    internal class GCSqlVisitor : SqlBaseVisitor<object>
    {
        private readonly SqlParser _parser;
        private static readonly Regex nameEscapePattern = new Regex("(?s)^(?:\")(.*?)(?:\")|(?:\\[)(.*?)(?:\\])|(?:`)(.*?)(?:`)$");
        private readonly Stack<Dictionary<string, string>> queryTableStack = new Stack<Dictionary<string, string>>();
        private readonly Stack<Dictionary<string, ColumnExpression>> queryColumnStack = new Stack<Dictionary<string, ColumnExpression>>();
        private readonly string _sql;


        public GCSqlVisitor(SqlParser parser, string sql)
        {
            _parser = parser;
            _sql = sql.Trim();
        }

        public override object VisitParse([NotNull] SqlParser.ParseContext context)
        {
            if (context.exception != null)
            {
                throw new SQLParseException($"Incorrect syntax near '{context.exception.OffendingToken.Text}'");
            }

            if (context.ChildCount > 2)
            {
                throw new SQLParseException($"Incorrect syntax near '{context.GetChild(1)}'");
            }

            object result = null;
            if (context.sql_stmt_list() != null)
            {
                result = VisitSql_stmt_list(context.sql_stmt_list());
            }
            else
            {
                throw new SQLParseException($"Incorrect syntax near '{context.GetChild(0)}'");
            }

            return result;
        }

        public override object VisitSql_stmt_list([NotNull] SqlParser.Sql_stmt_listContext context)
        {
            var result = VisitSql_stmt(context.sql_stmt()[0]);
            return result;
        }

        public override object VisitSql_stmt([NotNull] SqlParser.Sql_stmtContext context)
        {
            if (context.exception != null)
            {
                throw new SQLParseException($"Incorrect syntax in the sql statement.");
            }
            ISQLStatement statement;
            queryTableStack.Push(new Dictionary<string, string>());
            queryColumnStack.Push(new Dictionary<string, ColumnExpression>());
            if (context.factored_select_stmt() != null)
            {
                statement = (SelectStatement)VisitFactored_select_stmt(context.factored_select_stmt());
            }
            else if (context.update_stmt() != null)
            {
                statement = (UpdateStatement)VisitUpdate_stmt(context.update_stmt());
            }
            else if (context.insert_stmt() != null)
            {
                statement = (InsertStatement)VisitInsert_stmt(context.insert_stmt());
            }
            else if (context.delete_stmt() != null)
            {
                statement = (DeleteStatement)VisitDelete_stmt(context.delete_stmt());
            }
            else
            {
                statement = (ISQLStatement)VisitChildren(context);
            }
            queryTableStack.Pop();
            queryColumnStack.Pop();
            return statement;
        }

        private void ProcessLimitClause(SqlParser.Limit_clauseContext context, SelectStatement statement)
        {
            if (context.K_OFFSET() != null && context.expr().Length == 2)
            {
                statement.Limit = ParseLiteralValue<int>((IExpression)VisitExpr(context.expr()[0]));
                statement.Offset = ParseLiteralValue<int>((IExpression)VisitExpr(context.expr()[1]));
            }
            else if (context.expr().Length == 2)
            {
                statement.Limit = ParseLiteralValue<int>((IExpression)VisitExpr(context.expr()[1]));
                statement.Offset = ParseLiteralValue<int>((IExpression)VisitExpr(context.expr()[0]));
            }
            else if (context.expr().Length == 1)
            {
                statement.Limit = ParseLiteralValue<int>((IExpression)VisitExpr(context.expr()[0]));
            }

        }

        public override object VisitFactored_select_stmt([NotNull] SqlParser.Factored_select_stmtContext context)
        {
            var statement = (SelectStatement)VisitSelect_core(context.select_core());
            if (context.ordering_term() != null)
            {
                statement.Orderby = new List<OrderbyColumn>();

                foreach (var item in context.ordering_term())
                {
                    statement.Orderby.Add((OrderbyColumn)VisitOrdering_term(item));
                }
            }

            if (context.limit_clause() != null)
            {
                ProcessLimitClause(context.limit_clause(), statement);
            }

            if (context.compound_clause() != null && context.compound_clause().Length > 0)
            {
                statement.IsCompound = true;
                statement.CompoundStatements = new List<CompoundSelectStatement>();
                foreach (var item in context.compound_clause())
                {
                    var compoundStatement = (CompoundSelectStatement)VisitCompound_clause(item);
                    statement.CompoundStatements.Add(compoundStatement);
                }
            }

            return statement;
        }

        public override object VisitCompound_clause([NotNull] SqlParser.Compound_clauseContext context)
        {
            var statement = new CompoundSelectStatement();
            if (context.compound_operator().K_UNION() != null && context.compound_operator().K_ALL() != null)
            {
                statement.CompoundType = CompoundType.UnionAll;
            }
            else if (context.compound_operator().K_UNION() != null)
            {
                statement.CompoundType = CompoundType.Union;
            }

            statement.Statement = (SelectStatement)VisitSelect_core(context.select_core());

            return statement;
        }

        private T ParseLiteralValue<T>(IExpression expr)
        {
            if (expr is LiteralValueExpression literalExpr)
            {
                return (T)Convert.ChangeType(literalExpr.Value, typeof(T));
            }

            throw new SQLParseException("Invalid literal value");
        }

        public override object VisitSelect_core([NotNull] SqlParser.Select_coreContext context)
        {
            SelectStatement select = null;

            select = new SelectStatement();

            if (context.table_or_subquery() != null)
            {
                if (context.table_or_subquery().Length > 1)
                {
                    select.Table = (TableName)VisitTable_or_subquery(context.table_or_subquery(0));
                    select.IsJoin = true;
                    select.JoinTables = new List<JoinTable>();
                    for (int i = 1; i < context.table_or_subquery().Length; i++)
                    {
                        select.JoinTables.Add((new JoinTable()
                        {
                            JoinType = JoinType.Unknown,
                            Table = (TableName)VisitTable_or_subquery(context.table_or_subquery(i))
                        }));
                    }
                }
                else if (context.table_or_subquery().Length == 1)
                {
                    select.Table = (TableName)VisitTable_or_subquery(context.table_or_subquery(0));
                }
            }

            if (context.join_clause() != null)
            {
                select.IsJoin = true;
                select.Table = (TableName)VisitTable_or_subquery(context.join_clause().table_or_subquery());

                var noAlias = false; ;
                if (select.Table.Alias.ToUpper() == "INNER" || select.Table.Alias.ToUpper() == "CROSS")
                {
                    noAlias = true;
                }

                select.JoinTables = new List<JoinTable>();
                foreach (var item in context.join_clause().join_right())
                {
                    var joinTable = (JoinTable)VisitJoin_right(item);
                    if (noAlias)
                    {
                        joinTable.JoinType = select.Table.Alias.ToUpper() == "INNER" ? JoinType.Inner : JoinType.Cross;
                        select.Table.Alias = "";
                    }
                    select.JoinTables.Add(joinTable);
                }
            }

            if (context.result_column() != null)
            {
                select.Columns = new List<ColumnExpression>();
                foreach (var item in context.result_column())
                {
                    select.Columns.Add((ColumnExpression)VisitResult_column(item));
                }
            }

            if (context.where_clause() != null)
            {
                select.Where = (IExpression)VisitWhere_clause(context.where_clause());
                if (select.Where is LiteralValueExpression)
                {
                    throw new SQLParseException($"Incorrect syntax near '{context.where_clause().GetChild(0)}'");
                }
            }

            if (context.groupby_clause() != null)
            {
                select.Groupby = (GroupbyClause)VisitGroupby_clause(context.groupby_clause());
            }

            return select;
        }

        public override object VisitJoin_right([NotNull] SqlParser.Join_rightContext context)
        {
            var joinTable = new JoinTable();
            if (context.join_operator().K_CROSS() != null)
            {
                joinTable.JoinType = JoinType.Cross;
            }
            else if (context.join_operator().K_INNER() != null)
            {
                joinTable.JoinType = JoinType.Inner;
            }
            else if (context.join_operator().K_LEFT() != null && context.join_operator().K_OUTER() != null)
            {
                joinTable.JoinType = JoinType.LeftOuter;
            }
            else if (context.join_operator().K_LEFT() != null)
            {
                joinTable.JoinType = JoinType.Left;
            }
            else
            {
                joinTable.JoinType = JoinType.Inner;
            }

            joinTable.Table = (TableName)VisitTable_or_subquery(context.table_or_subquery());
            joinTable.JoinContraints = (IExpression)VisitJoin_constraint(context.join_constraint());

            return joinTable;
        }

        public override object VisitJoin_constraint([NotNull] SqlParser.Join_constraintContext context)
        {
            if (context.expr() == null)
            {
                return null;
            }
            return VisitExpr(context.expr());
        }

        public override object VisitOrdering_term([NotNull] SqlParser.Ordering_termContext context)
        {
            var columnExpression = VisitExpr(context.expr()) as ColumnExpression;

            if (columnExpression == null)
            {
                throw new SQLParseException("Only Column be set in the Order by clause.");
            }

            return new OrderbyColumn()
            {

                Column = columnExpression,
                Order = context.K_ASC() != null ? "ASC" : context.K_DESC() != null ? "DESC" : "ASC"
            };
        }

        public override object VisitTable_or_subquery([NotNull] SqlParser.Table_or_subqueryContext context)
        {
            if (context.table_name() != null)
            {
                if (context.table_alias() != null && IsKeyWord(context.table_alias().GetText()))
                {
                    throw new SQLParseException($"Incorrect syntax near '{context.table_alias().GetText()}'");
                }

                var table = new TableName()
                {
                    Name = EscapeTableOrColumnName(context.table_name().GetText()),
                    Alias = EscapeTableOrColumnName(context.table_alias() == null ? "" : context.table_alias().GetText())
                };

                var tableMap = queryTableStack.Peek();
                if (!string.IsNullOrEmpty(table.Alias) && tableMap != null)
                {
                    tableMap[table.Alias] = table.Name;
                }

                return table;
            }

            return null;
        }

        private bool IsKeyWord(string word)
        {
            if (string.IsNullOrEmpty(word)) return false;

            var keywords = new List<string>() { "where", "order" };
            return keywords.IndexOf(word) > -1;
        }

        public override object VisitResult_column([NotNull] SqlParser.Result_columnContext context)
        {

            ColumnExpression column = null;
            if (context.GetText().EndsWith("*"))
            {
                var table = context.table_name() == null ? "" : context.table_name().GetText();

                var tableMap = queryTableStack.Peek();

                GetTableNameAlias(tableMap, table, out var tableName, out var tableAlias);

                column = new ColumnExpression()
                {
                    ColumnName = "*",
                    TableName = tableName,
                    TableAlias = tableAlias,
                };
            }
            else
            {
                // in case of resut column it should only set alias, but not use alias.
                var expr = (IExpression)VisitExpr(context.expr(), false);
                if (expr is ColumnExpression)
                {
                    column = expr as ColumnExpression;
                }
                else
                {
                    column = new ColumnExpression
                    {
                        IsExpression = true,
                        ColumnName = context.expr().GetText(),
                        SubExpressions = new List<IExpression>
                        {
                            expr
                        }
                    };
                }

                if (context.column_alias() != null)
                {
                    column.Alias = EscapeTableOrColumnName(context.column_alias().GetText());
                    var columnMap = queryColumnStack.Peek();
                    if (columnMap != null)
                    {
                        columnMap[column.Alias] = column;
                    }
                }
            }

            return column;
        }

        private string EscapeTableOrColumnName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var match = nameEscapePattern.Match(name);
                if (match.Success)
                {
                    return match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value;
                }
            }
            return name;
        }

        public override object VisitWhere_clause([NotNull] SqlParser.Where_clauseContext context)
        {
            if (context.expr() == null)
            {
                throw new SQLParseException($"Incorrect syntax near '{context.GetText()}'");
            }

            return VisitExpr(context.expr());
        }

        public override object VisitExpr([NotNull] SqlParser.ExprContext context)
        {
            return VisitExpr(context, true);
        }

        private object VisitExpr([NotNull] SqlParser.ExprContext context, bool considerColumnAlias)
        {
            if (context.K_IN() != null && (context.in_list_expr() != null || context.factored_select_stmt() != null))
            {
                var result = new OperationExpression()
                {
                    OperationType = OperationType.Binary,
                    Operator = context.K_NOT() != null ? "NOT IN" : "IN",
                    SubExpressions = new List<IExpression>()
                };

                result.SubExpressions.Add((IExpression)VisitExpr(context.expr(0)));

                if (context.factored_select_stmt() != null)
                {
                    var statement = (SelectStatement)VisitFactored_select_stmt(context.factored_select_stmt());
                    result.SubExpressions.Add(new SubQueryExpression()
                    {
                        Statement = statement
                    });
                }
                else
                {
                    var listExpression = new ListExpression()
                    {
                        SubExpressions = new List<IExpression>()
                    };

                    result.SubExpressions.Add(listExpression);

                    foreach (var item in context.in_list_expr().expr())
                    {
                        listExpression.SubExpressions.Add((IExpression)VisitExpr(item));
                    }
                }

                return result;
            }

            if (context.K_EXISTS() != null && context.factored_select_stmt() != null)
            {
                var statement = (SelectStatement)VisitFactored_select_stmt(context.factored_select_stmt());

                var result = new OperationExpression()
                {
                    OperationType = OperationType.Unary,
                    Operator = context.K_NOT() != null ? "NOT EXISTS" : "EXISTS",
                    SubExpressions = new List<IExpression>()
                    {
                        new SubQueryExpression()
                        {
                            Statement = statement,
                        }
                    }
                };

                return result;
            }

            if (context.column_name() != null)
            {
                var table = context.table_name() == null ? "" : context.table_name().GetText();
                var column = context.column_name().GetText();

                var tableMap = queryTableStack.Peek();
                var columMap = queryColumnStack.Peek();

                if (considerColumnAlias && TryGetColumnExpressionByAlias(columMap, column, out var columnExpression))
                {
                    return columnExpression;
                }

                GetTableNameAlias(tableMap, table, out var tableName, out var tableAlias);

                return new ColumnExpression()
                {
                    ColumnName = EscapeTableOrColumnName(column),
                    TableName = tableName,
                    TableAlias = tableAlias
                };
            }

            if (context.function() != null)
            {
                var func = new FunctionExpression
                {
                    FunctionName = context.function().function_name().GetText()
                };
                if (!GCSqlVisitorCheck.FunctionsDict.TryGetValue(func.FunctionName, out var parametersInfo))
                {
                    throw new SQLParseException($"Not supported function '{func.FunctionName}'");
                }

                // parameter count check
                var parameterCount = context.function().args_list()?.expr()?.Length ?? 0;
                if (parameterCount < parametersInfo.Min || parameterCount > parametersInfo.Max)
                {
                    if (parametersInfo.Min == parametersInfo.Max)
                    {
                        throw new SQLParseException($"Not '{func.FunctionName}' function accpet {parametersInfo.Max} parameter(s)");
                    }
                    else
                    {
                        throw new SQLParseException($"Not '{func.FunctionName}' function accpet {parametersInfo.Min} to {parametersInfo.Max} parameter(s)");
                    }
                }

                func.SubExpressions = new List<IExpression>();
                if (context.function().args_list() != null)
                {
                    if (context.function().args_list().GetText() == "*")
                    {
                        func.SubExpressions.Add(new KeywordExpression() { Keyword = context.function().args_list().GetText() });
                    }
                    else
                    {
                        var exprList = context.function().args_list().expr();
                        if (exprList != null && exprList.Length != 0)
                        {
                            for (int i = 0; i < exprList.Length; i++)
                            {
                                var item = exprList[i];

                                IExpression argumentExpression = null;

                                var descriptor = parametersInfo.Descriptors != null && i < parametersInfo.Descriptors.Length ? parametersInfo.Descriptors[i] : null;
                                if (descriptor != null)
                                {
                                    switch (descriptor.DescriptorType)
                                    {
                                        case ParameterDescriptorType.Enum:
                                            var parameterText = item?.GetText();
                                            if (!descriptor.CheckParameter(parameterText))
                                            {
                                                throw new SQLParseException($"'{parameterText}' is not a recognized datepart option.");
                                            }
                                            argumentExpression = new LiteralValueExpression()
                                            {
                                                Value = parameterText,
                                                ValueType = LiteralValueType.String,
                                            };
                                            break;
                                        default:
                                            // do not need special handling.
                                            break;
                                    }
                                }

                                argumentExpression = argumentExpression ?? (IExpression)VisitExpr(item);
                                func.SubExpressions.Add(argumentExpression);
                            }
                        }
                    }
                }

                return func;
            }

            if (context.unary_operator() != null)
            {
                var result = new OperationExpression()
                {
                    OperationType = OperationType.Unary
                };
                result.Operator = context.unary_operator().GetText().ToUpper();
                result.SubExpressions = new List<IExpression>
                {
                    (IExpression)VisitExpr((SqlParser.ExprContext)context.unary_operator().Parent.GetChild(1))
                };

                // special handling for the not exists, in order to reduce the layer of the expression
                if (result.SubExpressions.Count == 1 && (result.SubExpressions[0] as OperationExpression)?.Operator == "EXISTS")
                {
                    ((OperationExpression)result.SubExpressions[0]).Operator = "NOT EXISTS";
                    return result.SubExpressions[0];
                }
                return result;
            }

            if (context.BIND_PARAMETER() != null)
            {
                return new ParamExpression()
                {
                    ParamName = context.GetText()
                };
            }

            if (context.ChildCount == 1)
            {
                if (context.literal_value() != null)
                {
                    var value = context.literal_value();
                    var text = value.GetText();
                    if (!string.IsNullOrEmpty(text) && text.StartsWith("'") && text.EndsWith("'"))
                    {
                        text = text.Substring(1, text.Length - 2);
                    }

                    var result = new LiteralValueExpression()
                    {
                        Value = text
                    };

                    if (value.NUMERIC_LITERAL() != null)
                    {
                        result.ValueType = LiteralValueType.Number;
                    }
                    else if (value.STRING_LITERAL() != null)
                    {
                        result.ValueType = LiteralValueType.String;
                    }
                    else if (value.NUMERIC_BOOLEAN() != null)
                    {
                        result.ValueType = LiteralValueType.Boolean;
                    }
                    else if (value.BLOB_LITERAL() != null)
                    {
                        result.ValueType = LiteralValueType.Blob;
                    }
                    else if (value.K_NULL() != null)
                    {
                        result.ValueType = LiteralValueType.Null;
                    }

                    return result;
                }
            }
            else if (context.ChildCount == 2)
            {
                if (context.K_ISNULL() != null)
                {
                    var result = new OperationExpression()
                    {
                        OperationType = OperationType.Binary,
                        Operator = "IS"
                    };
                    result.SubExpressions = new List<IExpression>
                    {
                        (IExpression)VisitExpr((SqlParser.ExprContext)context.GetChild(0)),
                        new LiteralValueExpression() { ValueType = LiteralValueType.Null, Value = "NULL" }
                    };
                    return result;
                }
            }
            else if (context.ChildCount == 3)
            {
                if (context.GetChild(0).GetText() == "(" && context.GetChild(2).GetText() == ")")
                {
                    return VisitExpr((SqlParser.ExprContext)context.GetChild(1));
                }

                var result = new OperationExpression()
                {
                    OperationType = OperationType.Binary
                };
                var op = context.GetChild(1).GetText().ToUpper();
                switch (op)
                {
                    case "AND":
                    case "OR":
                    case "*":
                    case "/":
                    case "+":
                    case "-":
                    case "=":
                    case "!=":
                    case "<>":
                    case ">":
                    case "<":
                    case ">=":
                    case "<=":
                    case "<<":
                    case ">>":
                    case "&":
                    case "|":
                    case "%":
                    case "LIKE":
                        {
                            result.Operator = op;
                            result.SubExpressions = new List<IExpression>
                            {
                                (IExpression)VisitExpr((SqlParser.ExprContext)context.GetChild(0)),
                                (IExpression)VisitExpr((SqlParser.ExprContext)context.GetChild(2))
                            };
                        }
                        break;
                    case "IS":
                        {

                            result.SubExpressions = new List<IExpression>()
                            {
                                (IExpression) VisitExpr((SqlParser.ExprContext) context.GetChild(0))
                            };
                            if (context.GetChild(2) is SqlParser.ExprContext exprContext)
                            {
                                if (exprContext.ChildCount == 2 && exprContext.unary_operator() != null && exprContext.unary_operator().K_NOT() != null && exprContext.GetChild(1).GetText().ToUpper() == "NULL")
                                {
                                    result.Operator = "IS NOT";
                                    result.SubExpressions.Add(new LiteralValueExpression() { Value = "NULL", ValueType = LiteralValueType.Null });
                                }
                                else
                                {
                                    result.Operator = op;
                                    result.SubExpressions.Add((IExpression)VisitExpr((SqlParser.ExprContext)context.GetChild(2)));
                                }
                            }
                            else
                            {
                                result.Operator = op;
                                result.SubExpressions.Add((IExpression)VisitExpr((SqlParser.ExprContext)context.GetChild(2)));
                            }
                        }
                        break;
                }
                return result;
            }
            else if (context.ChildCount == 4)
            {
                if (context.expr().Length == 2 &&
                    context.GetChild(1).GetText().ToUpper() == "NOT" &&
                    context.GetChild(2).GetText().ToUpper() == "LIKE")
                {
                    var result = new OperationExpression()
                    {
                        OperationType = OperationType.Binary,
                        Operator = "NOT LIKE",
                        SubExpressions = new List<IExpression>()
                        {
                            (IExpression)VisitExpr(context.expr(0)),
                            (IExpression)VisitExpr(context.expr(1))
                        }
                    };

                    return result;
                }
            }
            else if (context.ChildCount == 5
                     && context.expr().Length == 3
                     && context.GetChild(1).GetText().ToUpper() == "BETWEEN"
                     && context.GetChild(3).GetText().ToUpper() == "AND")
            {
                return new BetweenExpression()
                {
                    SubExpressions = new List<IExpression>()
                        {
                            (IExpression)VisitExpr(context.expr(0)),
                            (IExpression)VisitExpr(context.expr(1)),
                            (IExpression)VisitExpr(context.expr(2))
                        }
                };
            }

            return null;
        }


        private void GetTableNameAlias(Dictionary<string, string> mapppingDict, string original, out string realName, out string alias)
        {
            if (string.IsNullOrEmpty(original))
            {
                realName = string.Empty;
                alias = string.Empty;
                return;
            }

            var escapedValue = EscapeTableOrColumnName(original);

            if (mapppingDict != null
                && string.IsNullOrEmpty(escapedValue) == false
                && mapppingDict.TryGetValue(escapedValue, out var c))
            {
                realName = c;
                alias = escapedValue;
                return;
            }

            realName = escapedValue;
            alias = string.Empty;
            return;
        }

        private bool TryGetColumnExpressionByAlias(Dictionary<string, ColumnExpression> mapppingDict, string original, out ColumnExpression columnExpression)
        {
            if (string.IsNullOrEmpty(original))
            {
                columnExpression = null;
                return false;
            }

            var escapedValue = EscapeTableOrColumnName(original);

            if (mapppingDict != null
                && string.IsNullOrEmpty(escapedValue) == false
                && mapppingDict.TryGetValue(escapedValue, out var c))
            {
                columnExpression = c;
                return true;
            }

            columnExpression = null;
            return false;
        }

        public override object VisitUpdate_stmt([NotNull] SqlParser.Update_stmtContext context)
        {
            var statement = new UpdateStatement();
            if (context.set_clause() != null)
            {
                statement.Assignments = new List<AssignExpression>();

                foreach (var item in context.set_clause().expr())
                {
                    var assignment = (IExpression)VisitExpr(item);
                    statement.Assignments.Add(new AssignExpression
                    {
                        Column = (ColumnExpression)assignment.SubExpressions[0],
                        Value = assignment.SubExpressions[1]
                    });
                }
            }

            if (context.qualified_table_name() != null)
            {
                statement.Table = (TableName)VisitQualified_table_name(context.qualified_table_name());
            }

            if (context.where_clause() != null)
            {
                statement.Where = (IExpression)VisitWhere_clause(context.where_clause());
            }

            return statement;
        }

        public override object VisitInsert_stmt([NotNull] SqlParser.Insert_stmtContext context)
        {
            var statement = new InsertStatement();
            var columnNames = context.column_name();
            if (columnNames != null && columnNames.Length > 0)
            {
                statement.Columns = new List<string>();
                foreach (var item in columnNames)
                {
                    statement.Columns.Add(item.GetText());
                }
            }

            if (context.qualified_table_name() != null)
            {
                statement.Table = (TableName)VisitQualified_table_name(context.qualified_table_name());
            }

            if (context.K_VALUES() != null && context.K_DEFAULT() == null)
            {
                statement.Values = new List<IList<IExpression>>();
                foreach (var vl in context.values_list())
                {
                    var valueList = new List<IExpression>();
                    foreach (var val in vl.expr())
                    {
                        valueList.Add((IExpression)VisitExpr(val));
                    }
                    statement.Values.Add(valueList);
                }
            }
            return statement;
        }

        public override object VisitQualified_table_name([NotNull] SqlParser.Qualified_table_nameContext context)
        {
            return new TableName()
            {
                Database = context.database_name() == null ? "" : context.database_name().GetText(),
                Name = EscapeTableOrColumnName(context.table_name() == null ? "" : context.table_name().GetText()),
                Alias = ""
            };
        }

        public override object VisitDelete_stmt([NotNull] SqlParser.Delete_stmtContext context)
        {
            var statement = new DeleteStatement();
            if (context.where_clause() != null)
            {
                statement.Where = (IExpression)VisitWhere_clause(context.where_clause());
            }

            if (context.qualified_table_name() != null)
            {
                statement.Table = (TableName)VisitQualified_table_name(context.qualified_table_name());
            }

            return statement;
        }

        public override object VisitGroupby_clause([NotNull] SqlParser.Groupby_clauseContext context)
        {
            var groupby = new GroupbyClause();
            if (context.expr() != null && context.expr().Length > 0)
            {
                groupby.Columns = new List<ColumnExpression>();
                foreach (var expr in context.expr())
                {
                    var columnExpression = VisitExpr(expr) as ColumnExpression;
                    if (columnExpression == null)
                    {
                        throw new SQLParseException("Only column could be set in Group by clause.");
                    }
                    groupby.Columns.Add(columnExpression);
                }
            }

            if (context.having_clause() != null)
            {
                groupby.Having = (IExpression)VisitExpr(context.having_clause().expr());
            }

            return groupby;
        }

        public override object VisitError([NotNull] SqlParser.ErrorContext context)
        {
            throw new SQLParseException(context.GetText());
        }

    }
}
