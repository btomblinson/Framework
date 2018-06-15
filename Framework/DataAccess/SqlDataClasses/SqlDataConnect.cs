using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Framework.DataAccess.SqlDataClasses
{
    //create the public enumerators here
    public enum eSelect
    {
        GetColumns,
        GetNextCaseId,
        GetAzmanRoles,
        GetAzmanRoleHeirarchy2,
        GetNoOfParentRoles
    }

    public enum eUpdate
    {
        RoleHeirarchy,
        RoleToMemberDates,
        RoleHeirarchy2
    }

    /// <summary>
    ///     Class that allows selecting, inserting, updating, deleting records from SQL Server database
    ///     via stored procedures and allow selecting and inserting records from SQL server database via
    ///     inline SQL.  Database transactions can be initied.
    /// </summary>
    public class SqlDataConnect
    {
        private int _commandTimeOut = 30;
        private SqlDataAdapter Adprdata;
        private bool bolTran;
        private SqlCommand cmdSql = new SqlCommand();
        private SqlConnection conDB;

        private string logMsg = "";
        private SqlTransaction oTran;

        //new function here
        public SqlDataConnect()
        {
            //use this one      
            applySettings();
            bolTran = false;
        }

        /// <summary>
        ///     Set the command timeout.  The default is 30 seconds.
        /// </summary>
        public int SetCommandTimeOut
        {
            set => _commandTimeOut = value < 1 ? 30 : value;
        }

        /// <summary>
        ///     Add all of the parameters to command
        /// </summary>
        /// <param name="colAllParameters">List of SqlDataAccessParameter to command</param>
        private void AddParameterValues(List<SqlDataAccessParameter> colAllParameters)
        {
            foreach (SqlDataAccessParameter dtoParam in colAllParameters)
                if (dtoParam.ObjectValue == DBNull.Value)
                {
                    cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType, dtoParam.Size);
                    cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;
                    cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                }
                else
                {
                    switch (dtoParam.DataType.ToString())
                    {
                        case "VarChar":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType, dtoParam.Size);
                            //cmdSql.Parameters[dtoParam.Name].Value = dtoParam.Value;
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.Value;
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = "";

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "Decimal":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType, dtoParam.Size);
                            //cmdSql.Parameters[dtoParam.Name].Value = Convert.ToDecimal(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = Convert.ToDecimal(dtoParam.Value);
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "TinyInt":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType, dtoParam.Size);
                            //cmdSql.Parameters[dtoParam.Name].Value = Convert.ToByte(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = Convert.ToByte(dtoParam.Value);
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "SmallInt":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType, dtoParam.Size);
                            //cmdSql.Parameters[dtoParam.Name].Value = Convert.ToInt16(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = Convert.ToInt16(dtoParam.Value);
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "Int":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType);
                            //cmdSql.Parameters[dtoParam.Name].Value = Convert.ToInt32(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = Convert.ToInt32(dtoParam.Value);
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "DateTime":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType);
                            //cmdSql.Parameters[dtoParam.Name].Value = Convert.ToDateTime(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = Convert.ToDateTime(dtoParam.Value);
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "Bit":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType, dtoParam.Size);
                            //cmdSql.Parameters[dtoParam.Name].Value = Convert.ToByte(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = Convert.ToByte(dtoParam.Value);
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;

                        case "VarBinary":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType, dtoParam.Size);
                            //cmdSql.Parameters[dtoParam.Name].Value = Convert.ToByte(dtoParam.Value);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.Value;
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = (byte[]) dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;
                        case "Structured":
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType);
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.Value;
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            // Set TypeName, this is used for table parameters
                            cmdSql.Parameters[dtoParam.Name].TypeName = dtoParam.TypeName;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;
                        default:
                            cmdSql.Parameters.Add(dtoParam.Name, dtoParam.DataType);
                            //cmdSql.Parameters[dtoParam.Name].Value = dtoParam.Value;
                            if (!string.IsNullOrWhiteSpace(dtoParam.Value))
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.Value;
                            else if (dtoParam.ObjectValue != null)
                                cmdSql.Parameters[dtoParam.Name].Value = dtoParam.ObjectValue;
                            else
                                cmdSql.Parameters[dtoParam.Name].Value = DBNull.Value;

                            cmdSql.Parameters[dtoParam.Name].Direction = ParameterDirection.Input;
                            break;
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
        public long updateData(string sProcedureName, ref string sError, SqlDataAccessContainer dtoContainer)
        {
            try
            {
                //add the parameters if they exist
                if (dtoContainer != null)
                    AddParameterValues(dtoContainer.AllParameters());

                //add the return value parameter
                cmdSql.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                cmdSql.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                //check for connection strings here
                if (OpenConnection())
                {
                    cmdSql.Connection = conDB;
                    cmdSql.CommandType = CommandType.StoredProcedure;
                    cmdSql.CommandText = sProcedureName;
                    cmdSql.CommandTimeout = _commandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + logMsg;
                    return 0;
                }

                //check for transactions
                if (bolTran)
                    cmdSql.Transaction = oTran;

                cmdSql.ExecuteNonQuery();
                long nReturn = long.Parse(cmdSql.Parameters["@RETURN_VALUE"].Value.ToString());

                cmdSql.Connection = null;
                cmdSql.Parameters.Clear();

                return nReturn;
            }

            catch (Exception e)
            {
                if (bolTran) rollBackTransaction();

                sError = e.Message;
                cmdSql.Parameters.Clear();
                return 0;
            }
            finally
            {
                closeConnection();
                if (cmdSql != null && bolTran == false) cmdSql.Dispose();
            }
        }

        #endregion

        /// <summary>
        ///     Insert a record via inline SQL
        /// </summary>
        /// <param name="sError">String value that contains an error message if error occurs</param>
        /// <param name="ssql">Inline insert statement</param>
        /// <returns>A <c>long</c> with the data returned from <c>scope_identity()</c> in database</returns>
        public long insertRawSqlData(ref string sError, string ssql)
        {
            try
            {
                //check for connection strings here
                if (OpenConnection())
                {
                    //append the scope_identity her
                    ssql = ssql + "; select scope_identity()";
                    cmdSql.Connection = conDB;
                    cmdSql.CommandText = ssql;
                    cmdSql.CommandType = CommandType.Text;
                    cmdSql.CommandTimeout = _commandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + logMsg;
                    return 0;
                }

                //check for transactions
                if (bolTran)
                    cmdSql.Transaction = oTran;

                object nReturn = cmdSql.ExecuteScalar();

                cmdSql.Connection = null;
                cmdSql.Parameters.Clear();

                if (nReturn == null) return 0;
                if (nReturn.ToString() == string.Empty) return 0;
                return int.Parse(nReturn.ToString());
            }

            catch (Exception e)
            {
                if (bolTran) rollBackTransaction();

                sError = e.Message;
                cmdSql.Parameters.Clear();
                return -1;
            }
            finally
            {
                closeConnection();
                if (cmdSql != null && bolTran == false) cmdSql.Dispose();
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
                    cmdSql.Connection = conDB;
                    cmdSql.CommandText = ssql;
                    cmdSql.CommandType = CommandType.Text;
                    cmdSql.CommandTimeout = _commandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + logMsg;
                    return 0;
                }

                //check for transactions
                if (bolTran)
                    cmdSql.Transaction = oTran;

                int nReturn = cmdSql.ExecuteNonQuery();

                return nReturn;
            }

            catch (Exception e)
            {
                if (bolTran) rollBackTransaction();

                sError = e.Message;
                return -1;
            }
            finally
            {
                closeConnection();
                if (cmdSql != null && bolTran == false) cmdSql.Dispose();
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
                    throw new Exception("The number of records modified needs to be greater than 0.");

                //check for connection strings here
                if (OpenConnection())
                {
                    cmdSql.Connection = conDB;
                    cmdSql.CommandText = ssql;
                    cmdSql.CommandType = CommandType.Text;
                    cmdSql.CommandTimeout = _commandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + logMsg;
                    return 0;
                }

                startTransaction();

                //check for transactions
                if (bolTran)
                    cmdSql.Transaction = oTran;

                //append the scope_identity her
                int nReturn = cmdSql.ExecuteNonQuery();
                if (nReturn != numberRecordsUpdated)
                    throw new Exception("The number of records to be updated is: " + nReturn +
                                        ", but the desired number of records to modify was: " + numberRecordsUpdated +
                                        ".  Transaction is rolled back.");
                endTransaction();

                return nReturn;
            }

            catch (Exception e)
            {
                if (bolTran) rollBackTransaction();

                sError = e.Message;
                return -1;
            }
            finally
            {
                closeConnection();
                if (cmdSql != null && bolTran == false) cmdSql.Dispose();
            }
        }

        #region "Transaction and open connections"

        /// <summary>
        ///     Sets connection string to default connection string
        /// </summary>
        public void applySettings()
        {
            try
            {
                string sConn = ConfigurationManager.ConnectionStrings["defaultConnectionString"]
                    .ToString();
                conDB = new SqlConnection(sConn);
            }

            catch (Exception e)
            {
                logMsg = e.Message;
            }
        }

        /// <summary>
        ///     Change the connection string used
        /// </summary>
        /// <param name="_conn">Connection string value</param>
        /// <returns>true if connection is set successfully, false if any errors occurred</returns>
        public bool ChangeConnectionString(string _conn)
        {
            try
            {
                if (conDB != null) conDB = null;

                conDB = new SqlConnection(_conn);
                return true;
            }
            catch (Exception ex)
            {
                logMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        ///     Open connection to database
        /// </summary>
        /// <returns>True if connection is open, false if error occurred</returns>
        public bool OpenConnection()
        {
            if (conDB != null)
                try
                {
                    if (conDB.State != ConnectionState.Open)
                    {
                        conDB.Open();
                        return true;
                    }

                    if (conDB.State == ConnectionState.Open) return true;
                }
                catch (Exception e)
                {
                    logMsg = e.Message;
                    return false;
                }

            return false;
        }

        /// <summary>
        ///     Closes the connection
        /// </summary>
        /// <returns>True if connection is closed, false if error occurred</returns>
        public bool closeConnection()
        {
            if (conDB != null && bolTran == false)
                try
                {
                    if (conDB.State != ConnectionState.Closed)
                    {
                        conDB.Close();
                        return true;
                    }

                    if (conDB.State == ConnectionState.Closed)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    logMsg = e.Message;
                    return false;
                }

            return false;
        }

        /// <summary>
        ///     Start a database transaction
        /// </summary>
        public void startTransaction()
        {
            if (OpenConnection())
                if (oTran == null)
                {
                    oTran = conDB.BeginTransaction();
                    bolTran = true;
                }
        }

        /// <summary>
        ///     Commit the transaction.  The closeConnection still needs to be called to close the connection.
        /// </summary>
        public void endTransaction()
        {
            if (oTran != null)
            {
                oTran.Commit();
                oTran.Dispose();
                oTran = null;
                bolTran = false;
            }
        }

        /// <summary>
        ///     Rollback the transaction.  The closeConnection still needs to be called to close the connection.
        /// </summary>
        public void rollBackTransaction()
        {
            if (oTran != null)
            {
                oTran.Rollback();
                oTran.Dispose();
                oTran = null;
                bolTran = false;
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
        public bool getData(ref DataSet dsData, string sProcedureName, ref string sError,
            SqlDataAccessContainer dtoContainer)
        {
            try
            {
                //add the parameters if they exist
                if (dtoContainer != null)
                    AddParameterValues(dtoContainer.AllParameters());

                if (OpenConnection())
                {
                    cmdSql.Connection = conDB;
                    cmdSql.CommandType = CommandType.StoredProcedure;
                    cmdSql.CommandText = sProcedureName;
                    cmdSql.CommandTimeout = _commandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + logMsg;
                    return false;
                }

                Adprdata = new SqlDataAdapter(cmdSql);
                dsData = new DataSet();
                Adprdata.Fill(dsData);
                Adprdata.Dispose();
                cmdSql.Parameters.Clear();
                return true;
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                if (cmdSql != null)
                    cmdSql.Parameters.Clear();

                return false;
            }
            finally
            {
                closeConnection();
                if (cmdSql != null && bolTran == false) cmdSql.Dispose();
            }
        }

        /// <summary>
        ///     Get data via inline SQL
        /// </summary>
        /// <param name="dsData">DataSet that will be filled with data if successful</param>
        /// <param name="sSqlString">Inline SQL to be ran</param>
        /// <param name="sError">String value that contains an error message if error occurs</param>
        /// <returns>true if SQL ran successfully, false if any errors occurred</returns>
        public bool getData(ref DataSet dsData, string sSqlString, ref string sError)
        {
            try
            {
                if (OpenConnection())
                {
                    cmdSql = new SqlCommand();
                    cmdSql.Connection = conDB;
                    cmdSql.CommandType = CommandType.Text;
                    cmdSql.CommandText = sSqlString;
                    cmdSql.CommandTimeout = _commandTimeOut;
                }
                else
                {
                    sError = "Unable to open the connection in SqlDataConnect. " + logMsg;
                    return false;
                }

                Adprdata = new SqlDataAdapter(cmdSql);
                dsData = new DataSet();
                Adprdata.Fill(dsData);
                Adprdata.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                if (cmdSql != null)
                    cmdSql.Parameters.Clear();

                return false;
            }
            finally
            {
                closeConnection();
                if (cmdSql != null && bolTran == false) cmdSql.Dispose();
            }
        }

        #endregion
    }
}