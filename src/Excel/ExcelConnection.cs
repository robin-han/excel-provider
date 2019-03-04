using GrapeCity.Documents.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ExcelConnection : DbConnection
    {
        private ExcelConnectionStringBuilder _connString;
        private ConnectionState _state;
        private Workbook _spread;
        private List<ColumnInfo> _columns;

        public ExcelConnection() : this(string.Empty)
        {
        }

        public ExcelConnection(string connectionString)
        {
            this._connString = new ExcelConnectionStringBuilder();
            this.ConnectionString = connectionString;
        }

        protected override DbCommand CreateDbCommand()
        {
            return new ExcelCommand("", this);
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return null;
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotSupportedException();
        }

        public override void Open()
        {
            if (this._state == ConnectionState.Open)
            {
                return;
            }

            try
            {
                this._state = ConnectionState.Connecting;
                this._columns = null;

                LicenseManager.SetLicense("GrapeCity-Internal-Use-Only,554358313849458#A0SIKISP3EkT4VnW7Y5K0JFbO3EZyA5bZhnRkxkZLlFbENjR6FVRMRUO7QlTrQ5dvNDbIR5TvBXdkpmRwFlSMBlc7oGeKNDWvMnT9sySmJlclVWbTdmTxcmVmFzZiojITJCL6UjM9UzNxIDO0IicfJye35XX3JSQwkzQiojIDJCLiEjVgUmcvNkLkFWZyB7UiojIOJyebpjIkJHUiwiIzUjN4EDMggDM6AzNxAjMiojI4J7QiwiI4NXZUJXZ49WdoJiOiEmTDJCLigTN4kDN8MTMzgTNzQTN5IiOiQWSisIONQ");
                Workbook workbook = new Workbook();
                //workbook.Open(this._connString.DataSource);
                workbook.Open(this._connString.DataSource, null, new OpenOptions { DoNotRecalculateAfterOpened = true });
                this._spread = workbook;

                this._state = ConnectionState.Open;
            }
            catch (Exception ex)
            {
                this._state = ConnectionState.Closed;
                throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
            base.Dispose(disposing);
        }
        public override void Close()
        {
            this._state = ConnectionState.Closed;
            this._spread = null;
            this._columns = null;
        }

        public override DataTable GetSchema()
        {
            return this.GetSchema(ExcelSchema.MetaDataCollections);
        }

        public override DataTable GetSchema(string collection)
        {
            return this.GetSchema(collection, null);
        }

        public override DataTable GetSchema(string collection, string[] restrictions)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (ConnectionState.Open != this._state)
            {
                throw new InvalidOperationException("The ExcelConnection is closed.");
            }

            if (string.Compare(ExcelSchema.MetaDataCollections, collection, true) == 0)
            {
                return this.GetMetaDataCollectionsSchema();
            }
            else if (string.Compare(ExcelSchema.Tables, collection, true) == 0)
            {
                return this.GetTablesSchema();
            }
            else if (string.Compare(ExcelSchema.Columns, collection, true) == 0)
            {
                return this.GetColumnsSchema();
            }
            else if (string.Compare(ExcelSchema.DataTypes, collection, true) == 0)
            {
                return this.GetDataTypesSchema();
            }
            else
            {
                throw new NotSupportedException($"The schema collection '{collection}' is not supported in Excel data source.");
            }
        }

        private DataTable GetMetaDataCollectionsSchema()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("CollectionName");
            dt.Columns.Add("NumberOfRestrictions");
            dt.Columns.Add("NumberOfIdentifierParts");

            dt.Rows.Add(ExcelSchema.MetaDataCollections, 0, 0);
            dt.Rows.Add(ExcelSchema.Tables, 4, 3);
            dt.Rows.Add(ExcelSchema.Columns, 4, 4);
            dt.Rows.Add(ExcelSchema.DataTypes, 0, 0);

            return dt;
        }

        private DataTable GetTablesSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TABLE_CATALOG", typeof(string));
            dt.Columns.Add("TABLE_SCHEMA", typeof(string));
            dt.Columns.Add("TABLE_NAME", typeof(string));
            dt.Columns.Add("TABLE_TYPE", typeof(string));

            foreach (IWorksheet sheet in this._spread.Worksheets)
            {
                dt.Rows.Add(this.Database, "Excel", sheet.Name, "BASE TABLE");
            }

            return dt;
        }

        private DataTable GetColumnsSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TABLE_CATALOG", typeof(string));
            dt.Columns.Add("TABLE_SCHEMA", typeof(string));
            dt.Columns.Add("TABLE_NAME", typeof(string));
            dt.Columns.Add("COLUMN_NAME", typeof(string));
            dt.Columns.Add("DATA_TYPE", typeof(string));
            dt.Columns.Add("IS_NULLABLE", typeof(string));
            dt.Columns.Add("DataType", typeof(string));
            dt.Columns.Add("ProviderDbType", typeof(ExcelDbType));

            List<ColumnInfo> columns = this.GetColumns();
            foreach (ColumnInfo column in columns)
            {
                dt.Rows.Add(
                    string.Empty,
                    "Excel",
                    column.TableAlias,
                    column.ColumnName,
                    column.DataType,
                    "YES",
                    DataTypeUtil.GetDotNetType(column.DataType).FullName,
                    column.DataType)
                ;
            }
            return dt;
        }

        private DataTable GetDataTypesSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TypeName", typeof(string));
            dt.Columns.Add("ProviderDbType", typeof(int));
            dt.Columns.Add("DataType", typeof(string));

            foreach (ExcelDbType dbType in Enum.GetValues(typeof(ExcelDbType)))
            {
                dt.Rows.Add(DataTypeUtil.GetTypeName(dbType), (int)dbType, DataTypeUtil.GetDotNetType(dbType).FullName);
            }

            return dt;
        }

        internal List<ColumnInfo> GetColumns()
        {
            if (this._columns != null)
            {
                return this._columns;
            }


            List<ColumnInfo> columnInfos = new List<ColumnInfo>();
            Dictionary<string, List<string>> columnNames = new Dictionary<string, List<string>>(); //process duplicate columnname

            foreach (IWorksheet sheet in this._spread.Worksheets)
            {
                string tableName = sheet.Name;
                if (!columnNames.ContainsKey(tableName))
                {
                    columnNames.Add(tableName, new List<string>());
                }

                IRange dataRange = this.GetWorksheetDataRange(sheet);
                IRange headerRange = this.GetWorksheetDataHeaderRange(sheet);
                if (dataRange == null && headerRange == null)
                {
                    continue;
                }
                IRange range = dataRange != null ? dataRange : headerRange;

                for (int c = 0; c < range.Columns.Count; c++)
                {
                    int column = range.Column + c;
                    string columnName = (headerRange == null || string.IsNullOrEmpty(headerRange[c].Text)) ? NumberToLetters(column + 1) : headerRange[c].Text;
                    columnName = this.GetName(columnName, columnNames[tableName]);
                    columnNames[tableName].Add(columnName);

                    ColumnInfo info = new ColumnInfo()
                    {
                        Ordinal = c,
                        Column = column,
                        DataType = DetectColumnDataType(range.Columns[c]),
                        ColumnName = columnName,
                        TableName = tableName
                    };
                    columnInfos.Add(info);
                }
            }

            this._columns = columnInfos;
            return columnInfos;
        }

        private string GetName(string newName, List<string> allNames)
        {
            List<int> indexes = new List<int>();
            bool hasDuplicated = false;

            foreach (string name in allNames)
            {
                if (string.Compare(newName, name, true) == 0)
                {
                    hasDuplicated = true;
                }
                else if (name.StartsWith(newName, true, CultureInfo.CurrentCulture) && int.TryParse(name.Substring(newName.Length), out int index))
                {
                    indexes.Add(index);
                }
            }

            if (!hasDuplicated)
            {
                return newName;
            }

            int id = 1;
            indexes.Sort();
            for (int i = 0; i < indexes.Count; i++, id++)
            {
                if (i + 1 != indexes[i])
                {
                    return newName + id;
                }
            }

            return newName + id;
        }

        internal List<ColumnInfo> GetWorksheetColumns(string sheetName)
        {
            IWorksheet sheet = this._spread.Worksheets[sheetName];
            return this.GetWorksheetColumns(sheet);
        }
        internal List<ColumnInfo> GetWorksheetColumns(IWorksheet sheet)
        {
            List<ColumnInfo> ret = new List<ColumnInfo>();
            foreach (ColumnInfo column in this.GetColumns())
            {
                if (string.Compare(column.TableName, sheet.Name, true) == 0)
                {
                    ret.Add(column);
                }
            }

            return ret;
        }

        internal IRange GetTableDataRange(string tableName)
        {
            IWorksheet sheet = this._spread.Worksheets[tableName];
            if (sheet != null)
            {
                return this.GetWorksheetDataRange(sheet);
            }
            return null;
        }
        private IRange GetWorksheetDataRange(IWorksheet sheet)
        {
            IRange dataRange = sheet.GetUsedRange(UsedRangeType.Data);
            if (this._connString.Header)
            {
                dataRange = dataRange.Rows.Count > 1 ? sheet.Range[dataRange.Row + 1, dataRange.Column, dataRange.Rows.Count - 1, dataRange.Columns.Count] : null;
            }
            return dataRange;
        }

        private IRange GetWorksheetDataHeaderRange(IWorksheet sheet)
        {
            if (this._connString.Header)
            {
                IRange dataRange = sheet.GetUsedRange(UsedRangeType.Data);
                return sheet.Range[dataRange.Row, dataRange.Column, 1, dataRange.Columns.Count];
            }
            return null;
        }

        internal bool HasTable(string tableName)
        {
            if(this._spread.Worksheets[tableName] != null)
            {
                return true;
            }
            return false;
        }

        private ExcelDbType DetectColumnDataType(IRange column)
        {
            var connString = this._connString;
            switch (connString.TypeDetectionScheme)
            {
                //
                case ExcelTypeDetectionScheme.RowScan:
                    {
                        bool hasNull = false;
                        bool hasString = false;
                     
                        bool hasNumber = false;
                        bool hasBoolean = false;
                        bool hasDateTime = false;

                        int rowScanDep = Math.Min(connString.RowScanDepth, column.Rows.Count);
                        for (int r = 0; r < rowScanDep; r++)
                        {
                            object value = column.Cells[r].Value;

                            if (value == null)
                            {
                                hasNull = true;
                            }
                            else if (value is string)
                            {
                                hasString = true;
                            }
                            else if (value is bool)
                            {
                                hasBoolean = true;
                            }
                            else if (DataTypeUtil.IsDate(value))
                            {
                                hasDateTime = true;
                            }
                            else if (DataTypeUtil.IsNumber(value))
                            {
                                hasNumber = true;
                            }
                            else //
                            {
                                hasString = true;
                            }

                            if (hasString)
                            {
                                break;
                            }
                        }

                        if (hasString || (hasNull && !hasNumber && !hasBoolean && !hasDateTime))
                        {
                            return ExcelDbType.String;
                        }
                        if (hasNumber && !hasBoolean && !hasDateTime)
                        {
                            return ExcelDbType.Double;
                        }
                        if (!hasNumber && hasBoolean && !hasDateTime)
                        {
                            return ExcelDbType.Boolean;
                        }
                        if (!hasNumber && !hasBoolean && hasDateTime)
                        {
                            return ExcelDbType.DateTime;
                        }

                        return ExcelDbType.String;
                    }
                //
                case ExcelTypeDetectionScheme.ColumnFormat:
                    {
                        return GetFormatType(column.NumberFormat);
                    }
                //
                case ExcelTypeDetectionScheme.None:
                default:
                    return ExcelDbType.String;
            }

        }

        private ExcelDbType GetFormatType(string numberFormat)
        {
            GeneralFormatter generalFormatter = new GeneralFormatter(numberFormat);
            switch (generalFormatter.FormatType)
            {
                case NumberFormatType.General:
                    return ExcelDbType.String;
                case NumberFormatType.Number:
                    return ExcelDbType.Double;
                case NumberFormatType.DateTime:
                    return ExcelDbType.DateTime;
                case NumberFormatType.Text:
                    return ExcelDbType.String;
                default:
                    return ExcelDbType.String;
            }
        }

        private static string NumberToLetters(int number)
        {
            StringBuilder sb = new StringBuilder();
            for (; number > 0; number = (number - 1) / 26)
            {
                sb.Insert(0, (char)('A' + (number - 1) % 26));
            }
            return sb.ToString();
        }


        public override string ConnectionString
        {
            get
            {
                return this._connString.ConnectionString;
            }
            set
            {
                if (this.State != ConnectionState.Closed)
                {
                    throw new ExcelException("The connection string cannot be modified after the connection has been opened.");
                }
                this._connString.ConnectionString = value;
            }
        }

        public override string DataSource
        {
            get
            {
                return this._connString.DataSource;
            }
        }

        public override string Database
        {
            get
            {
                return string.Empty;
            }
        }

        public override string ServerVersion
        {
            get
            {
                return "v1.0";
            }
        }

        public override ConnectionState State
        {
            get
            {
                return this._state;
            }
        }


        private class ExcelSchema
        {
            public static readonly string MetaDataCollections = "MetaDataCollections";

            public static readonly string Columns = "Columns";
            public static readonly string Tables = "Tables";
            public static readonly string DataTypes = "DataTypes";

        }
    }
}
