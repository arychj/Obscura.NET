using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura {
    internal class Settings {

        internal static string GetSetting(int id) {
            return GetSetting(id, null);
        }

        internal static string GetSetting(string name) {
            return GetSetting(null, name);
        }

        private static string GetSetting(int? id, string name) {
            string value = null, resultcode = null;
            bool? tfEncrypted = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspGetSetting(ref id, ref name, ref value, ref tfEncrypted, ref resultcode);
            }

            if(resultcode != "SUCCESS")
                throw new ObscuraException(string.Format("Unable to retrieve Setting ID {0}/{1}. ({2})", id, name, resultcode));

            //TODO: Setting Encryption

            return value;
        }

        internal static void UpdateSetting(string name, string value, bool isEncrypted) {
            UpdateSetting(null, name, value, isEncrypted);
        }

        internal static void UpdateSetting(int? id, string name, string value, bool isEncrypted) {
            string resultcode = null;

            //TODO: encrypt setting

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateSetting(ref id, name, value, isEncrypted, ref resultcode);
            }

            if (resultcode != "SUCCESS")
                throw new ObscuraException(string.Format("Unable to update Setting {0}/{1}. ({2})", id, name, resultcode));
        }
    }
}
