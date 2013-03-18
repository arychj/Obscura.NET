using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Images;

namespace Obscura.Entities {
    public class Album : Entity {
        private List<Photo> _photos;
        private Image _thumbnail;

        #region accessors

        public Image Thumbnail {
            get { return _thumbnail; }
        }

        public List<Photo> Photos {
            get { return _photos; }
        }

        #endregion

        public Album(int id)
            : base(id) {

        }
    }
}
