using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class ParamExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.Param;

        public IList<IExpression> SubExpressions { get { return null; } set => throw new NotSupportedException(); }

        public string ParamName { get; set; }
    }
}
