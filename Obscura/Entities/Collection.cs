using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class Collection : Entity {
        private EntityCollection<Entity> _entities;

        public Collection(string key) : base(key) {

        }
    }
}
