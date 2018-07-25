using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class SubQueryExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.SubQuery;

        public IList<IExpression> SubExpressions { get { return null; } set => throw new NotSupportedException(); }

        public SelectStatement Statement { get; set; }
    }
}
