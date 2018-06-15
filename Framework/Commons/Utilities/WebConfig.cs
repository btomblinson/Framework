using System;
using System.Configuration;
using Framework.Commons.Logging;

namespace Framework.Commons.Utilities
{
    /// <summary>Used to get a key from web.config and set a default value if key does not exist.</summary>
    public static class WebConfig
    {
        /// <summary>Get a web.config key's value</summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key of desired value</param>
        /// <param name="defaultValue">Default value is key is not present</param>
        public static T Get<T>(string key, T defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrWhiteSpace(setting))
            {
                try
                {
                    return (T) Convert.ChangeType(setting, typeof(T));
                    // return (T)(object)setting;
                }
                catch (Exception)
                {
                    Logger.LogError("There was an error when trying to cast AppSettings Key \"" + key +
                                    "\",  using the default instead");
                    return defaultValue;
                }
            }

            Logger.LogError("AppSettings Key \"" + key + "\" was not found using the default instead");
            return defaultValue;
        }
    }
}