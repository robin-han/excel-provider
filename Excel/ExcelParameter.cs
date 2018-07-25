using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public sealed class ExcelParameter : DbParameter
    {
        private DbType _dbType;
        private bool _dbTypeSet;
        private object _value;
        private bool _isNullable;
        //private ParameterDirection _direction;
        private string _parameterName;

        private string _sourceColumn;
        private bool _sourceColumnNullMapping;
        private int _size;

        public ExcelParameter()
        {
            this.ResetDbType();
        }

        public ExcelParameter(string name, object value) : this()
        {
            this.ParameterName = name;
            this.Value = value;
        }

        public ExcelParameter(string name, DbType dbType) : this()
        {
            this.ParameterName = name;
            this.DbType = DbType;
        }

        public ExcelParameter(string name, DbType dbType, string sourceColumn) : this(name, dbType)
        {
            this.SourceColumn = sourceColumn;
        }

        public override DbType DbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                this._dbType = value;
                this._dbTypeSet = true;
            }
        }

        public override ParameterDirection Direction
        {
            get
            {
                return ParameterDirection.Input;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override bool IsNullable
        {
            get
            {
                return this._isNullable;
            }
            set
            {
                this._isNullable = value;
            }
        }

        public override string ParameterName
        {
            get
            {
                return this._parameterName;
            }
            set
            {
                this._parameterName = value;
            }
        }

        public override int Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        public override string SourceColumn
        {
            get
            {
                return this._sourceColumn;
            }
            set
            {
                this._sourceColumn = value;
            }
        }

        public override bool SourceColumnNullMapping
        {
            get
            {
                return this._sourceColumnNullMapping;
            }
            set
            {
                this._sourceColumnNullMapping = value;
            }
        }

        public override object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                if (!this._dbTypeSet)
                {
                    this.DbType = this.GetDbType(value);
                }
            }
        }

        public override DataRowVersion SourceVersion
        {
            get
            {
                return DataRowVersion.Default;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override void ResetDbType()
        {
            this._value = null;
            this._dbType = DbType.Object;
            this._dbTypeSet = false;

            this._isNullable = true;
            this._parameterName = null;
            this._sourceColumn = null;
        }

        private DbType GetDbType(object value)
        {
            if (value == null)
            {
                return DbType.Object;
            }
            return DataTypeUtil.GetDbType(value.GetType());
        }

    }
}
