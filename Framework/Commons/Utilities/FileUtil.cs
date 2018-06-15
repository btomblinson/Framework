using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Framework.Commons.Logging;
using Framework.Security;

namespace Framework.Commons.Utilities
{
    /// <summary>
    ///     Utility for encapsulating file management logic
    /// </summary>
    public static class FileUtil
    {
        /// <summary>
        ///     Helper method for deleting a file
        /// </summary>
        /// <param name="filepath">The file path to be delete</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static void DeleteFile(string filepath, bool useImpersonation, string username = "",
            string password = "", string domain = "sos.mo.gov")
        {
            if (useImpersonation)
                using (new Impersonator(username, domain, password))
                {
                    File.Delete(filepath);
                }
            else
                File.Delete(filepath);
        }

        /// <summary>
        ///     Helper method for uploading a file
        /// </summary>
        /// <param name="filepath">The file path to be delete</param>
        /// <param name="fileStream">The filestream of data to be written to the file</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static void UploadFile(string filepath, Stream fileStream, bool useImpersonation, string username = "",
            string password = "", string domain = "sos.mo.gov")
        {
            if (useImpersonation)
                using (new Impersonator(username, domain, password))
                {
                    SaveFile(filepath, fileStream);
                }
            else
                SaveFile(filepath, fileStream);
        }

        /// <summary>
        ///     Helper method for saving a file from a URL
        /// </summary>
        /// <param name="url">The url to download from</param>
        /// <param name="filePath">The file path to save to</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static void SaveFileWebRequest(string url, string filePath, bool useImpersonation, string username = "",
            string password = "", string domain = "sos.mo.gov")
        {
            Logger.LogInfo("Begin Saving File");
            Logger.LogInfo("URL=" + url);
            Logger.LogInfo("FilePath=" + filePath);

            WebClient client = new WebClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;
            client.UseDefaultCredentials = true;

            if (useImpersonation)
                using (new Impersonator(username, domain, password))
                {
                    SaveFile(filePath, new MemoryStream(client.DownloadData(url)));
                }
            else
                SaveFile(filePath, new MemoryStream(client.DownloadData(url)));


            Logger.LogInfo("Done Saving File");
        }

        /// <summary>
        ///     Helper method for downloading a file
        /// </summary>
        /// <param name="filepath">The file path to be downloaded</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static byte[] DownloadFile(string filepath, bool useImpersonation, string username = "",
            string password = "", string domain = "sos.mo.gov")
        {
            byte[] bytes;
            if (useImpersonation)
                using (new Impersonator(username, domain, password))
                {
                    bytes = File.ReadAllBytes(filepath);
                }
            else
                bytes = File.ReadAllBytes(filepath);

            return bytes;
        }

        /// <summary>
        ///     Helper method for downloading a file from a URL
        /// </summary>
        /// <param name="url">The url to download from</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static byte[] DownloadFileWebRequest(string url, bool useImpersonation, string username = "",
            string password = "", string domain = "sos.mo.gov")
        {
            Logger.LogInfo("Begin Saving File");
            Logger.LogInfo("URL=" + url);

            WebClient client = new WebClient();
            client.UseDefaultCredentials = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;
            if (useImpersonation)
                using (new Impersonator(username, domain, password))
                {
                    return client.DownloadData(url);
                }

            return client.DownloadData(url);
        }

        /// <summary>
        ///     Helper method for listing all files in a directory
        /// </summary>
        /// <param name="directoryPath">The directory to be listed</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static List<string> GetFileNames(string directoryPath, bool useImpersonation, string username = "",
            string password = "", string domain = "sos.mo.gov")
        {
            var files = new string[0];

            try
            {
                if (useImpersonation)
                    using (new Impersonator(username, domain, password))
                    {
                        files = GetFiles(directoryPath);
                    }
                else
                    files = GetFiles(directoryPath);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            var filenames = new List<string>();
            foreach (string f in files) filenames.Add(Path.GetFileName(f));

            return filenames;
        }

        /// <summary>
        ///     Helper method for moving a file
        /// </summary>
        /// <param name="sourcePath">The source file to be moved</param>
        /// <param name="destinationPath">The destination path of the file to be moved to</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static void MoveFile(string sourcePath, string destinationPath, bool useImpersonation,
            string username = "", string password = "", string domain = "sos.mo.gov")
        {
            if (useImpersonation)
                using (new Impersonator(username, domain, password))
                {
                    File.Move(sourcePath, destinationPath);
                }
            else
                File.Move(sourcePath, destinationPath);
        }

        /// <summary>
        ///     Helper method for moving a file
        /// </summary>
        /// <param name="directoryPath">The directory to be cleaned</param>
        /// <param name="hours">The number of hours a file has to be there before it gets cleaned up</param>
        /// <param name="useImpersonation">Whether or not this requires impersonation for permissions</param>
        /// <param name="username">The impersonated user's username</param>
        /// <param name="password">The impersonated user's password</param>
        /// <param name="domain">The user's domain, defaults to sos.mo.gov</param>
        public static void CleanUpTemporaryDirectory(string directoryPath, int hours, bool useImpersonation,
            string username = "", string password = "", string domain = "sos.mo.gov")
        {
            if (useImpersonation)
                using (new Impersonator(username, domain, password))
                {
                    CleanupDirectory(directoryPath, hours);
                }
            else
                CleanupDirectory(directoryPath, hours);
        }

        private static void CleanupDirectory(string directoryPath, int hours)
        {
            var directories = Directory.GetDirectories(directoryPath);

            foreach (string dir in directories)
            {
                string fullDirectoryPath = Path.Combine(directoryPath, dir);
                DateTime lastModified = Directory.GetLastWriteTime(fullDirectoryPath);

                if (lastModified.AddHours(hours) < DateTime.Now)
                {
                    var files = Directory.GetFiles(fullDirectoryPath);
                    foreach (string file in files)
                        try
                        {
                            File.Delete(Path.Combine(fullDirectoryPath, file));
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex);
                        }
                }

                try
                {
                    Directory.Delete(fullDirectoryPath);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            }
        }

        private static string[] GetFiles(string directoryPath)
        {
            var files = new string[0];

            try
            {
                files = Directory.GetFiles(directoryPath);
            }

            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            return files;
        }


        private static void SaveFile(string filepath, Stream fileStream)
        {
            //Create the Directory If it doesn't exists
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));

            if (!File.Exists(filepath))
            {
                FileStream OutFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                var buffer = new byte[102400];
                int BytesRead = 0;
                while ((BytesRead = fileStream.Read(buffer, 0, 10400)) > 0)
                {
                    OutFile.Write(buffer, 0, BytesRead);
                    OutFile.Flush();
                }

                long filesize = OutFile.Length;

                OutFile.Close();
                fileStream.Close();
            }
            else
            {
                throw new Exception("Skipping saving of file because filename already exists at that location");
            }
        }
    }
}