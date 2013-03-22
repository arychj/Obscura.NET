﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class EntityCollection<ET> where ET: Entity {
        private Dictionary<int, ET> _entities;

        #region operator overloads

        public ET this[int key] {
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
            _entities = new Dictionary<int, ET>();
        }

        public EntityCollection(string id) {

        }

        public bool ContainsId(int id) {
            return _entities.ContainsKey(id);
        }

        public void Add(int id, ET entity) {
            _entities.Add(id, entity);
            //TODO: save relation to database
        }

        public static EntityCollection<ET> GetEntityCollection(string id) {
            //TODO: GetEnityCollection()
            return null;
        }
    }
}
