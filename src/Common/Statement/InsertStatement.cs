using GrapeCity.Enterprise.Data.DataSource.Common.Expression;
using System.Collections.Generic;

namespace GrapeCity.Enterprise.Data.DataSource.Common
{
    public class InsertStatement : ISQLStatement
    {
        public StatementType Type => StatementType.Insert;

        public TableName Table { get; set; }

        public IList<string> Columns { get; set; }

        public IList<IList<IExpression>> Values { get; set; }
    }
}
