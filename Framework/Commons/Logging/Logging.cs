using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Config;

namespace Framework.Commons.Logging
{
    /// <summary>
    ///     Sets up logging class
    /// </summary>
    /// <example>Logger.log.Error("Error", ex);</example>
    /// <remarks>You need to add this to your code: Logger.InitializeLogging();</remarks>
    public static class Logger
    {
        /// <summary>
        ///     Object that does the logging
        /// </summary>
        public static readonly ILog Log = LogManager.GetLogger("LogFileAppender");

        /// <summary>
        ///     Is the logging initialized
        /// </summary>
        private static bool _IsLoggingInitialized;

        /// <summary>
        ///     Initialize Logging
        /// </summary>
        /// <returns>True when configured</returns>
        public static bool InitializeLogging()
        {
            if (!_IsLoggingInitialized)
            {
                XmlConfigurator.Configure();
                _IsLoggingInitialized = true;
            }

            return true;
        }

        /// <summary>
        ///     Log error message
        /// </summary>
        /// <param name="error">Error message to log</param>
        public static void LogError(string error)
        {
            InitializeLogging();
            Log.Error(error);
        }

        /// <summary>
        ///     Log info message
        /// </summary>
        /// <param name="info">Info message to log</param>
        public static void LogInfo(string info)
        {
            InitializeLogging();
            Log.Info(info);
        }

        /// <summary>
        ///     Log debug message
        /// </summary>
        /// <param name="debug">Debug message to log</param>
        public static void LogDebug(string debug)
        {
            InitializeLogging();
            Log.Debug(debug);
        }


        /// <summary>
        ///     Log error exception
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogError(Exception ex)
        {
            InitializeLogging();
            Log.Error("", ex);
        }

        /// <summary>
        ///     Log error message and include procedure name
        /// </summary>
        /// <param name="error">Error message to log</param>
        /// <param name="procedureName">Procedure name to log</param>
        public static void LogError(string error, string procedureName)
        {
            InitializeLogging();
            Log.Error("Error while executing " + procedureName + ": " + error);
        }

        /// <summary>
        ///     Log exception and include procedure name
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Exception to log</param>
        public static void LogError(string message, Exception ex)
        {
            InitializeLogging();
            Log.Error(message, ex);
        }

        /// <summary>
        ///     Log exception, procedure, and message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="procedureName">Procedure name to log</param>
        /// <param name="ex">Exception to log</param>
        public static void LogError(string message, string procedureName, Exception ex)
        {
            InitializeLogging();
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                LogError(message, ex);
            }
            else
            {
                Log.Error("Error while executing " + procedureName + ": " + message, ex);
            }
        }

        /// <summary>
        ///     Log exception, procedure, and procedure parameters
        /// </summary>
        /// <param name="procedureName">Procedure name to log</param>
        /// <param name="sqlDataAccessParameter">SqlDataAccessParameter or SqlDataAccessParameter[]</param>
        /// <param name="ex">Exception to log</param>
        public static void LogError(string procedureName, dynamic sqlDataAccessParameter, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Error while executing ").Append(procedureName).Append(Environment.NewLine);
            if (sqlDataAccessParameter.GetType().IsArray)
            {
                foreach (dynamic dataAccessParameter in sqlDataAccessParameter)
                {
                    sb.Append("Parameter Name: ").Append(dataAccessParameter.Name);
                    sb.Append(": ");
                    if (dataAccessParameter.Value != null)
                    {
                        sb.Append(dataAccessParameter.Value);
                    }

                    sb.Append(Environment.NewLine);
                }
            }
            else
            {
                sb.Append("Parameter Name: ").Append(sqlDataAccessParameter.Name);
                sb.Append(": ");
                if (sqlDataAccessParameter.Value != null)
                {
                    sb.Append(sqlDataAccessParameter.Value);
                }

                sb.Append(Environment.NewLine);
            }

            Log.Error(sb.ToString(), ex);
        }

        /// <summary>
        ///     Log fatal exception
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogFatal(Exception ex)
        {
            InitializeLogging();
            Log.Fatal("", ex);
        }

        /// <summary>
        ///     Log fatal and include message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Exception to log</param>
        public static void LogFatal(string message, Exception ex)
        {
            InitializeLogging();
            Log.Fatal(message, ex);
        }


        /// <summary>
        ///     Log procedure parameters
        /// </summary>
        /// <param name="sqlDataAccessParameter">SqlDataAcessParameter, SqlDataAccessParameter[] or IList of SqlDataAccessParameter</param>
        public static void LogParameters(dynamic sqlDataAccessParameter)
        {
            StringBuilder sb = new StringBuilder();
            if (sqlDataAccessParameter.GetType().IsArray || IsList(sqlDataAccessParameter))
            {
                sb.Append("Parameters: ").Append(Environment.NewLine);
                foreach (dynamic dataAccessParameter in sqlDataAccessParameter)
                {
                    sb.Append(dataAccessParameter.Name);
                    sb.Append(": ");
                    if (dataAccessParameter.Value != null)
                    {
                        sb.Append(dataAccessParameter.Value);
                    }
                    else if (dataAccessParameter.ObjectValue != null)
                    {
                        sb.Append(dataAccessParameter.ObjectValue);
                    }

                    sb.Append(" (").Append(dataAccessParameter.DataType).Append(")");
                    sb.Append(Environment.NewLine);
                }
            }
            else if (sqlDataAccessParameter.GetType().ToString() == "Framework.DataAccess.SqlDataAccessParameter")
            {
                sb.Append("Parameter: ").Append(Environment.NewLine);
                sb.Append(sqlDataAccessParameter.Name);
                sb.Append(": ");
                if (sqlDataAccessParameter.Value != null)
                {
                    sb.Append(sqlDataAccessParameter.Value);
                }

                sb.Append(Environment.NewLine);
            }

            if (sb.Length > 0)
            {
                LogInfo(sb.ToString());
            }
        }

        private static bool IsList(object o)
        {
            if (o == null)
            {
                return false;
            }

            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}