using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using Obscura.Common;

namespace Obscura.Web {

    /// <summary>
    /// Manages Ajax requests between the client JS and the server .NET
    /// Note: This class assumes the existence of a variable named 'handler' in the page request
    /// </summary>
    public class AjaxManager {
        /// <summary>
        /// Default handler delagate for construction new Ajax handlers
        /// </summary>
        /// <param name="request">The HTTP Request object from the ajax request</param>
        /// <param name="dom">A handle to the XML DOM to be be returned</param>
        public delegate void Handler(HttpRequest request, XmlDocument dom);

        private Dictionary<string, Handler> _handlers;
        private Page _page;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">The caller's Page object</param>
        public AjaxManager(Page page) : this(page, new Dictionary<string, Handler>()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">The caller's Page object</param>
        /// <param name="handlers">The request handlers</param>
        public AjaxManager(Page page, Dictionary<string, Handler> handlers) {
            _page = page;
            _handlers = handlers;
        }

        /// <summary>
        /// Sends the handler's response to the client
        /// </summary>
        public void SendResponse(){
            _page.Response.Clear();
            SetContentType(_page.Response);
            _page.Response.Write(GetResponse());
        }

        /// <summary>
        /// Gets the response fromt the requested handler
        /// </summary>
        /// <returns>A string representation of the XML response</returns>
        public string GetResponse() {
            XmlDocument document = new XmlDocument();
            XmlElement root;
            string handlerId = _page.Request["handler"];

            document.AppendChild(document.CreateNode(XmlNodeType.XmlDeclaration, "", ""));
            document.AppendChild((root = document.CreateElement("", "response", "")));

            if (handlerId != null && handlerId.Length > 0 && _handlers.ContainsKey(handlerId)) {
                Handler handler = _handlers[handlerId];

                try {
                    handler(_page.Request, document);
                }
                catch (Exception e) {
                    LogError(document, e, null, _page.Request);
                }
            }
            else {
                LogError(document, null, "Handler '" + handlerId + "' not found.", _page.Request);
            }

            return document.OuterXml;
        }

        /// <summary>
        /// Sets the reponse's content type to handle XML
        /// </summary>
        /// <param name="response">The page response</param>
        private void SetContentType(HttpResponse response) {
            response.ContentType = "text/xml";
        }

        /// <summary>
        /// Logs an error to file and to the client
        /// </summary>
        /// <param name="document">The document to place the reponse in</param>
        /// <param name="errorClient">The error to send to the client</param>
        /// <param name="errorLog">The detailed error to log to file</param>
        /// <param name="request">The current http request</param>
        private void LogError(XmlDocument document, Exception e, string message, HttpRequest request) {
            XmlElement error = document.CreateElement("error");
            error.InnerText = (message == null ? e.Message : message);
            document.DocumentElement.AppendChild(error);

            ObscuraException oex;
            if(e == null)
                 oex = new ObscuraException(e);
            else
                oex = new ObscuraException(message);

        }
    }
}
