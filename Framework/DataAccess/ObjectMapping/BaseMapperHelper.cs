using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.DataAccess.ObjectMapping
{
    internal class BaseMapperHelper
    {
        public static Dictionary<string, PropertyInfoPlus> BuildPropertyMap(Type t)
        {
            var propMap = new Dictionary<string, PropertyInfoPlus>();
            var pia = t.GetProperties();
            foreach (PropertyInfo pi in pia)
            {
                var customAttrs =
                    (DataBinding[]) pi.GetCustomAttributes(typeof(DataBinding), true);
                if (customAttrs.Length > 0)
                    propMap.Add(customAttrs[0].FieldName.ToUpper(), new PropertyInfoPlus
                    {
                        propertyInfo = pi,
                        dataBinding = customAttrs[0]
                    });
            }

            return propMap;
        }
    }
}