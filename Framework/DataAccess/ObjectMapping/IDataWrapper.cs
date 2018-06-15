using System;
using System.Collections.Generic;

namespace Framework.DataAccess.ObjectMapping
{
    public interface IDataWrapper
    {
        /*
         * This is a map generated from the data reader
         * of field names to field indices in the data
         * reader object.
         */
        Dictionary<string, int> GetColumnMap();
        bool IsFieldNullByIndex(int row, int column);
        object GetValueByIndex(int row, int column);
        DateTime? GetDateByFieldName(int row, int column);
    }
}