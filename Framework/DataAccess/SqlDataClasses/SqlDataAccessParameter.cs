using System.Data;

namespace Framework.DataAccess.SqlDataClasses
{
    /// <summary>
    ///     Class to hold SQL stored producture parameter variable's information needed to run a stored procedure.
    /// </summary>
    public class SqlDataAccessParameter
    {
        /// <summary>
        ///     Constructor that sets Name and Value to emtpy strings and Size and Precision to 0.
        /// </summary>
        public SqlDataAccessParameter()
        {
            Name = "";
            Size = 0;
            Precision = 0;
            Value = "";
        }

        /// <summary>
        ///     Constructor that sets Name, DataType, and Value. Size and Precision is set to 0.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="dataType">SqlDbType DataType</param>
        /// <param name="value">Value</param>
        public SqlDataAccessParameter(string name, SqlDbType dataType, string value)
        {
            Name = name;
            DataType = dataType;
            Size = 0;
            Precision = 0;
            Value = value;
        }

        /// <summary>
        ///     Constructor that sets Name, DataType, and Value. Size and Precision is set to 0.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="dataType">SqlDbType DataType</param>
        /// <param name="value">Value</param>
        public SqlDataAccessParameter(string name, SqlDbType dataType, object value)
        {
            Name = name;
            DataType = dataType;
            Size = 0;
            Precision = 0;
            ObjectValue = value;
        }

        /// <summary>
        ///     Constructor that sets Name, DataType, TypeName, and Value. Size and Precision is set to 0.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="dataType">SqlDbType DataType</param>
        /// <param name="typeName">string TypeName</param>
        /// <param name="value">Value</param>
        public SqlDataAccessParameter(string name, SqlDbType dataType, string typeName, object value)
        {
            TypeName = typeName;
            Name = name;
            DataType = dataType;
            Size = 0;
            Precision = 0;
            ObjectValue = value;
        }

        /// <summary>
        ///     Constructor that sets Name, DataType, Size, and Value.  Precision is set to 0.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="dataType">SqlDbType DataType</param>
        /// <param name="size">Size</param>
        /// <param name="value">Value</param>
        public SqlDataAccessParameter(string name, SqlDbType dataType, int size, string value)
        {
            Name = name;
            DataType = dataType;
            Size = size;
            Precision = 0;
            Value = value;
        }

        /// <summary>
        ///     Constructor that sets Name, DataType, Size, and Value.  Precision is set to 0.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="dataType">SqlDbType DataType</param>
        /// <param name="size">Size</param>
        /// <param name="value">Value</param>
        public SqlDataAccessParameter(string name, SqlDbType dataType, int size, object value)
        {
            Name = name;
            DataType = dataType;
            Size = size;
            Precision = 0;
            ObjectValue = value;
        }

        /// <summary>
        ///     Constructor that sets Name, DataType, Size, Precision, and Value.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="dataType">SqlDbType DataType</param>
        /// <param name="size">Size</param>
        /// <param name="precision">Precision</param>
        /// <param name="value">Value</param>
        public SqlDataAccessParameter(string name, SqlDbType dataType, int size, byte precision, string value)
        {
            Name = name;
            DataType = dataType;
            Size = size;
            Precision = precision;
            Value = value;
        }

        /// <summary>
        ///     Constructor that sets Name, DataType, Size, Precision, and Value.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="dataType">SqlDbType DataType</param>
        /// <param name="size">Size</param>
        /// <param name="precision">Precision</param>
        /// <param name="value">Value</param>
        public SqlDataAccessParameter(string name, SqlDbType dataType, int size, byte precision, object value)
        {
            Name = name;
            DataType = dataType;
            Size = size;
            Precision = precision;
            ObjectValue = value;
        }

        /// <summary>
        ///     SQL stored producture parameter variable's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     SQL stored producture parameter variable's data type
        /// </summary>
        public SqlDbType DataType { get; set; }

        /// <summary>
        ///     SQL stored producture parameter variable's size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     SQL stored producture parameter variable's precision
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        ///     SQL stored producture parameter variable's value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     SQL stored producture parameter variable's value
        /// </summary>
        public object ObjectValue { get; set; }

        public string TypeName { get; set; }
    }
}