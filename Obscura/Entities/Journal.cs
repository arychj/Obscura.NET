using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Shared;

namespace Obscura.Entities {
    public class Journal : Entity {
        private string _body;

        #region accessors

        public Entity Entity {
            get { return _entity; }
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

        public Journal(string key) : base(key) {
            
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
