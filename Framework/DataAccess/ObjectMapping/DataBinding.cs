using System;

namespace Framework.DataAccess.ObjectMapping
{
    public class DataBinding : Attribute
    {
        public DataBinding()
        {
        }

        public DataBinding(string fieldNameParm)
        {
            FieldName = fieldNameParm;
        }

        public string FieldName { get; set; }
    }
}