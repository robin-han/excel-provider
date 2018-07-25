using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ExcelNullValueException : DbException
    {
        public ExcelNullValueException()
        {
        }

        public ExcelNullValueException(string message)
            : base(message)
        {
        }
    }
}
