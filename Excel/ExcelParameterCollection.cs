using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;

namespace GrapeCity.Enterprise.Data.DataSource.Excel
{
    public sealed class ExcelParameterCollection : DbParameterCollection
    {
        private InnerCollection _parameters;

        public ExcelParameterCollection()
        {
            this._parameters = new InnerCollection();
        }

        public override int Add(object excelParameter)
        {
            ExcelParameter item = (ExcelParameter)excelParameter;
            this._parameters.Add(item);

            return this.Count - 1;
        }

        public override void AddRange(Array values)
        {
            foreach (var parameter in values)
            {
                this.Add(parameter);
            }
        }

        public ExcelParameter AddWithValue(string name, object value)
        {
            ExcelParameter parameter = new ExcelParameter(name, value);
            this.Add(parameter);
            return parameter;
        }

        public override void Clear()
        {
            this._parameters.Clear();
        }

        public override bool Contains(object value)
        {
            return this._parameters.Contains((ExcelParameter)value);
        }

        public override bool Contains(string key)
        {
            return this._parameters.Contains(key);
        }

        public override void CopyTo(Array array, int index)
        {
            for (int i = 0; (i < this.Count) && (i < (array.Length - index)); i++)
            {
                array.SetValue(this[i], (int)(i + index));
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return this._parameters.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            return this._parameters.IndexOf((ExcelParameter)value);
        }

        public override int IndexOf(string parameterName)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this._parameters[i].ParameterName == parameterName)
                {
                    return i;
                }
            }
            return -1;
        }

        public override void Insert(int index, object value)
        {
            this._parameters.Insert(index, (ExcelParameter)value);
        }

        public override void Remove(object value)
        {
            this._parameters.Remove((ExcelParameter)value);
        }

        public override void RemoveAt(int index)
        {
            this._parameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            this._parameters.RemoveAt(this.IndexOf(parameterName));
        }

        protected override DbParameter GetParameter(int index)
        {
            return this._parameters[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return this._parameters[parameterName];
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            this._parameters[index] = (ExcelParameter)value;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            int index = this.IndexOf(parameterName);
            if (index >= 0)
            {
                this._parameters.RemoveAt(index);
            }
            this.Insert(index, value);
        }

        public new ExcelParameter this[string name]
        {
            get
            {
                return (ExcelParameter)this.GetParameter(name);
            }
            set
            {
                this.SetParameter(name, value);

            }
        }

        public new ExcelParameter this[int index]
        {
            get
            {
                return (ExcelParameter)this.GetParameter(index);
            }
            set
            {
                this.SetParameter(index, value);
            }
        }


        public override int Count
        {
            get
            {
                return this._parameters.Count;
            }
        }

        public override object SyncRoot
        {
            get
            {
                return null;
            }
        }

        public override bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override bool IsSynchronized
        {
            get
            {
                return false;
            }
        }


        private class InnerCollection : KeyedCollection<string, ExcelParameter>
        {
            protected override string GetKeyForItem(ExcelParameter item)
            {
                return item.ParameterName;
            }
        }
    }
}
