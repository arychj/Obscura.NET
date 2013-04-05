using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Obscura.Common {

    public class ClientTools {

        /// <summary>
        /// Returns the client's IP Address from the active HttpContext
        /// </summary>
        /// <returns>The client's IP address</returns>
        public static string IPAddress() {
            return IPAddress(HttpContext.Current.Request);
        }

        /// <summary>
        /// Returns the client's IP Address
        /// </summary>
        /// <param name="request">The current HttpRequest object</param>
        /// <returns>The client's IP address</returns>
        public static string IPAddress(HttpRequest request) {
            return request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
