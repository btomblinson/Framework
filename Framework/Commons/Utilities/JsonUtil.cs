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
        public static string DataSetToJSON(DataSet ds)
        {
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            var list = new List<Dictionary<string, object>>();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow Row in dt.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn Col in dt.Columns) dict[Col.ColumnName] = Convert.ToString(Row[Col]);

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
        /// <param name="JsonFilePath">FilePath to JSON File</param>
        /// <returns>Returns Json String.</returns>
        public static string LoadJsonFromFile(string JsonFilePath)
        {
            string results = "";

            if (!string.IsNullOrWhiteSpace(JsonFilePath))
            {
                if (CheckIfFileIsJson(JsonFilePath))
                {
                    Logger.LogInfo("Loading Json from FilePath = " + JsonFilePath);
                    using (StreamReader r = new StreamReader(JsonFilePath))
                    {
                        results = r.ReadToEnd();
                    }

                    Logger.LogInfo("Done Loading Json from FilePath = " + JsonFilePath);
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
        ///     <param name="UploadedFile">Uploaded JSON File</param>
        ///     <returns>Returns Json String.</returns>
        public static string LoadJsonFromFile(FileUpload UploadedFile)
        {
            string results = "";

            if (UploadedFile != null)
            {
                if (!string.IsNullOrWhiteSpace(UploadedFile.PostedFile.FileName))
                {
                    string FileName = UploadedFile.PostedFile.FileName;

                    if (CheckIfFileIsJson(FileName))
                    {
                        Logger.LogInfo("Loading Json from Uploaded File = " + FileName);
                        using (StreamReader r =
                            new StreamReader(UploadedFile.PostedFile.InputStream))
                        {
                            results = r.ReadToEnd();
                        }

                        Logger.LogInfo("Done Loading Json from Uploaded File = " + FileName);
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
        /// <param name="FilePath">Filepath to check to see if extension of filepath is '.json'</param>
        /// <returns>Returns Booelaan on wether the FilePath contains the JSON extension or not</returns>
        public static bool CheckIfFileIsJson(string FilePath)
        {
            bool test = false;

            try
            {
                test = Path.GetExtension(FilePath).EqualsIgnoreCase(".json");
            }
            catch (Exception)
            {
            }

            return test;
        }

        /// <summary>Saves JSON to the given FilePath without Impersonation</summary>
        /// <param name="FilePath">Filepath to write JSON contents to</param>
        /// <param name="JsonContent">JSON data that will be saved</param>
        public static void SaveJsonToAFile(string FilePath, string JsonContent)
        {
            if (CheckIfFileIsJson(FilePath))
                File.WriteAllText(FilePath, JsonContent);
            else
                throw new Exception(
                    "The FilePath provided to save the JSON to a file doesn't contain the correct Extension for a JSON File");
        }

        /// <summary>Saves JSON to the given FilePath with Impersonation</summary>
        /// <param name="FilePath">Filepath to write JSON contents to</param>
        /// <param name="JsonContent">JSON data that will be saved</param>
        /// <param name="UserName">Username of identity that has access to save in the specified file location</param>
        /// <param name="Password">Password of identity that has access to save in the specified file location</param>
        /// <param name="Domain">Domain of identity that has access to save in the specified file location</param>
        public static void SaveJsonToAFile(string FilePath, string JsonContent, string UserName, string Password,
            string Domain)
        {
            if (CheckIfFileIsJson(FilePath))
                using (new Impersonator(UserName, Domain, Password))
                {
                    File.WriteAllText(FilePath, JsonContent);
                }
            else
                throw new Exception(
                    "The FilePath provided to save the JSON to a file doesn't contain the correct Extension for a JSON File");
        }
    }
}