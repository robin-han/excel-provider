using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public class ColumnExpression : IExpression
    {
        private IList<IExpression> _subExpressions;

        public ExpressionType Type => ExpressionType.Column;
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string TableAlias { get; set; }
        public string Alias { get; set; }
        public bool IsExpression { get; set; }
        public IList<IExpression> SubExpressions { get => _subExpressions; set => _subExpressions = value; }
    }
}
