using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class KeywordExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.Keyword;

        public IList<IExpression> SubExpressions { get { return null; } set => throw new NotSupportedException(); }

        public string Keyword { get; set; }
    }
}
