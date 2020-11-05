using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Framework.Commons.Logging;
using Framework.Security;
using Newtonsoft.Json;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace Framework.Commons.Utilities
{
    /// <summary>Extra utilities for JSON data</summary>
    public static class JsonUtil
    {
        /// <summary>Converts first table in DataSet to JSON</summary>
        /// <param name="ds">Dataset with a table with data.  Only the first table will converted</param>
        public static string DataSetToJson(DataSet ds)
        {
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            var list = new List<Dictionary<string, object>>();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        dict[col.ColumnName] = Convert.ToString(row[col]);
                    }

                    list.Add(dict);
                }
            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(list);
        }

        /// <summary>Try Deserialize Json to check if its valid</summary>
        /// <param name="json">Json string to see if its valid</param>
        /// <returns>Any Errors when string is deserialized to json</returns>
        public static List<string> IsValidJsonReturnErrors(string json)
        {
            var messages = new List<string>();

            JsonConvert.DeserializeObject(json, new JsonSerializerSettings
            {
                Error = delegate(object sender, ErrorEventArgs args2)
                {
                    messages.Add(args2.ErrorContext.Error.Message);
                    args2.ErrorContext.Handled = true;
                }
            });

            return messages;
        }

        /// <summary>Try Deserialize Json to check if its valid</summary>
        /// <param name="json">Json string to see if its valid</param>
        /// <returns>Bool tells wether its valid or not</returns>
        public static bool IsValidJson(string json)
        {
            bool test = true;

            JsonConvert.DeserializeObject(json, new JsonSerializerSettings
            {
                Error = delegate(object sender, ErrorEventArgs args2)
                {
                    test = false;
                    args2.ErrorContext.Handled = true;
                }
            });

            return test;
        }

        /// <summary>Reads a json file from a filepath on the server </summary>
        /// <param name="jsonFilePath">FilePath to JSON File</param>
        /// <returns>Returns Json String.</returns>
        public static string LoadJsonFromFile(string jsonFilePath)
        {
            string results = "";

            if (!string.IsNullOrWhiteSpace(jsonFilePath))
            {
                if (CheckIfFileIsJson(jsonFilePath))
                {
                    Logger.LogInfo("Loading Json from FilePath = " + jsonFilePath);
                    using (StreamReader r = new StreamReader(jsonFilePath))
                    {
                        results = r.ReadToEnd();
                    }

                    Logger.LogInfo("Done Loading Json from FilePath = " + jsonFilePath);
                }
                else
                {
                    throw new Exception(
                        "The file Extension for the File doesn't contain the proper extension for a json file");
                }
            }
            else
            {
                throw new Exception("No FilePath was provided when trying to Load Json From a FilePath");
            }

            return results;
        }

        /// <summary>
        ///     Reads the json from a Uploaded File.  If UploadedFile contains multiple files it will only read the first file in
        ///     the InputStream/summary>
        ///     <param name="uploadedFile">Uploaded JSON File</param>
        ///     <returns>Returns Json String.</returns>
        public static string LoadJsonFromFile(FileUpload uploadedFile)
        {
            string results = "";

            if (uploadedFile != null)
            {
                if (!string.IsNullOrWhiteSpace(uploadedFile.PostedFile.FileName))
                {
                    string fileName = uploadedFile.PostedFile.FileName;

                    if (CheckIfFileIsJson(fileName))
                    {
                        Logger.LogInfo("Loading Json from Uploaded File = " + fileName);
                        using (StreamReader r =
                            new StreamReader(uploadedFile.PostedFile.InputStream))
                        {
                            results = r.ReadToEnd();
                        }

                        Logger.LogInfo("Done Loading Json from Uploaded File = " + fileName);
                    }
                    else
                    {
                        throw new Exception(
                            "The File that was uploaded doesn't contain the proper extension for a json file.");
                    }
                }
                else
                {
                    throw new Exception("Unable to get the FileName from the Uploaded File");
                }
            }
            else
            {
                throw new Exception("The UploadedFile parameter was null");
            }

            return results;
        }

        /// <summary>Checks if the FilePath passed in contains the JSON Extension</summary>
        /// <param name="filePath">Filepath to check to see if extension of filepath is '.json'</param>
        /// <returns>Returns Booelaan on wether the FilePath contains the JSON extension or not</returns>
        public static bool CheckIfFileIsJson(string filePath)
        {
            bool test = false;

            try
            {
                test = Path.GetExtension(filePath).EqualsIgnoreCase(".json");
            }
            catch (Exception)
            {
            }

            return test;
        }

        /// <summary>Saves JSON to the given FilePath without Impersonation</summary>
        /// <param name="filePath">Filepath to write JSON contents to</param>
        /// <param name="jsonContent">JSON data that will be saved</param>
        public static void SaveJsonToAFile(string filePath, string jsonContent)
        {
            if (CheckIfFileIsJson(filePath))
            {
                File.WriteAllText(filePath, jsonContent);
            }
            else
            {
                throw new Exception(
                    "The FilePath provided to save the JSON to a file doesn't contain the correct Extension for a JSON File");
            }
        }

        /// <summary>Saves JSON to the given FilePath with Impersonation</summary>
        /// <param name="filePath">Filepath to write JSON contents to</param>
        /// <param name="jsonContent">JSON data that will be saved</param>
        /// <param name="userName">Username of identity that has access to save in the specified file location</param>
        /// <param name="password">Password of identity that has access to save in the specified file location</param>
        /// <param name="domain">Domain of identity that has access to save in the specified file location</param>
        public static void SaveJsonToAFile(string filePath, string jsonContent, string userName, string password,
            string domain)
        {
            if (CheckIfFileIsJson(filePath))
            {
                using (new Impersonator(userName, domain, password))
                {
                    File.WriteAllText(filePath, jsonContent);
                }
            }
            else
            {
                throw new Exception(
                    "The FilePath provided to save the JSON to a file doesn't contain the correct Extension for a JSON File");
            }
        }
    }
}