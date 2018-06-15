using System;
using System.Collections.Generic;
using System.Data;

namespace Framework.DataAccess.ObjectMapping
{
    public class DataTableWrapper : IDataWrapper
    {
        private readonly Dictionary<string, int> _columnMap;
        private readonly DataTable _dt;

        public DataTableWrapper(DataTable dt)
        {
            _dt = dt;
            _columnMap = new Dictionary<string, int>();
            int i = 0;
            foreach (DataColumn column in dt.Columns)
            {
                _columnMap.Add(column.ColumnName.ToUpper(), i++);
            }
        }

        public Dictionary<string, int> GetColumnMap()
        {
            return _columnMap;
        }

        public bool IsFieldNullByIndex(int row, int column)
        {
            return _dt.Rows[row].IsNull(column);
        }

        public object GetValueByIndex(int row, int column)
        {
            return _dt.Rows[row][column];
        }

        public DateTime? GetDateByFieldName(int row, int column)
        {
            return _dt.Rows[row][column] as DateTime? ?? null;
        }
    }
}