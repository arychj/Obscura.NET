using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class Collection : Entity {
        private Image _thumbnail;
        private EntityCollection _entities;

        #region accessors

        public Image Thumbnail {
            get { return _thumbnail; }
        }

        public EntityCollection Entities {
            get { return _entities; }
        }

        #endregion

        public Collection(int id)
            : base(id) {

        }

        public static Collection Create() {
            //TODO: Create
            return null;
        }
    }
}
