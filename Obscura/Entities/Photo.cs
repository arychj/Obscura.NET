using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Images;

namespace Obscura.Entities {
    public class Photo : Entity{
        private Image _image;

        #region accessors

        public Image Image {
            get { return _image; }
        }

        #endregion

        public Photo(int key)
            : base(key) {
            
        }
    }
}
