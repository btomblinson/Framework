using System.Collections.Generic;
using System.Data;

namespace Framework.DataAccess.SqlDataClasses
{
    public class SqlDataAccessContainer
    {
        private List<SqlDataAccessParameter> _colParameters;

        /// <summary>
        ///     Contructor that initiates the SqlDataAccessParameter List
        /// </summary>
        public SqlDataAccessContainer()
        {
            _colParameters = new List<SqlDataAccessParameter>();
        }

        /// <summary>
        ///     Removes all parameters
        /// </summary>
        public void Clear()
        {
            _colParameters.Clear();
        }

        /// <summary>
        ///     Adds SqlDataAccessParameter to SqlDataAccessParameter List
        /// </summary>
        /// <param name="dtoParam">SqlDataAccessParameter to add</param>
        public void Add(SqlDataAccessParameter dtoParam)
        {
            _colParameters.Add(dtoParam);
        }

        /// <summary>
        ///     Add parameters into a SqlDataAccessParameter object and add that to the list.  Size and Precision are assigned a 0
        ///     value.
        /// </summary>
        /// <param name="name">Gets added to Name property</param>
        /// <param name="dbType">Gets added to DataType property</param>
        /// <param name="value">Gets added to Value property</param>
        public void Add(string name, SqlDbType dbType, string value)
        {
            SqlDataAccessParameter dto = new SqlDataAccessParameter();
            dto.Name = name;
            dto.DataType = dbType;
            dto.Size = 0;
            dto.Precision = 0;
            dto.Value = value;

            Add(dto);
        }

        /// <summary>
        ///     Add parameters into a SqlDataAccessParameter object and add that to the list.  Size and Precision are assigned a 0
        ///     value.
        /// </summary>
        /// <param name="name">Gets added to Name property</param>
        /// <param name="dbType">Gets added to DataType property</param>
        /// <param name="oValue">Gets added to Value property</param>
        public void Add(string name, SqlDbType dbType, object oValue)
        {
            SqlDataAccessParameter dto = new SqlDataAccessParameter();
            dto.Name = name;
            dto.DataType = dbType;
            dto.Size = 0;
            dto.Precision = 0;
            dto.ObjectValue = oValue;

            Add(dto);
        }

        /// <summary>
        ///     Add parameters into a SqlDataAccessParameter object and add that to the list.  Precision is assigned a 0 value.
        /// </summary>
        /// <param name="name">Gets added to Name property</param>
        /// <param name="dbType">Gets added to DataType property</param>
        /// <param name="size">Gets added to Size property</param>
        /// <param name="value">Gets added to Value property</param>
        public void Add(string name, SqlDbType dbType, int size, string value)
        {
            SqlDataAccessParameter dto = new SqlDataAccessParameter();
            dto.Name = name;
            dto.DataType = dbType;
            dto.Size = size;
            dto.Precision = 0;
            dto.Value = value;

            Add(dto);
        }

        /// <summary>
        ///     Add parameters into a SqlDataAccessParameter object and add that to the list.  Precision is assigned a 0 value.
        /// </summary>
        /// <param name="name">Gets added to Name property</param>
        /// <param name="dbType">Gets added to DataType property</param>
        /// <param name="size">Gets added to Size property</param>
        /// <param name="_value">Gets added to Value property</param>
        public void Add(string name, SqlDbType dbType, int size, object oValue)
        {
            SqlDataAccessParameter dto = new SqlDataAccessParameter();
            dto.Name = name;
            dto.DataType = dbType;
            dto.Size = size;
            dto.Precision = 0;
            dto.ObjectValue = oValue;

            Add(dto);
        }

        /// <summary>
        ///     Add parameters into a SqlDataAccessParameter object and add that to the list.
        /// </summary>
        /// <param name="name">Gets added to Name property</param>
        /// <param name="dbType">Gets added to DataType property</param>
        /// <param name="size">Gets added to Size property</param>
        /// <param name="precision">Gets added to Precision property</param>
        /// <param name="value">Gets added to Value property</param>
        public void Add(string name, SqlDbType dbType, int size, byte precision, string value)
        {
            SqlDataAccessParameter dto = new SqlDataAccessParameter();
            dto.Name = name;
            dto.DataType = dbType;
            dto.Size = size;
            dto.Precision = precision;
            dto.Value = value;

            Add(dto);
        }

        /// <summary>
        ///     Add parameters into a SqlDataAccessParameter object and add that to the list.
        /// </summary>
        /// <param name="name">Gets added to Name property</param>
        /// <param name="dbType">Gets added to DataType property</param>
        /// <param name="size">Gets added to Size property</param>
        /// <param name="precision">Gets added to Precision property</param>
        /// <param name="_value">Gets added to Value property</param>
        public void Add(string name, SqlDbType dbType, int size, byte precision, object oValue)
        {
            SqlDataAccessParameter dto = new SqlDataAccessParameter();
            dto.Name = name;
            dto.DataType = dbType;
            dto.Size = size;
            dto.Precision = precision;
            dto.ObjectValue = oValue;

            Add(dto);
        }

        /// <summary>
        ///     Returns the Parameter List
        /// </summary>
        /// <returns>Parameter List</returns>
        public List<SqlDataAccessParameter> AllParameters()
        {
            return _colParameters;
        }
    }
}