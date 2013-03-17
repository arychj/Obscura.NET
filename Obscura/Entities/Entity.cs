using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Shared;

namespace Obscura.Entities {
    public class Entity {
        private EntityKey _key;
        private DateTimeSet _dates;
        private bool _active;
        private string _title, _description;
        private int _hitcount;

        #region accessors

        public bool Active {
            get { return _active; }
            set {
                //TODO: set active
                _active = value;
            }
        }

        public int HitCount {
            get { return _hitcount; }
        }

        public string Title {
            get { return _title; }
            set {
                //TODO: set
                _title = value;
            }
        }

        public string Description {
            get { return _description; }
            set {
                //TODO: set
                _description = value;
            }
        }

        public DateTimeSet Dates {
            get { return _dates; }
        }

        #endregion

        internal Entity(EntityKey key) {
            _key = key;
            //TODO: create dates object
        }

        public void Hit() {
            //TODO: update hit count
        }

        internal Entity Create() {
            //TODO: create
            return null;
        }
    }
}