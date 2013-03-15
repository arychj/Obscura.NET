using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Images {
    public class Resolution {
        private int _x, _y;

        public int X {
            get { return _x; }
        }

        public int Y {
            get { return _y; }
        }

        internal Resolution(int x, int y) {
            _x = x;
            _y = y;
        }
    }
}
