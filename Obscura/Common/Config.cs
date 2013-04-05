using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Common {
    internal class Config {
        private const string CONNECTION_STRING_KEY = "ObscuraConnectionString";

        internal static string ConnectionString {
            get { return ConfigurationManager.ConnectionStrings[CONNECTION_STRING_KEY].ToString(); }
        }

        /// <summary>
        /// Gets an setting from the web.config
        /// </summary>
        /// <param name="key">the key to get</param>
        /// <returns>the setting</returns>
        internal static string GetSetting(string key) {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
