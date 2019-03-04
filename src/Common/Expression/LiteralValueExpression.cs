using System.Collections.Generic;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class LiteralValueExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.LiteralValue;
        public LiteralValueType ValueType { get; set; }
        public string Value { get; set; }
        public IList<IExpression> SubExpressions { get; set; }
    }

    public enum LiteralValueType
    {
        Number,
        String,
        Boolean,
        Blob,
        Null,
    }
}
