using System.Collections.Generic;
using System.Data;

namespace Framework.DataAccess.ObjectMapping
{
    public class MappingService
    {
        public List<T> MapDataTable<T>(DataTable dt) where T : new()
        {
            var results = new List<T>();
            var mapper = new BaseMapper<T>();
            IDataWrapper dtw = new DataTableWrapper(dt);
            for (int rowNum = 0; rowNum < dt.Rows.Count; rowNum++)
            {
                T thisT = new T();
                mapper.MapRow(dtw, rowNum, thisT);
                results.Add(thisT);
            }

            return results;
        }

        public T MapFirstDataRow<T>(DataTable dt) where T : new()
        {
            T result = new T();
            var mapper = new BaseMapper<T>();

            IDataWrapper dtw = new DataTableWrapper(dt);

            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                mapper.MapRow(dtw, 0, result);
            }

            return result;
        }
    }
}