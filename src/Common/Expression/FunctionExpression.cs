using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class FunctionExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.Function;

        public string FunctionName { get; set; }

        public IList<IExpression> SubExpressions { get; set; }
    }
}
