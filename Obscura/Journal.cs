using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Entities;
using Obscura.Shared;

namespace Obscura {
    public class Journal {
        private Entity _entity;
        private DateTimeSet _dates;
        private string _title, _body;

        #region accessors

        public Entity Entity {
            get { return _entity; }
        }

        public string Title {
            get { return _title; }
            set {
                Update(value, null);
                _title = value;
            }
        }

        public string Body {
            get { return _body; }
            set {
                Update(null, value);
                _body = value;
            }
        }

        public DateTimeSet Dates {
            get { return _dates; }
        }

        #endregion

        public Journal(Entity entity) {
            //_entity = new Entity();
        }

        private void Update(string title, string body) {
            //TODO: update journal
        }

        public static Journal Create(Entity entity, string title, string body) {
            //TODO: create journal
            return null;
        }
    }
}
