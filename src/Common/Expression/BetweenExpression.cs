using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class BetweenExpression : IExpression
    {
        private IList<IExpression> _subExpression;
        public ExpressionType Type => ExpressionType.Between;

        public IList<IExpression> SubExpressions { get => _subExpression; set => _subExpression = value; }
    }
}
