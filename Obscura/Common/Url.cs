using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Common {
    public class Url {
        private string _pretty, _actual;

        public string Pretty {
            get { return _pretty; }
        }

        public string Actual {
            get { return _actual; }
        }

        internal Url() {
            //TODO: constructor
        }
    }
}
