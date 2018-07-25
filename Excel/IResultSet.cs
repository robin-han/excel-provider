using GrapeCity.Documents.Spread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public interface IResultSet
    {
        void Close();

        List<ColumnInfo> GetColumns();
        ColumnInfo GetColumn(int ordinal);

        bool Next();

        object GetValue(int ordinal);
        object GetValue(string tableName, string columnName);

        object GetParameter(string name);
    }
}
