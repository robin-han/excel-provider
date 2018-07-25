using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class ListExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.List;

        public IList<IExpression> SubExpressions { get; set; }
    }
}
