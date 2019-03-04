using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public interface IExpression
    {
        ExpressionType Type { get; }
        IList<IExpression> SubExpressions { get; set; }
    }
}
