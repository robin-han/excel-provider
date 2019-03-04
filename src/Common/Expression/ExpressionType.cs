using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Common.Expression
{
    public enum ExpressionType
    {
        LiteralValue,
        Column,
        BindParameter,
        Operation,
        Assign,
        Function,
        Keyword,
        Between,
        List,
        SubQuery,
        Param,
    }
}
