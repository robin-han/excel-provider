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

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="SqlParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.1")]
[System.CLSCompliant(false)]
public interface ISqlVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.parse"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParse([NotNull] SqlParser.ParseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.error"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitError([NotNull] SqlParser.ErrorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.sql_stmt_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSql_stmt_list([NotNull] SqlParser.Sql_stmt_listContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.sql_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSql_stmt([NotNull] SqlParser.Sql_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.begin_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBegin_stmt([NotNull] SqlParser.Begin_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.commit_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommit_stmt([NotNull] SqlParser.Commit_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.delete_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDelete_stmt([NotNull] SqlParser.Delete_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.factored_select_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFactored_select_stmt([NotNull] SqlParser.Factored_select_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.compound_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompound_clause([NotNull] SqlParser.Compound_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.limit_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLimit_clause([NotNull] SqlParser.Limit_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.insert_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInsert_stmt([NotNull] SqlParser.Insert_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.values_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValues_list([NotNull] SqlParser.Values_listContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.release_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRelease_stmt([NotNull] SqlParser.Release_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.rollback_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRollback_stmt([NotNull] SqlParser.Rollback_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.savepoint_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSavepoint_stmt([NotNull] SqlParser.Savepoint_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.simple_select_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSimple_select_stmt([NotNull] SqlParser.Simple_select_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.select_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelect_stmt([NotNull] SqlParser.Select_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.select_or_values"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelect_or_values([NotNull] SqlParser.Select_or_valuesContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.update_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUpdate_stmt([NotNull] SqlParser.Update_stmtContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.set_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSet_clause([NotNull] SqlParser.Set_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.update_stmt_limited"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUpdate_stmt_limited([NotNull] SqlParser.Update_stmt_limitedContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitColumn_def([NotNull] SqlParser.Column_defContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.type_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType_name([NotNull] SqlParser.Type_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_constraint"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitColumn_constraint([NotNull] SqlParser.Column_constraintContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.conflict_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConflict_clause([NotNull] SqlParser.Conflict_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpr([NotNull] SqlParser.ExprContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.in_list_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIn_list_expr([NotNull] SqlParser.In_list_exprContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunction([NotNull] SqlParser.FunctionContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.args_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgs_list([NotNull] SqlParser.Args_listContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.foreign_key_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForeign_key_clause([NotNull] SqlParser.Foreign_key_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.raise_function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRaise_function([NotNull] SqlParser.Raise_functionContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.indexed_column"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIndexed_column([NotNull] SqlParser.Indexed_columnContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_constraint"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTable_constraint([NotNull] SqlParser.Table_constraintContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.qualified_table_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitQualified_table_name([NotNull] SqlParser.Qualified_table_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.ordering_term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrdering_term([NotNull] SqlParser.Ordering_termContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.result_column"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitResult_column([NotNull] SqlParser.Result_columnContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_or_subquery"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTable_or_subquery([NotNull] SqlParser.Table_or_subqueryContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitJoin_clause([NotNull] SqlParser.Join_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_right"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitJoin_right([NotNull] SqlParser.Join_rightContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitJoin_operator([NotNull] SqlParser.Join_operatorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.join_constraint"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitJoin_constraint([NotNull] SqlParser.Join_constraintContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.select_core"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelect_core([NotNull] SqlParser.Select_coreContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.where_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhere_clause([NotNull] SqlParser.Where_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.groupby_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGroupby_clause([NotNull] SqlParser.Groupby_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.having_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHaving_clause([NotNull] SqlParser.Having_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.compound_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompound_operator([NotNull] SqlParser.Compound_operatorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.signed_number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSigned_number([NotNull] SqlParser.Signed_numberContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.literal_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral_value([NotNull] SqlParser.Literal_valueContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.unary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnary_operator([NotNull] SqlParser.Unary_operatorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.error_message"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitError_message([NotNull] SqlParser.Error_messageContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.module_argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitModule_argument([NotNull] SqlParser.Module_argumentContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_alias"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitColumn_alias([NotNull] SqlParser.Column_aliasContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.keyword"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitKeyword([NotNull] SqlParser.KeywordContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitName([NotNull] SqlParser.NameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.function_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunction_name([NotNull] SqlParser.Function_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.database_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDatabase_name([NotNull] SqlParser.Database_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTable_name([NotNull] SqlParser.Table_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_or_index_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTable_or_index_name([NotNull] SqlParser.Table_or_index_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.new_table_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNew_table_name([NotNull] SqlParser.New_table_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.column_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitColumn_name([NotNull] SqlParser.Column_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.collation_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCollation_name([NotNull] SqlParser.Collation_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.foreign_table"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForeign_table([NotNull] SqlParser.Foreign_tableContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.index_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIndex_name([NotNull] SqlParser.Index_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.trigger_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTrigger_name([NotNull] SqlParser.Trigger_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.view_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitView_name([NotNull] SqlParser.View_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.module_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitModule_name([NotNull] SqlParser.Module_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.pragma_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPragma_name([NotNull] SqlParser.Pragma_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.savepoint_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSavepoint_name([NotNull] SqlParser.Savepoint_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.table_alias"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTable_alias([NotNull] SqlParser.Table_aliasContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.transaction_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTransaction_name([NotNull] SqlParser.Transaction_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlParser.any_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAny_name([NotNull] SqlParser.Any_nameContext context);
}
} // namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlParse