﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Common {

    [Serializable]
    public class ObscuraException : Exception {
        private string _origin, _type, _details, _ip, _message, _stackTrace;
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

        #endregion

        #region constructors

        public ObscuraException(string origin, string message, string type, string stackTrace, string ip, string details)
            : base(message) {
            _origin = origin;
            _type = type;
            _message = message;
            _stackTrace = stackTrace;
            _details = details;
            _ip = ip;

            LogException();
        }

        public ObscuraException(string origin, string ip, string details)
            : this(origin, "", "", "", ip, "") { }

        public ObscuraException(string origin, Exception e, string ip)
            : this (origin, e.Message, e.GetType().ToString(), e.StackTrace, ip, "") { }

        public ObscuraException(string origin, Exception e, string ip, string details)
            : this (origin, e.Message, e.GetType().ToString(), e.StackTrace, ip, details) { }

        /// <summary>
        /// Set the custom properties for the Exception
        /// </summary>
        /// <param name="info">Seralization Info</param>
        /// <param name="context">Streaming Congext</param>
        protected ObscuraException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
            if (info != null) {
                _origin = info.GetString("txtorigin");
                _type = info.GetString("txttype");
                _message = info.GetString("txtMessage");
                _stackTrace = info.GetString("txtStackTrace");
                _details = info.GetString("txtDetails");
                _ip = info.GetString("txtIP");
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
                info.AddValue("txtorigin", _origin);
                info.AddValue("txttype", _type);
                info.AddValue("txtMessage", _message);
                info.AddValue("txtStackTrace", _stackTrace);
                info.AddValue("txtDetails", _details);
                info.AddValue("txtIP", _ip);
            }
        }

        /// <summary>
        /// Logs the exception to the database
        /// </summary>
        public void LogException() {
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                int? id = null;
                string resultcode = null;

                db.xspWriteException(ref id, _origin, _type, _message, _details, _stackTrace, ClientTools.IPAddress(), ref resultcode);

                _id = (int)id;
            }
        }
    }
}
