using GrapeCity.Enterprise.Data.DataSource.Common.Expression;
using System.Collections.Generic;

namespace GrapeCity.Enterprise.Data.DataSource.Common
{
    public class SelectStatement : ISQLStatement
    {
        public StatementType Type => StatementType.Select;
        public IList<ColumnExpression> Columns { get; set; }
        public TableName Table { get; set; }
        public IExpression Where { get; set; }
        public GroupbyClause Groupby { get; set; }
        public IList<OrderbyColumn> Orderby { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public bool IsJoin { get; set; } = false;
        public IList<JoinTable> JoinTables { get; set; }
        public bool IsCompound { get; set; } = false;
        public IList<CompoundSelectStatement> CompoundStatements { get; set; }

    }

    public class CompoundSelectStatement
    {
        public CompoundType CompoundType { get; set; } = CompoundType.UnionAll;
        public SelectStatement Statement { get; set; }
    }

    public enum CompoundType
    {
        Union,
        UnionAll
    }

    public enum JoinType
    {
        Left,
        LeftOuter,
        Inner,
        Cross,
        Unknown
    }

    public class JoinTable
    {
        public TableName Table { get; set; }
        public JoinType JoinType { get; set; }
        public IExpression JoinContraints { get; set; }
    }

    public class OrderbyColumn
    {
        public ColumnExpression Column { get; set; }
        public string Order { get; set; }
    }

    public class GroupbyClause
    {
        public IList<ColumnExpression> Columns { get; set; }
        public IExpression Having { get; set; }
    }

}
