using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using Obscura.Entities;

namespace Site {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Image image = Image.Create(@"C:\Users\eolson\Desktop\images\alpha.jpg");
            Response.Clear();
            Response.Write(image.Id);
        }
    }
}