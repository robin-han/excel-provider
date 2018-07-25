using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public interface ISQLStatement
    {
        StatementType Type { get; }
    }
}
