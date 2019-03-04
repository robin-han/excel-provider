using GrapeCity.Enterprise.Data.DataSource.Common.Expression;
using GrapeCity.Enterprise.Data.DataSource.Common.SqlParse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ExcelCommand : DbCommand
    {
        private string _cmdText;
        private ExcelConnection _connection;
        private ExcelParameterCollection _parameters;

        public ExcelCommand()
            : this(string.Empty, null)
        {
        }
        public ExcelCommand(string cmdText)
            : this(cmdText, null)
        {
        }
        public ExcelCommand(string cmdText, ExcelConnection connection)
        {
            this._cmdText = cmdText;
            this._connection = connection;
            this._parameters = new ExcelParameterCollection();
        }

        public override void Cancel()
        {
            throw new NotSupportedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            return new ExcelParameter();

        }

        public new ExcelParameter CreateParameter()
        {
            return new ExcelParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return this.ExecuteDataReader(behavior);
        }

        private ExcelDataReader ExecuteDataReader(CommandBehavior behavior)
        {
            this.VerifyConnectionAndOpen();

            GCSqlParser parser = new GCSqlParser();
            ISQLStatement sqlStatement = parser.Parse(this._cmdText);
            ExcelResultSet resultSet = new ExcelResultSet(sqlStatement, this);
            return new ExcelDataReader(resultSet);
        }

        public new ExcelDataReader ExecuteReader()
        {
            return this.ExecuteReader(CommandBehavior.Default);
        }

        public new ExcelDataReader ExecuteReader(CommandBehavior behavior)
        {
            return (ExcelDataReader)base.ExecuteReader(behavior);
        }

        public override int ExecuteNonQuery()
        {
            throw new NotSupportedException();
        }

        public override object ExecuteScalar()
        {
            this.VerifyConnectionAndOpen();

            using (ExcelDataReader reader = this.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue(0);
                }
            }

            return DBNull.Value;
        }

        public override void Prepare()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._cmdText = string.Empty;
                this._connection = null;
            }

            base.Dispose(disposing);
        }

        public override string CommandText
        {
            get
            {
                return this._cmdText;
            }
            set
            {
                this._cmdText = value;
            }
        }

        public override int CommandTimeout
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public override CommandType CommandType
        {
            get
            {
                return CommandType.Text;
            }
            set
            {
                if (value != CommandType.Text)
                {
                    throw new NotSupportedException(value.ToString());
                }
            }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return this.Connection;
            }
            set
            {
                this.Connection = (ExcelConnection)value;
            }
        }

        public new ExcelConnection Connection
        {
            get
            {
                return this._connection;
            }
            set
            {
                this._connection = value;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return this.Parameters;
            }
        }

        public new ExcelParameterCollection Parameters
        {
            get
            {
                return this._parameters;
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public override bool DesignTimeVisible
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }


        private void VerifyConnectionAndOpen()
        {
            if (this.Connection == null)
            {
                throw new NullReferenceException("Connection is null.");
            }

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }
        }
    }
}
