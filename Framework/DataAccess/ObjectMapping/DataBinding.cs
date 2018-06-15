using System;

namespace Framework.DataAccess.ObjectMapping
{
    public class DataBinding : Attribute
    {
        public DataBinding()
        {
        }

        public DataBinding(string FieldNameParm)
        {
            FieldName = FieldNameParm;
        }

        public string FieldName { get; set; }
    }
}