using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Common {
    public class DataTools {

        /// <summary>
        /// Flattens a Dictionary into a format suitable for posting
        /// </summary>
        /// <param name="dict">The Dictionary to flatten</param>
        /// <returns>The flattened string form of the Dictionary</returns>
        public static string FlattenDictionary(Dictionary<string, string> dict) {
            string flat = "";

            if (dict != null) {
                foreach (KeyValuePair<string, string> kvp in dict)
                    flat += string.Format("{0}={1}&", kvp.Key, System.Web.HttpUtility.UrlEncode(kvp.Value));

                flat = flat.Trim('&');
            }

            return flat;
        }

        /// <summary>
        /// Builds a string using the suplied variables
        /// </summary>
        /// <param name="s">The string to build</param>
        /// <param name="vars">Variables to compile into the template</param>
        /// <returns>The compiled template</returns>
        public static string BuildString(string s, Dictionary<string, string> vars) {
            if (s != null) {
                foreach (KeyValuePair<string, string> var in vars) {
                    s = BuildString(s, var.Key, var.Value);
                }
            }

            return s;
        }

        /// <summary>
        /// Builds a string using the suplied variables
        /// </summary>
        /// <param name="ss">The strings to build</param>
        /// <param name="vars">Variables to compile into the template</param>
        public static IEnumerable<KeyValuePair<string, string>> BuildString(IEnumerable<KeyValuePair<string, string>> ss, Dictionary<string, string> vars) {
            Dictionary<string, string> t = (Dictionary<string, string>)ss;
            Dictionary<string, string> n = new Dictionary<string, string>();

            if (ss.Count() > 0) {
                foreach (KeyValuePair<string, string> e in t)
                    n.Add(e.Key, BuildString(e.Value, vars));
            }

            return n;
        }

        /// <summary>
        /// Builds a string using the suplied variables
        /// </summary>
        /// <param name="ss">The strings to build</param>
        /// <param name="var">Variable name to compile into the template</param>
        /// <param name="val">Value of the variable to compile into the template</param>
        /// <returns>The compiled templates</returns>
        public static IEnumerable<KeyValuePair<string, string>> BuildString(IEnumerable<KeyValuePair<string, string>> ss, string var, string val) {
            Dictionary<string, string> t = (Dictionary<string, string>)ss;
            Dictionary<string, string> n = new Dictionary<string, string>();

            if (ss.Count() > 0) {
                foreach (KeyValuePair<string, string> e in t)
                    n.Add(e.Key, BuildString(e.Value, var, val));
            }

            return n;
        }

        /// <summary>
        /// Builds a string using the suplied variables
        /// </summary>
        /// <param name="s">The string to build</param>
        /// <param name="var">Variable name to compile into the template</param>
        /// <param name="val">Value of the variable to compile into the template</param>
        /// <returns>The compiled template</returns>
        public static string BuildString(string s, string var, string val) {
            if (s != null && var != null) {
                if (val == null)
                    val = "";

                s = s.Replace("{" + var + "}", val);
            }

            return s;
        }
    }
}
