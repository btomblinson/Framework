using System.Collections.Generic;
using System.Data;
using Framework.DataAccess.SqlServerDataClasses;
using MySql.Data.MySqlClient;

namespace Framework.DataAccess.MySqlDataClasses
{
    public class MySqlDataAccessContainer
    {
        private List<MySqlDataAccessParameter> _ColParameters;

        /// <summary>
        ///     Contructor that initiates the SqlDataAccessParameter List
        /// </summary>
        public MySqlDataAccessContainer()
        {
            _ColParameters = new List<MySqlDataAccessParameter>();
        }

        /// <summary>
        ///     Removes all parameters
        /// </summary>
        public void Clear()
        {
            _ColParameters.Clear();
        }

        /// <summary>
        ///     Adds SqlDataAccessParameter to SqlDataAccessParameter List
        /// </summary>
        /// <param name="dtoParam">SqlDataAccessParameter to add</param>
        public void Add(MySqlDataAccessParameter dtoParam)
        {
            _ColParameters.Add(dtoParam);
        }

        /// <summary>
        ///     Add parameters into a SqlDataAccessParameter object and add that to the list.  Size and Precision are assigned a 0
        ///     value.
        /// </summary>
        /// <param name="name">Gets added to Name property</param>
        /// <param name="dbType">Gets added to DataType property</param>
        /// <param name="value">Gets added to Value property</param>
        public void Add(string name, MySqlDbType dbType, string value)
        {
            MySqlDataAccessParameter dto = new MySqlDataAccessParameter();
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
        public void Add(string name, MySqlDbType dbType, object oValue)
        {
            MySqlDataAccessParameter dto = new MySqlDataAccessParameter();
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
        public void Add(string name, MySqlDbType dbType, int size, string value)
        {
            MySqlDataAccessParameter dto = new MySqlDataAccessParameter();
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
        public void Add(string name, MySqlDbType dbType, int size, object oValue)
        {
            MySqlDataAccessParameter dto = new MySqlDataAccessParameter();
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
        public void Add(string name, MySqlDbType dbType, int size, byte precision, string value)
        {
            MySqlDataAccessParameter dto = new MySqlDataAccessParameter();
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
        public void Add(string name, MySqlDbType dbType, int size, byte precision, object oValue)
        {
            MySqlDataAccessParameter dto = new MySqlDataAccessParameter();
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
        public List<MySqlDataAccessParameter> AllParameters()
        {
            return _ColParameters;
        }
    }
}