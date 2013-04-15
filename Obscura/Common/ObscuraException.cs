using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Obscura.Common {

    [Serializable]
    public class ObscuraException : Exception {
        private string _origin, _type, _details, _ip, _message, _stackTrace, _url;
        int _id;

        #region accessors

        public int Id {
            get { return _id; }
        }

        public string Type {
            get { return _type; }
        }

        public string Details {
            get { return _details; }
        }

        public string ClientIp {
            get { return _ip; }
        }

        public string Message {
            get { return _message; }
        }

        public string StackTrace {
            get { return _stackTrace; }
        }

        public string Origin {
            get { return _origin; }
        }

        public string Url {
            get { return _url; }
        }

        #endregion

        #region constructors

        public ObscuraException(string message, string type, string stackTrace, string details)
            : base(message) {
            _origin = Config.ApplicationName;
            _type = type;
            _message = message;
            _stackTrace = stackTrace;
            _details = details;
            _ip = ClientTools.IPAddress();
            _url = (HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.Url.AbsoluteUri);

            LogException();
        }

        public ObscuraException(string details)
            : this(details, "", "", details) { }

        public ObscuraException(Exception e)
            : this (e.Message, e.GetType().ToString(), e.StackTrace, "") { }

        public ObscuraException(Exception e, string details)
            : this (e.Message, e.GetType().ToString(), e.StackTrace, details) { }

        /// <summary>
        /// Set the custom properties for the Exception
        /// </summary>
        /// <param name="info">Seralization Info</param>
        /// <param name="context">Streaming Congext</param>
        protected ObscuraException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
            if (info != null) {
                _id = info.GetInt32("intId");
                _origin = info.GetString("txtOrigin");
                _type = info.GetString("txtType");
                _details = info.GetString("txtDetails");
                _ip = info.GetString("txtIp");
                _message = info.GetString("txtMessage");
                _stackTrace = info.GetString("txtStackTrace");
                _url = info.GetString("txtUrl");
            }
        }

        #endregion

        /// <summary>
        /// Serialized data handler
        /// </summary>
        /// <param name="info">Seralization Info</param>
        /// <param name="context">Streaming Congext</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);

            if (info != null) {
                info.AddValue("intId", _id);
                info.AddValue("txtOrigin", _origin);
                info.AddValue("txtType", _type);
                info.AddValue("txtDetails", _details);
                info.AddValue("txtIp", _ip);
                info.AddValue("txtMessage", _message);
                info.AddValue("txtStackTrace", _stackTrace);
                info.AddValue("txtUrl", _url);
            }
        }

        /// <summary>
        /// Logs the exception to the database
        /// </summary>
        public void LogException() {
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                int? id = null;
                string resultcode = null;

                db.xspWriteException(ref id, _origin, _type, _message, _details, _stackTrace, _url, _ip, ref resultcode);

                _id = (int)id;
            }
        }
    }
}
