using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class Album : Entity {
        private List<Photo> _photos;

        #region accessors

        public List<Photo> Photos {
            get { return _photos; }
        }

        #endregion

        public Album(string id)
            : base(id) {

        }
    }
}
