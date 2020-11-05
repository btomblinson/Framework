using System;
using System.Collections.Generic;
using System.Data;

namespace Framework.DataAccess.ObjectMapping
{
    public class DataTableWrapper : IDataWrapper
    {
        private readonly Dictionary<string, int> _ColumnMap;
        private readonly DataTable _Dt;

        public DataTableWrapper(DataTable dt)
        {
            _Dt = dt;
            _ColumnMap = new Dictionary<string, int>();
            int i = 0;
            foreach (DataColumn column in dt.Columns)
            {
                _ColumnMap.Add(column.ColumnName.ToUpper(), i++);
            }
        }

        public Dictionary<string, int> GetColumnMap()
        {
            return _ColumnMap;
        }

        public bool IsFieldNullByIndex(int row, int column)
        {
            return _Dt.Rows[row].IsNull(column);
        }

        public object GetValueByIndex(int row, int column)
        {
            return _Dt.Rows[row][column];
        }

        public DateTime? GetDateByFieldName(int row, int column)
        {
            return _Dt.Rows[row][column] as DateTime? ?? null;
        }
    }
}