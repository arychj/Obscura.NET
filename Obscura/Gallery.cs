using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Obscura.Entities;

namespace Obscura {
    public class Gallery {

        public Gallery() {
            string connectionstring = ConfigurationManager.AppSettings["ObscuraConnectionString"];
        }

        public Gallery(string connectionString) {

        }
    }
}
