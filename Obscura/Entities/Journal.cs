﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {
    public class Journal : Entity {
        private string _body;

        #region accessors

        public string Body {
            get { return _body; }
            set {
                Update(null, value);
                _body = value;
            }
        }

        #endregion

        public Journal(int id)
            : base(id) {
            
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
