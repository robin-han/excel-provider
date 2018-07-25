using GrapeCity.Enterprise.Data.DataSource.Common.Expression;
using System.Collections.Generic;

namespace GrapeCity.Enterprise.Data.DataSource.Common
{
    public class UpdateStatement : ISQLStatement
    {
        public StatementType Type => StatementType.Update;

        public IList<AssignExpression> Assignments { get; set; }

        public TableName Table { get; set; }

        public IExpression Where { get; set; }
    }
}
