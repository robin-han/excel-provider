using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ExcelException : DbException
    {
        public ExcelException()
        {
        }

        public ExcelException(string message)
            : base(message)
        {
        }
    }
}
