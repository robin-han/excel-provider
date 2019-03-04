using GrapeCity.Documents.Spread;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GrapeCity.Enterprise.Data.DataSource.Common;
using GrapeCity.Enterprise.Data.DataSource.Common.Expression;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    #region Class ExcelResultSet
    internal class ExcelResultSet : IResultSet
    {
        private SelectStatement _selectStatement;
        private ExcelCommand _command;
        private ExcelEvalVisitor _evaluator;

        private int _position;
        private int _recordCount;
        private object[] _currentRecord;

        private List<ColumnInfo> _columns;
        private IData _data;

        public ExcelResultSet(ISQLStatement sqlStatement, ExcelCommand command)
        {
            if (sqlStatement.Type != StatementType.Select)
            {
                throw new NotSupportedException(sqlStatement.ToString());
            }

            SelectStatement selectStatement = (SelectStatement)sqlStatement;
            if (!command.Connection.HasTable(selectStatement.Table.Name))
            {
                throw new ExcelException("Cannot find table " + selectStatement.Table.Name);
            }

            this.Reset();

            this._selectStatement = selectStatement;
            this._command = command;
            this._evaluator = new ExcelEvalVisitor(new ExcelEvalContext(this));
        }

        public void Close()
        {
            this.Reset();

            this._selectStatement = null;
            this._command = null;
            this._evaluator = null;
        }
        private void Reset()
        {
            this._position = -1;
            this._recordCount = 0;
            this._currentRecord = null;

            this._columns = null;
            this._data = null;
        }

        public ColumnInfo GetColumn(int ordinal)
        {
            List<ColumnInfo> columns = this.GetColumns();
            if (ordinal < 0 || ordinal >= columns.Count)
            {
                throw new IndexOutOfRangeException("Index was outside the bounds.");
            }
            return columns[ordinal];
        }

        public List<ColumnInfo> GetColumns()
        {
            if (this._columns != null)
            {
                return this._columns;
            }

            List<ColumnInfo> columns = new List<ColumnInfo>();

            string tableName = this._selectStatement.Table.Name;
            List<ColumnInfo> sheetColumns = this._command.Connection.GetWorksheetColumns(tableName);
            for (int i = 0; i < sheetColumns.Count; i++)
            {
                ColumnInfo sheetColumn = sheetColumns[i];
                ColumnExpression columnExpr = this.GetColumnExpr(sheetColumn.TableAlias, sheetColumn.ColumnName);
                if (columnExpr != null)
                {
                    ColumnInfo column = (ColumnInfo)sheetColumn.Clone();
                    column.Ordinal = columns.Count;
                    column.ColumnAlias = columnExpr.Alias;
                    column.TableAlias = columnExpr.TableAlias;
                    columns.Add(column);
                }
            }

            this._columns = columns;
            return columns;
        }

        public bool Next()
        {
            this.VerifyConnection();

            //prepare data
            if (this._position < 0 && !this.PrepareData())
            {
                return false;
            }


            SelectStatement select = this._selectStatement;
            int recordCount = this._data.GetRowCount();

            int limit = select.Limit.HasValue ? Math.Min(recordCount, select.Limit.Value) : recordCount;
            int offset = select.Offset.HasValue ? select.Offset.Value : 0;

            //skip some
            if (this._position < 0 && offset > 0)
            {
                int count = 0;
                while (count < offset && ++this._position < recordCount)
                {
                    this._currentRecord = this._data.GetRowData(this._position, select.Columns);
                    if (this.Evaluate(select))
                    {
                        count++;
                    }
                }
            }

            //
            while (this._recordCount < limit && ++this._position < recordCount)
            {
                this._currentRecord = this._data.GetRowData(this._position, select.Columns);
                if (this.Evaluate(select))
                {
                    this._recordCount++;
                    return true;
                }
            }

            this._currentRecord = null;
            return false;
        }

        private bool PrepareData()
        {
            SelectStatement select = this._selectStatement;

            //resultset's read
            if (select.IsJoin || select.IsCompound || select.Orderby.Count > 0 || select.Groupby != null)
            {
                //TODO:
                throw new NotImplementedException();
            }

            string tableName = select.Table.Name;
            List<ColumnInfo> dataColumns = this._command.Connection.GetWorksheetColumns(tableName);
            IRange dataRange = this._command.Connection.GetTableDataRange(tableName);
            if (dataRange != null && dataColumns.Count > 0)
            {
                this._data = new RangeData(dataRange, dataColumns, this._evaluator);
                return true;
            }
            return false;
        }

        public object GetValue(int ordinal)
        {
            this.VerifyRecord();

            object[] values = this._currentRecord;
            if (ordinal < 0 || ordinal >= values.Length)
            {
                throw new IndexOutOfRangeException("Index was outside the bounds.");
            }

            return values[ordinal];
        }

        public object GetValue(string tableName, string columnName)
        {
            //TODO: now only assume in one table
            return this._data.GetValue(this._position, columnName);
        }

        public object GetParameter(string parameterName)
        {
            ExcelParameterCollection parameters = this._command.Parameters;
            if (parameters.Contains(parameterName))
            {
                return parameters[parameterName];
            }
            if (parameterName.StartsWith("@") && parameters.Contains(parameterName.Substring(1)))
            {
                return parameters[parameterName.Substring(1)];
            }
            throw new ExcelException(string.Format("Cannot get parameter {0}.", parameterName));
        }

        internal ExcelCommand Command
        {
            get { return this._command; }
        }


        private bool Evaluate(SelectStatement select)
        {
            if (select.Where == null || (bool)this._evaluator.Evaluate(select.Where))
            {
                return true;
            }
            return false;
        }


        private ColumnExpression GetColumnExpr(string tableName, string columnName)
        {
            SelectStatement select = this._selectStatement;
            IList<ColumnExpression> columns = select.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                ColumnExpression columnExpr = columns[i];

                string columnExprTableName = string.IsNullOrEmpty(columnExpr.TableName) ? select.Table.Name : columnExpr.TableName;
                if (string.Compare(columnExprTableName, tableName, true) != 0)
                {
                    continue;
                }

                if (columnExpr.ColumnName == "*" || string.Compare(columnExpr.ColumnName, columnName, true) == 0)
                {
                    return columnExpr;
                }
            }

            return null;
        }

        private void VerifyConnection()
        {
            if (this._command.Connection.State != System.Data.ConnectionState.Open)
            {
                throw new InvalidOperationException("Invalid attempt to read when reader is closed.");
            }
        }
        private void VerifyRecord()
        {
            if (this._currentRecord == null)
            {
                throw new InvalidOperationException("Invalid attempt to read when no data is present.");
            }
        }

    }
    #endregion

    #region Interface IData
    internal interface IData
    {
        int GetRowCount();
        object[] GetRowData(int position, IList<ColumnExpression> selectColumns);

        object GetValue(int position, string columnName);
    }
    #endregion

    #region Class RangeData
    internal class RangeData : IData
    {
        #region Fields
        private List<ColumnInfo> _dataColumns;
        private IRange _dataRange;
        private ExcelEvalVisitor _evaluator;

        private Dictionary<string, ColumnInfo> _columnDict;
        #endregion

        #region Constructor
        public RangeData(IRange dataRange, List<ColumnInfo> dataColumns, ExcelEvalVisitor evaluator)
        {
            this._dataRange = dataRange;
            this._dataColumns = dataColumns;
            this._evaluator = evaluator;

            this.Init();
        }
        #endregion

        #region Methods
        private void Init()
        {
            this._columnDict = new Dictionary<string, ColumnInfo>();
            foreach (ColumnInfo columnInfo in this._dataColumns)
            {
                this._columnDict.Add(columnInfo.ColumnName.ToLower(), columnInfo);
            }
        }

        public int GetRowCount()
        {
            return this._dataRange.Rows.Count;
        }

        public object[] GetRowData(int position, IList<ColumnExpression> selectColumns)
        {
            object[] rowData;

            bool all = selectColumns.FirstOrDefault(c => c.ColumnName == "*") != null;
            if (all)
            {
                IRange row = this._dataRange.Rows[position];
                List<ColumnInfo> columns = this._dataColumns;
                int columnCount = columns.Count;

                rowData = new object[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    rowData[i] = GetCellValue(row.Cells[columns[i].Column - row.Column], columns[i].DataType, row.Worksheet.SheetView.DisplayZeros);
                }
            }
            else
            {
                ExcelEvalVisitor evaluator = this._evaluator;
                int columCount = selectColumns.Count;

                rowData = new object[columCount];
                for (int i = 0; i < columCount; i++)
                {
                    rowData[i] = evaluator.Evaluate(selectColumns[i]);
                }
            }

            return rowData;
        }

        public object GetValue(int position, string columnName)
        {
            IRange row = this._dataRange.Rows[position];
            ColumnInfo column = this._columnDict[columnName.ToLower()];
            if (column == null || string.Compare(row.Worksheet.Name, column.TableName, true) != 0)
            {
                throw new ExcelException(string.Format("The column \"{0}\" does not in table \"{1}\".", columnName, row.Worksheet.Name));
            }

            return GetCellValue(row.Cells[column.Column - row.Column], column.DataType, row.Worksheet.SheetView.DisplayZeros);
        }

        private object GetCellValue(IRange cell, ExcelDbType expectedType, bool displayZero = true)
        {
            object v = cell.Value;
            if (v == null)
            {
                return null;
            }

            if (!displayZero)
            {
                if((v is double && (double)v == 0) || (v is TimeSpan && (TimeSpan)v == TimeSpan.Zero))
                {
                    return null;
                }
            }

            //
            if (v is CalcError)
            {
                switch ((CalcError)v)
                {
                    case CalcError.Null:
                        return "#NULL!";
                    case CalcError.Div0:
                        return "#DIV/0!";
                    case CalcError.Value:
                        return "#VALUE!";
                    case CalcError.Ref:
                        return "#REF!";
                    case CalcError.Name:
                        return "#NAME?";
                    case CalcError.Num:
                        return "#NUM!";
                    case CalcError.NA:
                        return "#N/A";
                    case CalcError.GettingData:
                        return "#GETTING_DATA";

                    default:
                        return null;
                }
            }

            //
            try
            {
                switch (expectedType)
                {
                    case ExcelDbType.Boolean:
                        return DataTypeUtil.ToBoolean(v);

                    case ExcelDbType.Date:
                    case ExcelDbType.DateTime:
                        return DataTypeUtil.ToDateTime(v);

                    case ExcelDbType.Time:
                        return DataTypeUtil.ToTimeSpan(v);

                    case ExcelDbType.Decimal:
                    case ExcelDbType.Double:
                    case ExcelDbType.Single:
                        return DataTypeUtil.ToDouble(v);

                    case ExcelDbType.SByte:
                    case ExcelDbType.Int16:
                    case ExcelDbType.Int32:
                        return DataTypeUtil.ToInt(v);

                    case ExcelDbType.Int64:
                        return DataTypeUtil.ToLong(v);

                    case ExcelDbType.Object:
                        return v;

                    case ExcelDbType.String:
                        return DataTypeUtil.ToString(v);

                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
    #endregion

    #region Class ResultSetData
    internal class ResultSetData : IData
    {
        public ResultSetData()
        {
            //TODO:
        }

        public int GetRowCount()
        {
            throw new NotImplementedException();
        }

        public object[] GetRowData(int position, IList<ColumnExpression> selectColumns)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int position, string columnName)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}