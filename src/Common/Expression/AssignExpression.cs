using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class AssignExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.Assign;

        public ColumnExpression Column { get; set; }
        public IExpression Value { get; set; }

        public IList<IExpression> SubExpressions { get { return null; } set => throw new NotSupportedException(); }
    }
}
