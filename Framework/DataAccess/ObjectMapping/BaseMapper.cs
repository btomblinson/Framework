using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Framework.DataAccess.ObjectMapping
{
    public class BaseMapper<T>
    {
        private Dictionary<string, PropertyInfoPlus> _namePropMap;

        public BaseMapper()
        {
            _namePropMap = BaseMapperHelper.BuildPropertyMap(typeof(T));
        }

        private static string HexStr(byte[] p)
        {
            //char[] c = new char[p.Length * 2 + 2];
            var c = new char[p.Length * 2];
            byte b;
            //c[0] = '0'; c[1] = 'x';
            //for (int y = 0, x = 2; y < p.Length; ++y, ++x)
            for (int y = 0, x = 0; y < p.Length; ++y, ++x)
            {
                b = (byte) (p[y] >> 4);
                c[x] = (char) (b > 9 ? b + 0x37 : b + 0x30);
                b = (byte) (p[y] & 0xF);
                c[++x] = (char) (b > 9 ? b + 0x37 : b + 0x30);
            }

            return new string(c);
        }

        public void MapRow(IDataWrapper fromRow, int rowNumber, T toObject)
        {
            var columnMap = fromRow.GetColumnMap();
            foreach (string databaseFieldName in columnMap.Keys)
            {
                if (_namePropMap.ContainsKey(databaseFieldName))
                {
                    int indexOfField = columnMap[databaseFieldName];
                    PropertyInfo pi = _namePropMap[databaseFieldName].propertyInfo;
                    object newObject = null;
                    if (!fromRow.IsFieldNullByIndex(rowNumber, indexOfField))
                    {
                        object objToConvert = fromRow.GetValueByIndex(rowNumber, indexOfField);
                        if (objToConvert is DateTime)
                        {
                            newObject = fromRow.GetDateByFieldName(rowNumber, indexOfField);
                        }
                        else if (objToConvert.GetType() == typeof(byte[]) && pi.PropertyType == typeof(string))
                        {
                            newObject = HexStr(objToConvert as byte[]);
                        }
                        else if (ValidateJson(objToConvert.ToString()))
                        {
                            try
                            {
                                newObject = JsonConvert.DeserializeObject(objToConvert.ToString(),
                                    _namePropMap[databaseFieldName].propertyInfo.PropertyType);
                            }
                            catch
                            {
                                newObject = objToConvert.ToString();
                            }
                        }
                        else if (pi.PropertyType.IsEnum)
                        {
                            if (objToConvert.GetType() == typeof(string))
                            {
                                newObject = Enum.Parse(pi.PropertyType, objToConvert.ToString());
                            }
                            else
                            {
                                newObject = Enum.ToObject(pi.PropertyType, Convert.ToInt32(objToConvert));
                            }
                        }
                        else
                        {
                            if (pi.PropertyType.IsGenericType
                                && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                if (objToConvert == null)
                                {
                                    newObject = null;
                                }
                                else
                                {
                                    Type underType = Nullable.GetUnderlyingType(pi.PropertyType);
                                    newObject = Convert.ChangeType(
                                        objToConvert,
                                        underType);
                                }
                            }
                            else
                            {
                                newObject = Convert.ChangeType(
                                    objToConvert,
                                    pi.PropertyType);
                            }
                        }
                    }

                    toObject.GetType().InvokeMember(
                        pi.Name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                        Type.DefaultBinder,
                        toObject,
                        new[] {newObject});
                }
            }
        }

        private static bool ValidateJson(string s)
        {
            s = s.Trim();
            if (s.StartsWith("{") && s.EndsWith("}") || //For object
                s.StartsWith("[") && s.EndsWith("]")) //For array
            {
                try
                {
                    JToken.Parse(s);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    return false;
                }
            }

            return false;
        }
    }
}