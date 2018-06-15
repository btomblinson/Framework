using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Framework.Commons.CommonObj;
using Framework.DataAccess.ObjectMapping;
using Framework.Security;
using Newtonsoft.Json;

namespace Framework.Commons.Utilities
{
    /// <summary>
    ///     Group of extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        ///     Compare the values of two strings to see if they are the same not depending on case
        /// </summary>
        /// <param name="str1">First string to compare</param>
        /// <param name="str2">Second string to compare</param>
        /// <returns>boolean value stating is 2 strings are same value (case insensitive)</returns>
        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     Convert String to Title Case
        /// </summary>
        /// <param name="str1">String to Convert</param>
        /// <returns>String in Title Case format</returns>
        public static string ToTitleCase(this string str1)
        {
            if (!string.IsNullOrWhiteSpace(str1))
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                return myTI.ToTitleCase(str1.ToLower());
            }

            return "";
        }


        /// <summary>
        ///     Turn the rows into columns and columns into rows for a datatable.
        /// </summary>
        /// <param name="dt"><see cref="System.Data.DataTable" /> to transpose</param>
        /// <param name="removeFirstRow">Remove the first row in the transposed table</param>
        /// <returns>Transposed datatable</returns>
        public static DataTable TransposeDataTable(this DataTable dt, bool removeFirstRow)
        {
            DataTable transposedTable = new DataTable();

            //Add a column for each row in first data table
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataColumn dc = new DataColumn(dt.Rows[i][0].ToString());
                transposedTable.Columns.Add(dc);
            }

            for (int j = 1; j < dt.Columns.Count; j++)
            {
                DataRow dr = transposedTable.NewRow();

                for (int k = 0; k < dt.Rows.Count; k++) dr[k] = dt.Rows[k][j];

                transposedTable.Rows.Add(dr);
            }

            if (removeFirstRow) transposedTable.Rows[0].Delete();

            return transposedTable;
        }


        /// <summary>
        ///     Compare the values of two strings to see if they are the same not depending on case
        /// </summary>
        /// <param name="str1">First string to compare</param>
        /// <param name="str2">Second string to compare</param>
        /// <returns>boolean value stating is 2 strings are same value (case insensitive)</returns>
        public static bool ContainsIgnoreCase(this string str1, string str2)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(str1, str2, CompareOptions.IgnoreCase) >= 0;
        }

        /// <summary>
        ///     Add text to string.  If text being added is larger than maxLength, the text being
        ///     added will be substrung.  If text being added is shorter, then spaces will be padded to the right.
        /// </summary>
        /// <param name="str1">String to be added</param>
        /// <param name="str2">String to add</param>
        /// <param name="maxLength">Maximum length to be added to string</param>
        /// <returns>Formatted string with full or partial value of str2</returns>
        public static string AddFormattedField(this string str1, string str2, int maxLength)
        {
            if (!string.IsNullOrEmpty(str2))
            {
                if (str2.Length > maxLength)
                    str2 = str2.Substring(0, maxLength - 1);
                else if (str2.Length < maxLength) str2 += AddRepeatingChar(' ', str2.Length - maxLength);

                str1 += str2;
            }
            else
            {
                str1 += AddRepeatingChar(' ', maxLength);
            }

            return str1;
        }

        /// <summary>
        ///     Add a specified amount of a specified character to a string
        /// </summary>
        /// <param name="character">Character to add</param>
        /// <param name="length">Amount of characters to add in a row</param>
        /// <returns>String of characters that have been repeating a specified length</returns>
        public static string AddRepeatingChar(char character, int length)
        {
            string returnString = "";

            if (length > 0)
                for (int i = 0; i < length; i++)
                    returnString += character;

            return returnString;
        }

        /// <summary>
        ///     Get a specified number of characters off beginning of a string
        /// </summary>
        /// <param name="sValue">String to use</param>
        /// <param name="iMaxLength">Amount of characters get off the beginning of the string</param>
        /// <returns>String of characters from the beginning of the string</returns>
        public static string Left(this string sValue, int iMaxLength)
        {
            if (string.IsNullOrEmpty(sValue))
                sValue = string.Empty;
            else if (sValue.Length > iMaxLength) return sValue.Substring(0, iMaxLength);

            return sValue;
        }

        /// <summary>
        ///     Get a specified number of characters off end of a string
        /// </summary>
        /// <param name="sValue">String to use</param>
        /// <param name="iMaxLength">Amount of characters get off the end of the string</param>
        /// <returns>String of characters from the end of the string</returns>
        public static string Right(this string sValue, int iMaxLength)
        {
            //Check if the value is valid
            if (string.IsNullOrEmpty(sValue))
                sValue = string.Empty;
            else if (sValue.Length > iMaxLength) sValue = sValue.Substring(sValue.Length - iMaxLength);

            //Return the string
            return sValue;
        }

        /// <summary>
        ///     Check to see if object is null or the default of the object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="argument">Object to test if null or default</param>
        /// <returns>Is object null or the default</returns>
        public static bool IsNullOrEmpty<T>(this T argument)
        {
            // deal with normal scenarios
            if (argument == null) return true;

            if (Equals(argument, default(T))) return true;

            // deal with non-null nullables
            Type methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;

            // deal with boxed value types
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return argument.GetType().GetProperties()
                .Where(pi => pi.GetValue(argument) is string)
                .Select(pi => (string) pi.GetValue(argument))
                .All(value => string.IsNullOrEmpty(value));
        }

        /// <summary>
        ///     Check to see if IEnumerable is null or empty
        /// </summary>
        /// <typeparam name="T">Type of IEnumerable</typeparam>
        /// <param name="value">IEnumerable to check to see if null or default</param>
        /// <returns>Is IEnumerable null or empty</returns>
        public static bool IsListNullOrEmpty<T>(this IEnumerable<T> value)
        {
            return value == null || value.Count() == default(int);
        }

        /// <summary>
        ///     Get a short date of a nullable DateTime
        /// </summary>
        /// <param name="date">nullable DateTime</param>
        /// <returns>Blank for null DateTime or Short Date String</returns>
        public static string GetShortDateValue(this DateTime? date)
        {
            return date == null ? "" : date.Value.ToShortDateString();
        }

        /// <summary>
        ///     Outputs the value of the Nullable DateTime to the specified DateTime.ToString format
        /// </summary>
        /// <param name="date">Nullable DateTime to format</param>
        /// <param name="format">Format</param>
        /// <returns>Formatted date or empty string if passed in Nullable DateTime is null</returns>
        public static string ToString(this DateTime? date, string format)
        {
            return date == null ? "" : date.Value.ToString(format);
        }

        /// <summary>
        ///     Sets the maximum length the value can be
        /// </summary>
        /// <param name="str1">String to have value checked</param>
        /// <param name="maxLength">Maximum length of value of string</param>
        /// <returns>Beginning part of the string with maximum length being the value passed in</returns>
        public static string MaxLength(this string str1, int maxLength)
        {
            if (!string.IsNullOrWhiteSpace(str1) && str1.Length > maxLength) return str1.Substring(0, maxLength - 1);

            return str1;
        }

        /// <summary>
        ///     Determine if a value is not in an enumerable object
        /// </summary>
        /// <typeparam name="T">Type of enumerable object</typeparam>
        /// <param name="source">Enumerable object </param>
        /// <param name="predicate">What to filter on</param>
        /// <returns>Is item in enumerable object or not</returns>
        public static bool None<T>(this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }

        /// <summary>
        ///     Convert list to a DataSet Object
        /// </summary>
        /// <typeparam name="T">Type of List</typeparam>
        /// <param name="list">List to convert to a DataSet</param>
        /// <returns>DataSet with data in list</returns>
        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            if (elementType.Name.ToUpper() == "STRING")
            {
                t.Columns.Add("Device", typeof(string));
                for (int i = 0; i < list.Count; i++)
                {
                    DataRow row = t.NewRow();
                    row["Device"] = list[i].ToString();
                    t.Rows.Add(row);
                }
            }
            else
            {
                //add a column to table for each public property on T
                foreach (PropertyInfo propInfo in elementType.GetProperties())
                {
                    Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                    t.Columns.Add(propInfo.Name, ColType);
                }

                //go through each property on T and add each value to the table
                foreach (T item in list)
                {
                    DataRow row = t.NewRow();

                    foreach (PropertyInfo propInfo in elementType.GetProperties())
                        row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;

                    t.Rows.Add(row);
                }
            }

            return ds;
        }

        /// <summary>
        ///     Check to see if user is in any role
        /// </summary>
        /// <param name="user">Logged in User</param>
        /// <param name="roles">Roles to check to see if user is in role</param>
        /// <returns>Is user in at least in one of the roles</returns>
        public static bool IsInAnyRole(this IPrincipal user, List<string> roles)
        {
            foreach (string role in roles)
                if (user.IsInRole(role.Trim()))
                    return true;

            return false;
        }

        /// <summary>
        ///     Check to see if user is in any role
        /// </summary>
        /// <param name="user">Logged in User</param>
        /// <param name="roles">Comma separated values that contain roles to check to see if user is in role</param>
        /// <returns>Is user in at least in one of the roles</returns>
        public static bool IsInAnyRole(this IPrincipal user, string roles)
        {
            var combinedRoles = roles.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            return IsInAnyRole(user, combinedRoles.ToList());
        }

        /// <summary>
        ///     Convert Object to a DataSet Object
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="obj">Object to convert to a DataSet</param>
        /// <returns>DataSet with data in Object</returns>
        public static DataTable ToDataTable<T>(this T obj)
        {
            Type elementType = typeof(T);
            DataTable t = new DataTable();

            //add a column to table for each public property on T
            foreach (PropertyInfo propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table

            DataRow row = t.NewRow();

            foreach (PropertyInfo propInfo in elementType.GetProperties())
                row[propInfo.Name] = propInfo.GetValue(obj, null) ?? DBNull.Value;

            t.Rows.Add(row);

            return t;
        }

        /// <summary>
        ///     Convert list to a DataSet Object
        /// </summary>
        /// <typeparam name="T">Type of List</typeparam>
        /// <param name="list">List to convert to a DataSet</param>
        /// <returns>DataSet with data in list</returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataTable t = new DataTable();

            //add a column to table for each public property on T
            foreach (PropertyInfo propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (PropertyInfo propInfo in elementType.GetProperties())
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;

                t.Rows.Add(row);
            }

            return t;
        }

        /// <summary>Use to add an item to an IEnumerable object</summary>
        /// <typeparam name="T">Type to add</typeparam>
        /// <param name="e">Enumerable in which to add value</param>
        /// <param name="value">Value to add</param>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (T cur in e) yield return cur;

            yield return value;
        }


        /// <summary>
        ///     Converts datarow to model using the description attributes on the model properties as
        ///     the column name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T ToModel<T>(this DataRow row) where T : new()
        {
            DataTable table = row.Table.Clone();
            DataRow newRow = table.NewRow();
            newRow.ItemArray = row.ItemArray;
            table.Rows.Add(newRow);
            var list = new MappingService().MapDataTable<T>(table);
            if (list != null && list.Count > 0) return list[0];

            return new T();
        }


        /// <summary>
        ///     Converts DataTable to model collection using the description attributes on the model properties as
        ///     the column name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ToModelList<T>(this DataTable table) where T : new()
        {
            var list = new MappingService().MapDataTable<T>(table);
            return list;
        }


        /// <summary>
        ///     Capitalize the first character and add a space before each capitalized letter (except the first character).
        /// </summary>
        /// <param name="the_string"></param>
        /// <returns></returns>
        public static string ToProperCase(this string the_string)
        {
            if (string.IsNullOrEmpty(the_string)) return "";

            const string pattern = @"(?<=\w)(?=[A-Z])";
            string result = Regex.Replace(the_string, pattern,
                " ", RegexOptions.None);
            return result.Substring(0, 1).ToUpper() + result.Substring(1);
        }


        /// <summary>
        ///     Determines if one date instance is earlier than another --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool EarlierThan(this DateTime date1, DateTime date2)
        {
            return date1 < date2;
        }

        /// <summary>
        ///     Determines if one date instance is earlier than another --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool EarlierThan(this DateTime? date1, DateTime date2)
        {
            if (date1.IsMissing()) return false;

            DateTime d = date1 ?? DateTime.Now;
            return d.EarlierThan(date2);
        }

        /// <summary>
        ///     Determines if one date instance is earlier than another --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool EarlierThan(this DateTime? date1, DateTime? date2)
        {
            if (date1.IsMissing() || date2.IsMissing()) return false;

            DateTime d = date1 ?? DateTime.Now;
            DateTime d2 = date2 ?? DateTime.Now;
            return d.EarlierThan(d2);
        }


        /// <summary>
        ///     Determines if one date instance is later than another --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool LaterThan(this DateTime date1, DateTime date2)
        {
            return date1 > date2;
        }


        /// <summary>
        ///     Determines if one date instance is later than another --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool LaterThan(this DateTime? date1, DateTime date2)
        {
            if (date1.IsMissing()) return false;

            DateTime d = date1 ?? DateTime.Now;
            return d.LaterThan(date2);
        }


        /// <summary>
        ///     Determines if one date instance is later than another --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool LaterThan(this DateTime? date1, DateTime? date2)
        {
            if (date1.IsMissing() || date2.IsMissing()) return false;

            DateTime d = date1 ?? DateTime.Now;
            DateTime d2 = date2 ?? DateTime.Now;
            return d.LaterThan(d2);
        }


        /// <summary>
        ///     Determines if two date instances are the same --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool SameTimeAs(this DateTime date1, DateTime date2)
        {
            return date1 == date2;
        }


        /// <summary>
        ///     Determines if two date instances are the same --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool SameTimeAs(this DateTime? date1, DateTime date2)
        {
            if (date1.IsMissing()) return false;

            DateTime d = date1 ?? DateTime.Now;
            return d.SameTimeAs(date2);
        }


        /// <summary>
        ///     Determines if two date instances are the same --- by Temi
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool SameTimeAs(this DateTime? date1, DateTime? date2)
        {
            if (date1.IsMissing() || date2.IsMissing()) return false;

            DateTime d = date1 ?? DateTime.Now;
            DateTime d2 = date2 ?? DateTime.Now;
            return d.SameTimeAs(d2);
        }


        /// <summary>
        ///     Alias for is null, empty, or white space  -- by Temi
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMissing(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        ///     Alias for is null, empty, or white space  -- by Temi
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsMissing(this object obj)
        {
            return obj == null;
        }


        /// <summary>
        ///     Shortcut for determining if a IEnumerable or any of it's derived classes (List, Array, etc.) is null or less than
        ///     one -- by Temi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsMissingOrLessThanOne<T>(this IEnumerable<T> obj)
        {
            return obj == null || obj.Count() < 1;
        }

        /// <summary>
        ///     Alias for is not null, not empty and not white space -- by Temi
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotMissing(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }


        /// <summary>
        ///     Outputs any object as JSON
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string JsonOut(this object input)
        {
            return JsonConvert.SerializeObject(input);
        }

        /// <summary>
        ///     Safely gets a portion of a string
        /// </summary>
        /// <param name="input">Initial string</param>
        /// <param name="length">Length of characters to return</param>
        /// <param name="startIndex">Starting index</param>
        public static string SafeSubstring(this string input, int length, int startIndex = 0)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            if (input.Length >= startIndex + length) return input.Substring(startIndex, length);

            if (input.Length > startIndex) return input.Substring(startIndex);

            return string.Empty;
        }

        /// <summary>
        ///     Strips HTML tags
        /// </summary>
        /// <param name="input">input to strip</param>
        public static string StripHtmlTags(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        /// <summary>
        ///     Convert a bitmap object to byte array
        /// </summary>
        /// <param name="img">Bitmap object to convert to byte array</param>
        public static byte[] ToBytes(this Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        ///     Convert an object to byte array
        /// </summary>
        /// <param name="obj">object to convert to byte array</param>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null) return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Checks to see if value in an array
        /// </summary>
        /// <param name="array">Array to check</param>
        /// <param name="value">Value to check on</param>
        public static bool ContainsIgnoreCase(this string[] array, string value)
        {
            bool rtn = false;
            try
            {
                array = array.Select(a => a.ToLower()).ToArray();
                int pos = Array.IndexOf(array, value.ToLower());
                if (pos > -1)
                    rtn = true;
                else
                    rtn = false;
            }
            catch
            {
            }

            return rtn;
        }

        /// <summary>Validates an uploaded file for size and extension</summary>
        /// <param name="file">File to be validated</param>
        /// <returns>
        ///     <see cref="FileValidationResult" />
        /// </returns>
        public static FileValidationResult Validate(this HttpPostedFileBase file)
        {
            int maxSize = WebConfig.Get("max_upload_size", 4194304); // default is 4MB
            string acceptedFiles = WebConfig.Get("accepted_file_types", ".pdf,.jpg,.png,.doc,.docx");
            var ValidFileTypes = acceptedFiles.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            FileValidationResult result = new FileValidationResult();
            result.IsValid = false;
            if (file.IsMissing())
            {
                result.Message = "file is missing";
                return result;
            }

            if (file.ContentLength < 1)
            {
                result.Message = "empty file";
                return result;
            }

            if (file.ContentLength > maxSize)
            {
                result.Message = "file is too large";
                return result;
            }

            if (!HtmlValidator.CheckFileType(file.FileName, ValidFileTypes))
                if (file.ContentLength > maxSize)
                {
                    result.Message = "Invalid file type";
                    return result;
                }

            result.Message = "file is valid";
            result.IsValid = true;
            return result;
        }

        /// <summary>
        ///     Compare two objects
        /// </summary>
        /// <param name="obj1">The first object</param>
        /// <param name="obj2">The second object</param>
        /// <returns></returns>
        public static bool Compare(this object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null) return false;


            if (obj1.GetType() != obj2.GetType()) return false;

            Type type = obj1.GetType();
            if (type.IsPrimitive || typeof(string) == type) return obj1.Equals(obj2);

            if (type.IsArray)
            {
                Array first = obj1 as Array;
                Array second = obj2 as Array;
                IEnumerator en = first.GetEnumerator();
                int i = 0;
                while (en.MoveNext())
                {
                    if (!Equals(en.Current, second.GetValue(i))) return false;

                    i++;
                }
            }
            else
            {
                foreach (PropertyInfo pi in type.GetProperties(
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                {
                    object val = pi.GetValue(obj1);
                    object tval = pi.GetValue(obj2);
                    if (!Equals(val, tval)) return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Helper method to check if a string contains HTML tags
        /// </summary>
        /// <param name="input">The string to check</param>
        /// <returns>True if the string contained HTML, false if not</returns>
        public static bool ContainsHtml(this string input)
        {
            try
            {
                XElement x = XElement.Parse("<wrapper>" + input + "</wrapper>");
                return !(x.DescendantNodes().Count() == 1 && x.DescendantNodes().First().NodeType == XmlNodeType.Text);
            }
            catch (XmlException ex)
            {
                return true;
            }
        }
    }
}