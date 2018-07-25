using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ExcelDataAdapter : DbDataAdapter, IDbDataAdapter
    {
        private ExcelCommand _selectCommand;

        public ExcelDataAdapter()
        {
        }
        public ExcelDataAdapter(ExcelCommand selectCommand)
        {
            this._selectCommand = selectCommand;
        }
        public ExcelDataAdapter(string selectCommandText, ExcelConnection selectConnection)
        {
            this._selectCommand = new ExcelCommand(selectCommandText, selectConnection);
        }
        public ExcelDataAdapter(string selectCommandText, string selectConnectionString)
        {
            ExcelConnection connection = new ExcelConnection(selectConnectionString);
            this._selectCommand = new ExcelCommand(selectCommandText, connection);
        }

        public new ExcelCommand SelectCommand
        {
            get
            {
                return this._selectCommand;
            }
            set
            {
                this._selectCommand = value;
            }
        }

        IDbCommand IDbDataAdapter.SelectCommand
        {
            get
            {
                return this._selectCommand;
            }
            set
            {
                this._selectCommand = (ExcelCommand)value;
            }
        }

    }
}
