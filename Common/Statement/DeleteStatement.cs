using GrapeCity.Enterprise.Data.DataSource.Common.Expression;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common
{
    public class DeleteStatement : ISQLStatement
    {
        public StatementType Type => StatementType.Delete;

        public TableName Table { get; set; }

        public IExpression Where { get; set; }
    }
}
