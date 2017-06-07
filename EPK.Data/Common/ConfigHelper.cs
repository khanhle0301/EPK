using System.Configuration;

namespace EPK.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetByKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}