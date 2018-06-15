using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Framework.Commons.Data
{
    /// <summary>
    ///     Contains functions to retrieve data from datarow
    /// </summary>
    public class GetData
    {
        /// <summary>
        ///     Get the string value of the of the column in the row
        /// </summary>
        /// <param name="row">DataRow that contains data</param>
        /// <param name="columnName">Cells name from which the value is retrieved</param>
        /// <returns>String value of cell. If datarow is null or value in cell is null, String.Empty is returned.</returns>
        public static string GetString(DataRow row, string columnName)
        {
            if (row == null)
            {
                return string.Empty;
            }

            try
            {
                return row[columnName] != null ? row[columnName].ToString() : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Get the string value of the of the column in the row
        /// </summary>
        /// <param name="row">DataRow that contains data</param>
        /// <param name="columnNumber">Cell's index from which the value is retrieved</param>
        /// <returns>String value of cell. If datarow is null or value in cell is null, String.Empty is returned.</returns>
        public static string GetString(DataRow row, int columnNumber)
        {
            if (row == null)
            {
                return string.Empty;
            }

            try
            {
                return row[columnNumber] != null ? row[columnNumber].ToString() : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Get the integer value of the of the column in the row
        /// </summary>
        /// <param name="row">DataRow that contains data</param>
        /// <param name="columnName">Cell's name from which the value is retrieved</param>
        /// <returns>Integer value of cell. If datarow is null or value in cell is null, 0 is returned.</returns>
        public static int GetInt(DataRow row, string columnName)
        {
            if (row == null)
            {
                return 0;
            }

            try
            {
                return row[columnName] != null ? Convert.ToInt32(row[columnName]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///     Get the integer value of the of the column in the row
        /// </summary>
        /// <param name="row">DataRow that contains data</param>
        /// <param name="columnNumber">Cell's index from which the value is retrieved</param>
        /// <returns>Boolean value of cell. If datarow is null or value in cell is null, false is returned.</returns>
        public static int GetInt(DataRow row, int columnNumber)
        {
            if (row == null)
            {
                return 0;
            }

            try
            {
                return row[columnNumber] != null ? Convert.ToInt32(row[columnNumber]) : 0;
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        ///     Get the boolean value of the of the column in the row
        /// </summary>
        /// <param name="row">DataRow that contains data</param>
        /// <param name="columnName">Cell's name from which the value is retrieved</param>
        /// <returns>Boolean value of cell. If datarow is null or value in cell is null, false is returned.</returns>
        public static bool GetBoolean(DataRow row, string columnName)
        {
            if (row == null)
            {
                return false;
            }

            try
            {
                return row[columnName] != null && Convert.ToBoolean(row[columnName]);
            }
            catch
            {
                return false;
            }
        }

        public static bool GetBoolean(DataRow row, int columnNumber)
        {
            if (row == null)
            {
                return false;
            }

            try
            {
                return row[columnNumber] != null && Convert.ToBoolean(row[columnNumber]);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Maps the data row
        /// </summary>
        /// <param name="row">Row to map</param>
        /// <returns>Dictionary of the row with the value and the index the value came from</returns>
        public static Dictionary<string, int> GetRowMapper(DataRow row)
        {
            var rowMapper = new Dictionary<string, int>();
            for (int i = 0; i < row.ItemArray.Count(); i++)
            {
                rowMapper.Add(GetString(row, i), i);
            }

            return rowMapper;
        }
    }
}