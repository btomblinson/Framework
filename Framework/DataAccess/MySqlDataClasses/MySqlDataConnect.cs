using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Framework.DataAccess.SqlServerDataClasses;
using MySql.Data.MySqlClient;

namespace Framework.DataAccess.MySqlDataClasses
{
    /// <summary>
    ///     Class that allows selecting, inserting, updating, deleting records from SQL Server database
    ///     via stored procedures and allow selecting and inserting records from SQL server database via
    ///     inline SQL.  Database transactions can be initied.
    /// </summary>
    public class MySqlDataConnect
    {
        private int _CommandTimeOut = 30;

        private MySqlDataAdapter _Adprdata;

        private bool _BolTran;

        private MySqlCommand _CmdSql = new MySqlCommand();

        private MySqlConnection _ConDb;

        private string _LogMsg = "";

        private MySqlTransaction _OTran;

        /// <summary>
        /// You must call <see cref="ChangeConnectionString"/> to set the connection
        /// </summary>
        public MySqlDataConnect()
        {
            _ConDb = new MySqlConnection();
            _BolTran = false;
        }

        /// <summary>
        ///     Set the command timeout.  The default is 30 seconds.
        /// </summary>
        public int SetCommandTimeOut
        {
            set => _CommandTimeOut = value < 1 ? 30 : value;
        }

        /// <summary>
        ///     Add all of the parameters to command
        /// </summary>
        /// <param name="colAllParameters">List of SqlDataAccessParameter to command</param>
        private void AddParameterValues(List<MySqlDataAccessParameter> colAllParameters)
        {
            foreach (MySqlDataAccessParameter dtoParam in colAllParameters)
            {
                if (dtoParam.ObjectValue == DBNull.Value)
                {
                    _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                    _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                }
                else
                {
                    switch (dtoParam.DataType.ToString())
                    {
                        case "VarChar":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.Value);
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, "");
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "Decimal":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,Convert.ToDecimal(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, Convert.ToDecimal(dtoParam.Value));
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "TinyInt":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,Convert.ToByte(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, Convert.ToByte(dtoParam.Value));
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "SmallInt":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,Convert.ToInt16(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, Convert.ToInt16(dtoParam.Value));
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "Int":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,Convert.ToInt32(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, Convert.ToInt32(dtoParam.Value));
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "DateTime":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,Convert.ToDateTime(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, Convert.ToDateTime(dtoParam.Value));
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "Bit":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,Convert.ToByte(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, Convert.ToByte(dtoParam.Value));
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "VarBinary":
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,Convert.ToByte(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.Value);
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, (byte[]) dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;
                        case "Structured":
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.Value);
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            // Set TypeName, this is used for table parameters
                            //cmdSql.Parameters[dtoParam.Name].TypeName = dtoParam.TypeName;

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;
                        default:
                            //cmdSql.Parameters.AddWithValue(dtoParam.Name,dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.Value);
                            }
                            else if (dtoParam.ObjectValue != null)
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, dtoParam.ObjectValue);
                            }
                            else
                            {
                                _CmdSql.Parameters.AddWithValue(dtoParam.Name, DBNull.Value);
                            }

                            _CmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;
                    }
                }
            }
        }

        #region "Update/Add data"

        /// <summary>
        ///     Inserts, Updates, or deletes data via store procedure
        /// </summary>
        /// <param name="sProcedureName">Stored procedure's name to be ran</param>
        /// <param name="sError">String value that contains an error message if error occurs</param>
        /// <param name="dtoContainer">SqlDataAccessContainer with all parameters that will be added to the stored procedure</param>
        /// <returns>A <c>long</c> with the data from <c>@RETURN_VALUE</c> parameter in stored procedure</returns>
        public long UpdateData(string sProcedureName, ref string sError, MySqlDataAccessContainer dtoContainer)
        {
            try
            {
                //add the parameters if they exist
                if (dtoContainer != null)
                {
                    AddParameterValues(dtoContainer.AllParameters());
                }

                //add the return value parameter
                _CmdSql.Parameters.Add("@RETURN_VALUE", MySqlDbType.Int32);
                _CmdSql.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                //check for connection strings here
                if (OpenConnection())
                {
                    _CmdSql.Connection = _ConDb;
                    _CmdSql.CommandType = CommandType.StoredProcedure;
                    _CmdSql.CommandText = sProcedureName;
                    _CmdSql.CommandTimeout = _CommandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + _LogMsg;
                    return 0;
                }

                //check for transactions
                if (_BolTran)
                {
                    _CmdSql.Transaction = _OTran;
                }

                _CmdSql.ExecuteNonQuery();
                long nReturn = long.Parse(_CmdSql.Parameters["@RETURN_VALUE"].Value.ToString());

                _CmdSql.Connection = null;
                _CmdSql.Parameters.Clear();

                return nReturn;
            }

            catch (Exception e)
            {
                if (_BolTran)
                {
                    RollBackTransaction();
                }

                sError = e.Message;
                _CmdSql.Parameters.Clear();
                return 0;
            }
            finally
            {
                CloseConnection();
                if (_CmdSql != null && _BolTran == false)
                {
                    _CmdSql.Dispose();
                }
            }
        }

        #endregion

        /// <summary>
        ///     Insert a record via inline SQL
        /// </summary>
        /// <param name="sError">String value that contains an error message if error occurs</param>
        /// <param name="ssql">Inline insert statement</param>
        /// <returns>A <c>long</c> with the data returned from <c>scope_identity()</c> in database</returns>
        public long InsertRawSqlData(ref string sError, string ssql)
        {
            try
            {
                //check for connection strings here
                if (OpenConnection())
                {
                    //append the scope_identity her
                    ssql = ssql + "; select scope_identity()";
                    _CmdSql.Connection = _ConDb;
                    _CmdSql.CommandText = ssql;
                    _CmdSql.CommandType = CommandType.Text;
                    _CmdSql.CommandTimeout = _CommandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in MySqlDataConnect. " + _LogMsg;
                    return 0;
                }

                //check for transactions
                if (_BolTran)
                {
                    _CmdSql.Transaction = _OTran;
                }

                object nReturn = _CmdSql.ExecuteScalar();

                _CmdSql.Connection = null;
                _CmdSql.Parameters.Clear();

                if (nReturn == null)
                {
                    return 0;
                }

                if (nReturn.ToString() == string.Empty)
                {
                    return 0;
                }

                return int.Parse(nReturn.ToString());
            }

            catch (Exception e)
            {
                if (_BolTran)
                {
                    RollBackTransaction();
                }

                sError = e.Message;
                _CmdSql.Parameters.Clear();
                return -1;
            }
            finally
            {
                CloseConnection();
                if (_CmdSql != null && _BolTran == false)
                {
                    _CmdSql.Dispose();
                }
            }
        }

        /// <summary>
        ///     Modify data in database with raw sql statement
        /// </summary>
        /// <param name="sError">The error that occurred while executing the function</param>
        /// <param name="ssql">The raw sql to run</param>
        /// <returns>The number of records modified or -1 if error occurred</returns>
        public long ModifyRawSqlData(ref string sError, string ssql)
        {
            try
            {
                //check for connection strings here
                if (OpenConnection())
                {
                    _CmdSql.Connection = _ConDb;
                    _CmdSql.CommandText = ssql;
                    _CmdSql.CommandType = CommandType.Text;
                    _CmdSql.CommandTimeout = _CommandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in MySqlDataConnect. " + _LogMsg;
                    return 0;
                }

                //check for transactions
                if (_BolTran)
                {
                    _CmdSql.Transaction = _OTran;
                }

                int nReturn = _CmdSql.ExecuteNonQuery();

                return nReturn;
            }

            catch (Exception e)
            {
                if (_BolTran)
                {
                    RollBackTransaction();
                }

                sError = e.Message;
                return -1;
            }
            finally
            {
                CloseConnection();
                if (_CmdSql != null && _BolTran == false)
                {
                    _CmdSql.Dispose();
                }
            }
        }

        /// <summary>
        ///     Modify data in database with raw sql statement
        /// </summary>
        /// <param name="sError">The error that occurred while executing the function</param>
        /// <param name="ssql">The raw sql to run</param>
        /// <param name="numberRecordsUpdated">The desired number of records that are modified.</param>
        /// <returns>The number of records modified or -1 if error occurred</returns>
        public long ModifyRawSqlDataWithNumRecordCheck(ref string sError, string ssql, int numberRecordsUpdated)
        {
            try
            {
                if (numberRecordsUpdated < 1)
                {
                    throw new Exception("The number of records modified needs to be greater than 0.");
                }

                //check for connection strings here
                if (OpenConnection())
                {
                    _CmdSql.Connection = _ConDb;
                    _CmdSql.CommandText = ssql;
                    _CmdSql.CommandType = CommandType.Text;
                    _CmdSql.CommandTimeout = _CommandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + _LogMsg;
                    return 0;
                }

                StartTransaction();

                //check for transactions
                if (_BolTran)
                {
                    _CmdSql.Transaction = _OTran;
                }

                //append the scope_identity her
                int nReturn = _CmdSql.ExecuteNonQuery();
                if (nReturn != numberRecordsUpdated)
                {
                    throw new Exception("The number of records to be updated is: " + nReturn +
                                        ", but the desired number of records to modify was: " + numberRecordsUpdated +
                                        ".  Transaction is rolled back.");
                }

                EndTransaction();

                return nReturn;
            }

            catch (Exception e)
            {
                if (_BolTran)
                {
                    RollBackTransaction();
                }

                sError = e.Message;
                return -1;
            }
            finally
            {
                CloseConnection();
                if (_CmdSql != null && _BolTran == false)
                {
                    _CmdSql.Dispose();
                }
            }
        }

        #region "Transaction and open connections"

        /// <summary>
        ///     Change the connection string used
        /// </summary>
        /// <param name="conn">Connection string value</param>
        /// <returns>true if connection is set successfully, false if any errors occurred</returns>
        public bool ChangeConnectionString(string conn)
        {
            try
            {
                if (_ConDb != null)
                {
                    _ConDb = null;
                }

                _ConDb = new MySqlConnection(conn);
                return true;
            }
            catch (Exception ex)
            {
                _LogMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        ///     Open connection to database
        /// </summary>
        /// <returns>True if connection is open, false if error occurred</returns>
        public bool OpenConnection()
        {
            if (_ConDb != null)
            {
                try
                {
                    if (_ConDb.State != ConnectionState.Open)
                    {
                        _ConDb.Open();
                        return true;
                    }

                    if (_ConDb.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    _LogMsg = e.Message;
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///     Closes the connection
        /// </summary>
        /// <returns>True if connection is closed, false if error occurred</returns>
        public bool CloseConnection()
        {
            if (_ConDb != null && _BolTran == false)
            {
                try
                {
                    if (_ConDb.State != ConnectionState.Closed)
                    {
                        _ConDb.Close();
                        return true;
                    }

                    if (_ConDb.State == ConnectionState.Closed)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    _LogMsg = e.Message;
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///     Start a database transaction
        /// </summary>
        public void StartTransaction()
        {
            if (OpenConnection())
            {
                if (_OTran == null)
                {
                    _OTran = _ConDb.BeginTransaction();
                    _BolTran = true;
                }
            }
        }

        /// <summary>
        ///     Commit the transaction.  The closeConnection still needs to be called to close the connection.
        /// </summary>
        public void EndTransaction()
        {
            if (_OTran != null)
            {
                _OTran.Commit();
                _OTran.Dispose();
                _OTran = null;
                _BolTran = false;
            }
        }

        /// <summary>
        ///     Rollback the transaction.  The closeConnection still needs to be called to close the connection.
        /// </summary>
        public void RollBackTransaction()
        {
            if (_OTran != null)
            {
                _OTran.Rollback();
                _OTran.Dispose();
                _OTran = null;
                _BolTran = false;
            }
        }

        #endregion

        #region "getData"

        /// <summary>
        ///     Get the data via store procedure
        /// </summary>
        /// <param name="dsData">DataSet that will be filled with data if successful</param>
        /// <param name="sProcedureName">Stored procedure's name to be ran</param>
        /// <param name="sError">String value that contains an error message if error occurs</param>
        /// <param name="dtoContainer">SqlDataAccessContainer with all parameters that will be added to the stored procedure</param>
        /// <returns>true if stored procedure ran successfully, false if any errors occurred</returns>
        public bool GetData(ref DataSet dsData, string sProcedureName, ref string sError,
            MySqlDataAccessContainer dtoContainer)
        {
            try
            {
                //add the parameters if they exist
                if (dtoContainer != null)
                {
                    AddParameterValues(dtoContainer.AllParameters());
                }

                if (OpenConnection())
                {
                    _CmdSql.Connection = _ConDb;
                    _CmdSql.CommandType = CommandType.StoredProcedure;
                    _CmdSql.CommandText = sProcedureName;
                    _CmdSql.CommandTimeout = _CommandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + _LogMsg;
                    return false;
                }

                _Adprdata = new MySqlDataAdapter(_CmdSql);
                dsData = new DataSet();
                _Adprdata.Fill(dsData);
                _Adprdata.Dispose();
                _CmdSql.Parameters.Clear();
                return true;
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                if (_CmdSql != null)
                {
                    _CmdSql.Parameters.Clear();
                }

                return false;
            }
            finally
            {
                CloseConnection();
                if (_CmdSql != null && _BolTran == false)
                {
                    _CmdSql.Dispose();
                }
            }
        }

        /// <summary>
        ///     Get data via inline SQL
        /// </summary>
        /// <param name="dsData">DataSet that will be filled with data if successful</param>
        /// <param name="sSqlString">Inline SQL to be ran</param>
        /// <param name="sError">String value that contains an error message if error occurs</param>
        /// <returns>true if SQL ran successfully, false if any errors occurred</returns>
        public bool GetData(ref DataSet dsData, string sSqlString, ref string sError)
        {
            try
            {
                if (OpenConnection())
                {
                    _CmdSql = new MySqlCommand();
                    _CmdSql.Connection = _ConDb;
                    _CmdSql.CommandType = CommandType.Text;
                    _CmdSql.CommandText = sSqlString;
                    _CmdSql.CommandTimeout = _CommandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + _LogMsg;
                    return false;
                }

                _Adprdata = new MySqlDataAdapter(_CmdSql);
                dsData = new DataSet();
                _Adprdata.Fill(dsData);
                _Adprdata.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                if (_CmdSql != null)
                {
                    _CmdSql.Parameters.Clear();
                }

                return false;
            }
            finally
            {
                CloseConnection();
                if (_CmdSql != null && _BolTran == false)
                {
                    _CmdSql.Dispose();
                }
            }
        }

        #endregion
    }
}