//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Sql.g4 by ANTLR 4.6.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlParse {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ISqlVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.1")]
[System.CLSCompliant(false)]
public partial class SqlBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, ISqlVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.parse"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitParse([NotNull] SqlParser.ParseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.error"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitError([NotNull] SqlParser.ErrorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.sql_stmt_list"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSql_stmt_list([NotNull] SqlParser.Sql_stmt_listContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.sql_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSql_stmt([NotNull] SqlParser.Sql_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.begin_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBegin_stmt([NotNull] SqlParser.Begin_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.commit_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCommit_stmt([NotNull] SqlParser.Commit_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.delete_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDelete_stmt([NotNull] SqlParser.Delete_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.factored_select_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFactored_select_stmt([NotNull] SqlParser.Factored_select_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.compound_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCompound_clause([NotNull] SqlParser.Compound_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.limit_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitLimit_clause([NotNull] SqlParser.Limit_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.insert_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitInsert_stmt([NotNull] SqlParser.Insert_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.values_list"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitValues_list([NotNull] SqlParser.Values_listContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.release_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRelease_stmt([NotNull] SqlParser.Release_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.rollback_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRollback_stmt([NotNull] SqlParser.Rollback_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.savepoint_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSavepoint_stmt([NotNull] SqlParser.Savepoint_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.simple_select_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSimple_select_stmt([NotNull] SqlParser.Simple_select_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.select_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSelect_stmt([NotNull] SqlParser.Select_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.select_or_values"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSelect_or_values([NotNull] SqlParser.Select_or_valuesContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.update_stmt"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitUpdate_stmt([NotNull] SqlParser.Update_stmtContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.set_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSet_clause([NotNull] SqlParser.Set_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.update_stmt_limited"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitUpdate_stmt_limited([NotNull] SqlParser.Update_stmt_limitedContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_def"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitColumn_def([NotNull] SqlParser.Column_defContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.type_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitType_name([NotNull] SqlParser.Type_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_constraint"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitColumn_constraint([NotNull] SqlParser.Column_constraintContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.conflict_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitConflict_clause([NotNull] SqlParser.Conflict_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.expr"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpr([NotNull] SqlParser.ExprContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.in_list_expr"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIn_list_expr([NotNull] SqlParser.In_list_exprContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.function"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFunction([NotNull] SqlParser.FunctionContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.args_list"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitArgs_list([NotNull] SqlParser.Args_listContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.foreign_key_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitForeign_key_clause([NotNull] SqlParser.Foreign_key_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.raise_function"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRaise_function([NotNull] SqlParser.Raise_functionContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.indexed_column"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIndexed_column([NotNull] SqlParser.Indexed_columnContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_constraint"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTable_constraint([NotNull] SqlParser.Table_constraintContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.qualified_table_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitQualified_table_name([NotNull] SqlParser.Qualified_table_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.ordering_term"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitOrdering_term([NotNull] SqlParser.Ordering_termContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.result_column"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitResult_column([NotNull] SqlParser.Result_columnContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_or_subquery"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTable_or_subquery([NotNull] SqlParser.Table_or_subqueryContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitJoin_clause([NotNull] SqlParser.Join_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_right"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitJoin_right([NotNull] SqlParser.Join_rightContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitJoin_operator([NotNull] SqlParser.Join_operatorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_constraint"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitJoin_constraint([NotNull] SqlParser.Join_constraintContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.select_core"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSelect_core([NotNull] SqlParser.Select_coreContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.where_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitWhere_clause([NotNull] SqlParser.Where_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.groupby_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitGroupby_clause([NotNull] SqlParser.Groupby_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.having_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitHaving_clause([NotNull] SqlParser.Having_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.compound_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCompound_operator([NotNull] SqlParser.Compound_operatorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.signed_number"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSigned_number([NotNull] SqlParser.Signed_numberContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.literal_value"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitLiteral_value([NotNull] SqlParser.Literal_valueContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.unary_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitUnary_operator([NotNull] SqlParser.Unary_operatorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.error_message"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitError_message([NotNull] SqlParser.Error_messageContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.module_argument"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitModule_argument([NotNull] SqlParser.Module_argumentContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_alias"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitColumn_alias([NotNull] SqlParser.Column_aliasContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.keyword"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitKeyword([NotNull] SqlParser.KeywordContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitName([NotNull] SqlParser.NameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.function_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFunction_name([NotNull] SqlParser.Function_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.database_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDatabase_name([NotNull] SqlParser.Database_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTable_name([NotNull] SqlParser.Table_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_or_index_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTable_or_index_name([NotNull] SqlParser.Table_or_index_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.new_table_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNew_table_name([NotNull] SqlParser.New_table_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitColumn_name([NotNull] SqlParser.Column_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.collation_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCollation_name([NotNull] SqlParser.Collation_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.foreign_table"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitForeign_table([NotNull] SqlParser.Foreign_tableContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.index_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIndex_name([NotNull] SqlParser.Index_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.trigger_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTrigger_name([NotNull] SqlParser.Trigger_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.view_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitView_name([NotNull] SqlParser.View_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.module_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitModule_name([NotNull] SqlParser.Module_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.pragma_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPragma_name([NotNull] SqlParser.Pragma_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.savepoint_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSavepoint_name([NotNull] SqlParser.Savepoint_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_alias"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTable_alias([NotNull] SqlParser.Table_aliasContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.transaction_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTransaction_name([NotNull] SqlParser.Transaction_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.any_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAny_name([NotNull] SqlParser.Any_nameContext context) { return VisitChildren(context); }
}
} // namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlParse