using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public class ExcelConnectionStringBuilder : DbConnectionStringBuilder
    {
        private string _dataSource;
        private bool _header;
        private ExcelTypeDetectionScheme _typeDetectionScheme;
        private int _rowScanDepth;
        private StringComparer _comparer;

        public ExcelConnectionStringBuilder()
            : this("")
        {
        }

        public ExcelConnectionStringBuilder(string connectionString)
        {
            this._comparer = StringComparer.OrdinalIgnoreCase;

            this.Clear();
            if (!string.IsNullOrEmpty(connectionString))
            {
                base.ConnectionString = connectionString;
            }
        }

        public override void Clear()
        {
            this._dataSource = "";
            this._header = true;
            this._typeDetectionScheme = ExcelTypeDetectionScheme.ColumnFormat;
            this._rowScanDepth = 15;
        }

        public override bool Remove(string keyword)
        {
            if ((this._comparer.Compare(keyword, "Data Source") == 0) && base.Remove(keyword))
            {
                this._dataSource = "";
                return true;
            }

            if ((this._comparer.Compare(keyword, "Header") == 0) && base.Remove(keyword))
            {
                this._header = false;
                return true;
            }

            return false;
        }

        public override bool TryGetValue(string keyword, out object value)
        {
            keyword = Regex.Replace(keyword, @"\s+", string.Empty);
            value = null;

            if (this._comparer.Compare(keyword, "datasource") == 0)
            {
                value = this.DataSource;
                return true;
            }

            if (this._comparer.Compare(keyword, "header") == 0)
            {
                value = this.Header;
                return true;
            }

            if (this._comparer.Compare(keyword, "rowscandepth") == 0)
            {
                value = this.RowScanDepth;
                return true;
            }

            if (this._comparer.Compare(keyword, "typedetectionscheme") == 0)
            {
                value = this.TypeDetectionScheme;
                return true;
            }

            return false;
        }

        private bool TrySetValue(string keyword, object value)
        {
            keyword = Regex.Replace(keyword, @"\s+", string.Empty);

            if (this._comparer.Compare(keyword, "datasource") == 0)
            {
                this.DataSource = Convert.ToString(value);
                return true;
            }

            if (this._comparer.Compare(keyword, "header") == 0)
            {
                this.Header = Convert.ToBoolean(value);
                return true;
            }

            if (this._comparer.Compare(keyword, "rowscandepth") == 0)
            {
                this.RowScanDepth = Convert.ToInt32(value);
                return true;
            }

            if (this._comparer.Compare(keyword, "typedetectionscheme") == 0)
            {
                this.TypeDetectionScheme = (ExcelTypeDetectionScheme)Enum.Parse(typeof(ExcelTypeDetectionScheme), value.ToString(), true);
                return true;
            }

            return false;
        } 

        private void SetValue(string key, object value)
        {
            base[key] = Convert.ToString(value);
        }

        private Exception InvalidKeyword(string keyword)
        {
            return new ArgumentException(String.Format("Keyword not supported: '{0}'", new object[] { keyword }));
        }

        [DefaultValue("")]
        public string DataSource
        {
            get
            {
                return this._dataSource;
            }
            set
            {
                this.SetValue("Data Source", value);
                this._dataSource = value;
            }
        }

        [DefaultValue(true)]
        public bool Header
        {
            get
            {
                return this._header;
            }
            set
            {
                this.SetValue("Header", value);
                this._header = value;
            }
        }

        [DefaultValue(15)]
        public int RowScanDepth
        {
            get
            {
                return this._rowScanDepth;
            }
            set
            {
                this._rowScanDepth = value;
                this.SetValue("Row Scan Depth", value);
            }
        }

        [DefaultValue(ExcelTypeDetectionScheme.ColumnFormat)]
        public ExcelTypeDetectionScheme TypeDetectionScheme
        {
            get
            {
                return this._typeDetectionScheme;
                
            }
            set
            {
                this._typeDetectionScheme = value;
                this.SetValue("Type Detection Scheme", value);
            }
        }

        public override object this[string keyword]
        {
            get
            {
                object obj2 = null;
                if (!this.TryGetValue(keyword, out obj2))
                {
                    throw this.InvalidKeyword(keyword);
                }
                return obj2;
            }
            set
            {
                if (!this.TrySetValue(keyword, value))
                {
                    throw this.InvalidKeyword(keyword);
                }
            }
        }
      
       
    }
}
