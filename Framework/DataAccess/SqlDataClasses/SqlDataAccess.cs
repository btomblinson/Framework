﻿using System;
using System.Data;
using Framework.Commons.CommonObj;
using Framework.Commons.Logging;
using Framework.Commons.Utilities;

namespace Framework.DataAccess.SqlDataClasses
{
    /// <summary>
    ///     Access data from stored procedure
    /// </summary>
    public class SqlDataAccess
    {
        private string _Error = "";

        /// <summary>
        ///     SqlDataConnect object
        /// </summary>
        public SqlDataConnect SqlDataConnect;

        /// <summary>
        ///     Token String
        /// </summary>
        public string Token;

        private bool _TurnOnLogging = Logger.InitializeLogging();

        /// <summary>
        ///     Instantiates SqlDataConnect class
        /// </summary>
        public SqlDataAccess(SqlDatabaseType type)
        {
            SqlDataConnect = new SqlDataConnect(type);
        }

        /// <summary>
        ///     Instantiates and sets SqlDataConnect class
        /// </summary>
        /// <param name="sqlDataConnect"></param>
        public SqlDataAccess(SqlDataConnect sqlDataConnect)
        {
            SqlDataConnect = sqlDataConnect;
        }

        /// <summary>
        ///     Instantiates and sets SqlDataConnect class
        /// </summary>
        /// <param name="sqlDataConnect"></param>
        public SqlDataAccess(SqlDataConnect sqlDataConnect, string token)
        {
            SqlDataConnect = sqlDataConnect;
            Token = token;
        }

        /// <summary>
        ///     Gets data from stored procedure without need of parameters
        /// </summary>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>DtoDataSet filled with data from database</returns>
        public DtoDataSet GetData(string procedureName)
        {
            return GetData(new SqlDataAccessParameter[] { }, procedureName);
        }

        /// <summary>
        ///     Gets data from stored procedure with one parameter
        /// </summary>
        /// <param name="parameter">Single parameter to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>DtoDataSet filled with data from database</returns>
        public DtoDataSet GetData(SqlDataAccessParameter parameter, string procedureName)
        {
            return GetData(new[] {parameter}, procedureName);
        }

        /// <summary>
        ///     Gets data from stored procedure with multiple parameters
        /// </summary>
        /// <param name="parameters">Array of parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>DtoDataSet filled with data from database</returns>
        public DtoDataSet GetData(SqlDataAccessParameter[] parameters, string procedureName)
        {
            DtoDataSet infoDataSet = new DtoDataSet();
            SqlDataAccessContainer dac = new SqlDataAccessContainer();

            if (parameters != null)
            {
                foreach (SqlDataAccessParameter parameter in parameters)
                {
                    if (parameter != null && parameter.Value != null && parameter.Value.Length > 0)
                    {
                        dac.Add(parameter);
                    }
                    else if (parameter != null && parameter.ObjectValue != null)
                    {
                        dac.Add(parameter);
                    }
                }
            }

            infoDataSet.Ds = null;
            infoDataSet.Error = "";

            DataSet localDs = null;

            try
            {
                if (SqlDataConnect.GetData(ref localDs, procedureName, ref _Error, dac) == false)
                {
                    infoDataSet.Error = _Error;
                    infoDataSet.Ds = null;
                }

                infoDataSet.Ds = localDs;
            }
            catch (Exception ex)
            {
                infoDataSet.Error = ex.Message;
                Logger.LogError(procedureName, parameters, ex);
            }

            return infoDataSet;
        }

        /// <summary>
        ///     Gets data from stored procedure with multiple parameters.  Any null values will be sent to stored procedure.
        /// </summary>
        /// <param name="parameters">Array of parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>DtoDataSet filled with data from database</returns>
        public DtoDataSet GetDataKeepNullParameters(SqlDataAccessParameter[] parameters, string procedureName)
        {
            DtoDataSet infoDataSet = new DtoDataSet();
            SqlDataAccessContainer dac = new SqlDataAccessContainer();

            if (parameters != null)
            {
                foreach (SqlDataAccessParameter parameter in parameters)
                {
                    if (parameter != null && parameter.Value != null && parameter.Value.Length > 0)
                    {
                        dac.Add(parameter);
                    }
                    else if (parameter != null && parameter.ObjectValue != null)
                    {
                        dac.Add(parameter);
                    }
                    else
                    {
                        dac.Add(new SqlDataAccessParameter(parameter.Name, parameter.DataType, DBNull.Value));
                    }
                }
            }

            infoDataSet.Ds = null;
            infoDataSet.Error = "";

            DataSet localDs = null;

            try
            {
                if (SqlDataConnect.GetData(ref localDs, procedureName, ref _Error, dac) == false)
                {
                    infoDataSet.Error = _Error;
                    infoDataSet.Ds = null;
                }

                infoDataSet.Ds = localDs;
            }
            catch (Exception ex)
            {
                infoDataSet.Error = ex.Message;
                Logger.LogError(procedureName, parameters, ex);
            }

            return infoDataSet;
        }

        /// <summary>
        ///     Gets a dataset back from a stored procedure that modifies data
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="encryptedToken">Encrypted token value coming from client</param>
        /// <param name="unencryptedToken">Unencrypted token value from web.config</param>
        /// <returns></returns>
        public DtoDataSet ModifyDataReturnTable(SqlDataAccessParameter[] parameters, string procedureName,
            string encryptedToken, string unencryptedToken)
        {
            DtoDataSet infoDataSet = new DtoDataSet();
            SqlDataAccessContainer dac = new SqlDataAccessContainer();
            if (TokenGenerator.CompareTokens(encryptedToken, unencryptedToken))
            {
                foreach (SqlDataAccessParameter parameter in parameters)
                {
                    dac.Add(parameter);
                }

                infoDataSet.Ds = null;
                infoDataSet.Error = "";

                DataSet localDs = null;

                try
                {
                    if (SqlDataConnect.GetData(ref localDs, procedureName, ref _Error, dac) == false)
                    {
                        infoDataSet.Error = _Error;
                        infoDataSet.Ds = null;
                    }

                    infoDataSet.Ds = localDs;
                }
                catch (Exception ex)
                {
                    infoDataSet.Error = ex.Message;
                    Logger.LogError(procedureName, parameters, ex);
                }
            }
            else
            {
                infoDataSet.Error = "Security token validation failed.  Access to service is denied.";
            }

            return infoDataSet;
        }

        /// <summary>
        ///     Gets a dataset back from a stored procedure that modifies data and sends any NULL parameters to database
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="encryptedToken">Encrypted token value coming from client</param>
        /// <param name="unencryptedToken">Unencrypted token value from web.config</param>
        /// <returns></returns>
        public DtoDataSet ModifyDataReturnTableKeepNullParameters(SqlDataAccessParameter[] parameters,
            string procedureName, string encryptedToken, string unencryptedToken)
        {
            DtoDataSet infoDataSet = new DtoDataSet();
            SqlDataAccessContainer dac = new SqlDataAccessContainer();
            if (TokenGenerator.CompareTokens(encryptedToken, unencryptedToken))
            {
                if (parameters != null)
                {
                    foreach (SqlDataAccessParameter parameter in parameters)
                    {
                        if (parameter != null && parameter.Value != null && parameter.Value.Length > 0)
                        {
                            dac.Add(parameter);
                        }
                        else if (parameter != null && parameter.ObjectValue != null)
                        {
                            dac.Add(parameter);
                        }
                        else
                        {
                            dac.Add(new SqlDataAccessParameter(parameter.Name, parameter.DataType, DBNull.Value));
                        }
                    }
                }

                infoDataSet.Ds = null;
                infoDataSet.Error = "";

                DataSet localDs = null;

                try
                {
                    if (SqlDataConnect.GetData(ref localDs, procedureName, ref _Error, dac) == false)
                    {
                        infoDataSet.Error = _Error;
                        infoDataSet.Ds = null;
                    }

                    infoDataSet.Ds = localDs;
                }
                catch (Exception ex)
                {
                    infoDataSet.Error = ex.Message;
                    Logger.LogError(procedureName, parameters, ex);
                }
            }
            else
            {
                infoDataSet.Error = "Security token validation failed.  Access to service is denied.";
            }

            return infoDataSet;
        }

        /// <summary>
        ///     Insert, Update, or Delete data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj ModifyData(SqlDataAccessParameter[] parameters, string procedureName)
        {
            DtoDataSet infoDataSet = new DtoDataSet();
            SqlDataAccessContainer dac = new SqlDataAccessContainer();

            foreach (SqlDataAccessParameter parameter in parameters)
            {
                dac.Add(parameter);
            }

            string error = string.Empty;
            ReturnObj objReturn = new ReturnObj();
            try
            {
                long returnValue;
                returnValue = SqlDataConnect.UpdateData(procedureName, ref error, dac);
                if (returnValue <= 0)
                {
                    objReturn.Error = error;
                }

                objReturn.Result = returnValue;
            }
            catch (Exception ex)
            {
                //log this error 
                objReturn.Result = 0;
                objReturn.Error = ex.Message;
                Logger.Log.Error(ex);
            }

            return objReturn;
        }

        /// <summary>
        ///     Insert, Update, or Delete data via stored procedure.  Any null values will be sent to stored procedure.
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj ModifyDataKeepNullParameters(SqlDataAccessParameter[] parameters, string procedureName)
        {
            DtoDataSet infoDataSet = new DtoDataSet();
            SqlDataAccessContainer dac = new SqlDataAccessContainer();

            foreach (SqlDataAccessParameter parameter in parameters)
            {
                if (parameter != null && parameter.Value != null && parameter.Value.Length > 0)
                {
                    dac.Add(parameter);
                }
                else if (parameter != null && parameter.ObjectValue != null)
                {
                    dac.Add(parameter);
                }
                else
                {
                    dac.Add(new SqlDataAccessParameter(parameter.Name, parameter.DataType, DBNull.Value));
                }
            }

            string error = string.Empty;
            ReturnObj objReturn = new ReturnObj();
            try
            {
                long returnValue;
                returnValue = SqlDataConnect.UpdateData(procedureName, ref error, dac);
                if (returnValue <= 0)
                {
                    objReturn.Error = error;
                }

                objReturn.Result = returnValue;
            }
            catch (Exception ex)
            {
                //log this error 
                objReturn.Result = 0;
                objReturn.Error = ex.Message;
                Logger.Log.Error(ex);
            }

            return objReturn;
        }

        /// <summary>
        ///     Insert, Update, or Delete data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="encryptedToken">Encrypted token value coming from client</param>
        /// <param name="unencryptedToken">Unencrypted token value from web.config</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj ModifyData(SqlDataAccessParameter[] parameters, string procedureName, string encryptedToken,
            string unencryptedToken)
        {
            ReturnObj objReturn = new ReturnObj();

            if (TokenGenerator.CompareTokens(encryptedToken, unencryptedToken))
            {
                SqlDataAccessContainer dac = new SqlDataAccessContainer();

                foreach (SqlDataAccessParameter parameter in parameters)
                {
                    dac.Add(parameter);
                }

                try
                {
                    long returnValue;
                    returnValue = SqlDataConnect.UpdateData(procedureName, ref _Error, dac);
                    if (returnValue <= 0)
                    {
                        objReturn.Error = _Error;
                        Logger.LogError(_Error, procedureName);
                    }

                    objReturn.Result = returnValue;
                }
                catch (Exception ex)
                {
                    //log this error 
                    objReturn.Result = 0;
                    objReturn.Error = ex.Message;
                    Logger.LogError("", procedureName, ex);
                }
                finally
                {
                    SqlDataConnect.CloseConnection();
                }
            }
            else
            {
                objReturn.Error = "Security token validation failed.  Access to service is denied.";
            }

            return objReturn;
        }

        /// <summary>
        ///     Insert, Update, or Delete data via stored procedure.  Any null values will be sent to stored procedure.
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="encryptedToken">Encrypted token value coming from client</param>
        /// <param name="unencryptedToken">Unencrypted token value from web.config</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj ModifyDataKeepNullParameters(SqlDataAccessParameter[] parameters, string procedureName,
            string encryptedToken, string unencryptedToken)
        {
            ReturnObj objReturn = new ReturnObj();

            if (TokenGenerator.CompareTokens(encryptedToken, unencryptedToken))
            {
                SqlDataAccessContainer dac = new SqlDataAccessContainer();

                foreach (SqlDataAccessParameter parameter in parameters)
                {
                    if (parameter != null && parameter.Value != null && parameter.Value.Length > 0)
                    {
                        dac.Add(parameter);
                    }
                    else if (parameter != null && parameter.ObjectValue != null)
                    {
                        dac.Add(parameter);
                    }
                    else
                    {
                        dac.Add(new SqlDataAccessParameter(parameter.Name, parameter.DataType, DBNull.Value));
                    }
                }

                try
                {
                    long returnValue;
                    returnValue = SqlDataConnect.UpdateData(procedureName, ref _Error, dac);
                    if (returnValue <= 0)
                    {
                        objReturn.Error = _Error;
                        Logger.LogError(_Error, procedureName);
                    }

                    objReturn.Result = returnValue;
                }
                catch (Exception ex)
                {
                    //log this error 
                    objReturn.Result = 0;
                    objReturn.Error = ex.Message;
                    Logger.LogError("", procedureName, ex);
                }
                finally
                {
                    SqlDataConnect.CloseConnection();
                }
            }
            else
            {
                objReturn.Error = "Security token validation failed.  Access to service is denied.";
            }

            return objReturn;
        }

        /// <summary>
        ///     Insert data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj InsertData(SqlDataAccessParameter[] parameters, string procedureName)
        {
            return ModifyData(parameters, procedureName);
        }

        /// <summary>
        ///     Update data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj UpdateData(SqlDataAccessParameter[] parameters, string procedureName)
        {
            return ModifyData(parameters, procedureName);
        }

        /// <summary>
        ///     Delete data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj DeleteData(SqlDataAccessParameter[] parameters, string procedureName)
        {
            return ModifyData(parameters, procedureName);
        }

        /// <summary>
        ///     Insert data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="encryptedToken">Encrypted token value coming from client</param>
        /// <param name="unencryptedToken">Unencrypted token value from web.config</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj InsertData(SqlDataAccessParameter[] parameters, string procedureName, string encryptedToken,
            string unencryptedToken)
        {
            return ModifyData(parameters, procedureName, encryptedToken, unencryptedToken);
        }

        /// <summary>
        ///     Update data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="encryptedToken">Encrypted token value coming from client</param>
        /// <param name="unencryptedToken">Unencrypted token value from web.config</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj UpdateData(SqlDataAccessParameter[] parameters, string procedureName, string encryptedToken,
            string unencryptedToken)
        {
            return ModifyData(parameters, procedureName, encryptedToken, unencryptedToken);
        }

        /// <summary>
        ///     Delete data via stored procedure
        /// </summary>
        /// <param name="parameters">Parameters to send to database</param>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="encryptedToken">Encrypted token value coming from client</param>
        /// <param name="unencryptedToken">Unencrypted token value from web.config</param>
        /// <returns>Result object send from database</returns>
        public ReturnObj DeleteData(SqlDataAccessParameter[] parameters, string procedureName, string encryptedToken,
            string unencryptedToken)
        {
            return ModifyData(parameters, procedureName, encryptedToken, unencryptedToken);
        }
    }
}