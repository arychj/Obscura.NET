using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura {
    internal class Settings {
        private string _connectionString;
        private Dictionary<string, string> _settings;

        internal Settings() {
            string _connectionstring = ConfigurationManager.AppSettings["ObscuraConnectionString"];
        }

        internal Settings(string connectionString) {
            _connectionString = connectionString;
        }

        internal string GetSetting(string key) {
            return null;
        }

        private void Load() {
            _settings = new Dictionary<string, string>();
            //TODO: load()
        }
    }
}
