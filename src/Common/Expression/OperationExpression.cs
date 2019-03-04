using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class OperationExpression : IExpression
    {
        private IList<IExpression> _subExpressions;

        public ExpressionType Type => ExpressionType.Operation;
        public string Operator { get; set; }
        public OperationType OperationType { get; set; }
        public IList<IExpression> SubExpressions { get => _subExpressions; set => _subExpressions = value; }
    }

    public enum OperationType
    {
        Unary,
        Binary
    }
}
