using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Images;

namespace Obscura.Entities {
    public class Collection : Entity {
        private Image _thumbnail;
        private EntityCollection<Entity> _entities;

        #region accessors

        public Image Thumbnail {
            get { return _thumbnail; }
        }

        public EntityCollection<Entity> Entities {
            get { return _entities; }
        }

        #endregion

        public Collection(EntityKey key)
            : base(key) {

        }

        public static Collection Create() {
            //TODO: Create
            return null;
        }
    }
}
