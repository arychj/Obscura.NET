using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class Dimensions {
        private int _width, _height;

        public int Width {
            get { return _width; }
        }

        public int Height {
            get { return _height; }
        }

        internal Dimensions() { }

        internal Dimensions(int width, int height) {
            _width = width;
            _height = height;
        }
    }
}
