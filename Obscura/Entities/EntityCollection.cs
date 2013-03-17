using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class EntityCollection<ET> where ET: Entity {
        private Dictionary<EntityKey, ET> _entities;

        #region operator overloads

        public ET this[EntityKey key] {
            get { return _entities[key]; }
            set {
                if (_entities.ContainsKey(key))
                    _entities[key] = value;
                else
                    Add(key, value);
            }
        }

        #endregion

        public EntityCollection(){
            _entities = new Dictionary<EntityKey, ET>();
        }

        public EntityCollection(string key) {

        }

        public bool ContainsKey(EntityKey key) {
            return _entities.ContainsKey(key);
        }

        public void Add(EntityKey key, ET entity) {
            _entities.Add(key, entity);
            //TODO: save relation to database
        }

        public static EntityCollection<ET> GetEntityCollection(string key) {
            //TODO: GetEnityCollection()
            return null;
        }
    }
}
