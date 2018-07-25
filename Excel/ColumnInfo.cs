using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ColumnInfo : ICloneable
    {
        private int _column;
        private int _ordinal;
        private ExcelDbType _dataType;
        private string _columnAlias;
        private string _columnName;
        private string _tableName;
        private string _tableAlias;

        public ColumnInfo():this(-1, -1, string.Empty, ExcelDbType.Object)
        {
        }
        public ColumnInfo(int ordinal, int column, string columnName, ExcelDbType dataType)
        {
            this._ordinal = ordinal;
            this._column = column;
            this._columnName = columnName;
            this._dataType = dataType;

            this._columnAlias = string.Empty;
            this._tableName = string.Empty;
            this._tableAlias = string.Empty;
        }

        public int Column
        {
            get
            {
                return this._column;
            }
            internal set
            {
                this._column = value;
            }
        }

        public int Ordinal
        {
            get
            {
                return this._ordinal;
            }
            internal set
            {
                this._ordinal = value;
            }
        }

        public ExcelDbType DataType
        {
            get
            {
                return this._dataType;
            }
            internal set
            {
                this._dataType = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return this._columnName;
            }
            internal set
            {
                this._columnName = value;
            }
        }

        public string ColumnAlias
        {
            get
            {
                if (string.IsNullOrEmpty(this._columnAlias))
                {
                    return this._columnName;
                }
                return this._columnAlias;
            }
            internal set
            {
                this._columnAlias = value;
            }
        }

        public string TableName
        {
            get
            {
                return this._tableName;
            }
            internal set
            {
                this._tableName = value;
            }
        }

        public string TableAlias
        {
            get
            {
                if (string.IsNullOrEmpty(this._tableAlias))
                {
                    return this._tableName;
                }
                return this._tableAlias;
            }
            internal set
            {
                this._tableAlias = value;
            }
        }

        public object Clone()
        {
            return new ColumnInfo() {
                Ordinal = this._ordinal,
                Column = this._column,
                DataType = this._dataType,
                ColumnName = this._columnName,
                ColumnAlias = this._columnAlias,
                TableName = this._tableName,
                TableAlias = this._tableAlias
            };
        }
    }
}
