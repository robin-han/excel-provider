using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ExcelDataReader : DbDataReader
    {
        private bool _hasRows;
        private bool _skip;
        private IResultSet _resultSet;

        internal ExcelDataReader(IResultSet resultSet)
        {
            this._resultSet = resultSet;
            this._hasRows = false;
            this._skip = false;

            if (resultSet != null && resultSet.Next())
            {
                this._hasRows = true;
                this._skip = true;
            }
        }

        public override void Close()
        {
            this._resultSet.Close();
            this._resultSet = null;
        }

        public override bool GetBoolean(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToBoolean(value);
        }

        public override byte GetByte(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToByte(value);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotSupportedException("GetBytes not supported.");
        }

        public override char GetChar(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);

            if (value == null)
            {
                return '\0';
            }
            if (!(value is string) || (value.ToString().Length != 1))
            {
                throw new InvalidCastException();
            }
            return Convert.ToChar(value);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("DataReader buffer is null.");
            }
            string str = this.GetString(ordinal);
            if (string.IsNullOrEmpty(str))
            {
                return 0L;
            }
            int num2 = Math.Min(Math.Min((int)(str.Length - ((int)dataOffset)), (int)(buffer.Length - bufferOffset)), length);
            for (int i = 0; i < num2; i++)
            {
                buffer[i + bufferOffset] = str[i + ((int)dataOffset)];
            }
            return num2;
        }

        public override string GetDataTypeName(int ordinal)
        {
            return GetFieldType(ordinal).FullName;
        }

        public override DateTime GetDateTime(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToDateTime(value);
        }

        public override decimal GetDecimal(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToDecimal(value);
        }

        public override double GetDouble(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToDouble(value);
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return new DbEnumerator(this);
        }

        public override Type GetFieldType(int ordinal)
        {
            ColumnInfo column = this._resultSet.GetColumn(ordinal);
            return DataTypeUtil.GetDotNetType(column.DataType);
        }

        public override float GetFloat(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToSingle(value);
        }

        public override Guid GetGuid(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return new Guid(value.ToString());
        }

        public override short GetInt16(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToInt16(value);
        }

        public override int GetInt32(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToInt32(value);
        }

        public override long GetInt64(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            this.VerifyNotNull(value);

            return Convert.ToInt64(value);
        }

        public override string GetName(int ordinal)
        {
            return this._resultSet.GetColumn(ordinal).ColumnAlias;
        }

        public override int GetOrdinal(string name)
        {
            ColumnInfo column = this._resultSet.GetColumns().Find((c) => string.Compare(c.ColumnAlias, name, true) == 0);
            if (column == null)
            {
                throw new ExcelException(string.Format("Cannot find column with name {0}", name));
            }

            return column.Ordinal;
        }

        public override DataTable GetSchemaTable()
        {
            DataTable dt = new DataTable();
            var columnInfos = this._resultSet.GetColumns();

            for (int i = 0; i < columnInfos.Count; i++)
            {
                DataRow row = dt.NewRow();
                ColumnInfo info = columnInfos[i];

                row["TableName"] = info.TableAlias;
                row["ColumnName"] = info.ColumnAlias;

                row["BaseTableName"] = info.TableName;
                row["BaseColumnName"] = info.ColumnName;

                row["Ordinal"] = info.Ordinal;
                row["DataType"] = info.DataType;
                //...
            }

            return dt;
        }

        public override string GetString(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);
            if (value == null)
            {
                return null;
            }
            return Convert.ToString(value);
        }

        public override object GetValue(int ordinal)
        {
            object value = this._resultSet.GetValue(ordinal);

            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        public override int GetValues(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("DataReader values are null.");
            }

            int num = Math.Min(values.Length, this.FieldCount);
            for (int i = 0; i < num; i++)
            {
                values[i] = this.GetValue(i);
            }
            return num;
        }

        public override bool HasRows
        {
            get
            {
                return this._hasRows;
            }
        }

        public override bool IsClosed
        {
            get
            {
                return this._resultSet == null;
            }
        }

        public override bool IsDBNull(int ordinal)
        {
            return (this.GetValue(ordinal) is DBNull);
        }

        public override bool NextResult()
        {
            return false;
        }

        public override bool Read()
        {
            if (this.IsClosed)
            {
                throw new InvalidOperationException("Invalid attempt to call NextResult when reader is closed.");
            }


            if (this._hasRows && this._skip)
            {
                this._skip = false;
                return true;
            }

            return this._resultSet.Next();
        }

        private void VerifyNotNull(object value)
        {
            if (value == null)
            {
                throw new ExcelNullValueException();
            }
        }


        public override int Depth
        {
            get
            {
                return 0;
            }
        }

        public override int FieldCount
        {
            get
            {
                return this._resultSet.GetColumns().Count;
            }
        }

        public override object this[string name]
        {
            get
            {
                return this.GetValue(this.GetOrdinal(name));
            }
        }

        public override object this[int ordinal]
        {
            get
            {
                return this.GetValue(ordinal);
            }
        }

        public override int RecordsAffected
        {
            get
            {
                return -1;
            }
        }

    }
}
