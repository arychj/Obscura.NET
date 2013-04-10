using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {

    /// <summary>
    /// 
    /// </summary>
    public class EntityCollection {
        private Entity _parent;
        private Dictionary<int, Entity> _entities;

        #region operator overloads

        public Entity this[int id] {
            get { return _entities[id]; }
            set {
                if (_entities.ContainsKey(id))
                    _entities[id] = value;
                else
                    Add(id, value);
            }
        }

        #endregion

        public EntityCollection(Entity parent){
            _parent = parent;
            _entities = GetMemberEntities(parent);
        }

        public EntityCollection(int id) {

        }

        public bool ContainsId(int id) {
            return _entities.ContainsKey(id);
        }

        public void Add(int id, Entity entity) {
            _entities.Add(id, entity);
            //TODO: save relation to database
        }

        private Dictionary<int, Entity> GetMemberEntities(Entity parent) {
            return null;
        }

        public static EntityCollection GetEntityCollection(Entity entity) {
            return GetEntityCollection(entity.Id);
        }

        public static EntityCollection GetEntityCollection(int id) {
            //TODO: GetEnityCollection()
            return null;
        }
    }
}
