using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Common {
    public class DateTimeSet {
        private DateTime _created, _modified;

        #region accessors

        public DateTime Created {
            get { return _created; }
        }

        public DateTime Modified {
            get { return _modified; }
        }

        #endregion

        internal DateTimeSet(DateTime created, DateTime modified) {
            _created = created;
            _modified = modified;
        }
    }
}
