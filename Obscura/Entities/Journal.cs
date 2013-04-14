using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {
    public class Journal : Entity {
        private bool _loaded = false;
        private Image _cover;
        private string _body;

        #region accessors

        /// <summary>
        /// The cover Image associated with this Journal
        /// </summary>
        public Image Cover {
            get {
                Load();
                return _cover;
            }
            set {
                Load();
                Update(value, null);
            }
        }

        public string Body {
            get {
                Load();
                return _body;
            }
            set {
                Update(null, value);
                _body = value;
            }
        }

        #endregion

        internal Journal(int id) : base(id, false) { }

        internal Journal(int id, bool loadImmediately)
            : base(id, loadImmediately){
            if(loadImmediately)
                Load();
        }

        internal Journal(Entity entity, Image cover, string body)
            : base(entity) {
                _cover = cover;
                _body = body;
                _loaded = true;
        }

        /// <summary>
        /// Updates this Journal
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="coverid">the id of the cover Image associated with this Journal</param>
        /// <param name="body">the ibody text of this Journal</param>
        public void Update(Image cover, string body) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateJournal(base.Id, (cover == null ? null : (int?)cover.Id), body, ref resultcode);

                if (resultcode == "SUCCESS") {
                    if (cover != null)
                        _cover = cover;
                    if (body != null)
                        _body = body;
                }
                else
                    throw new ObscuraException(string.Format("Unable to update Journal Entity ID {0}. ({1})", base.Id, resultcode));
            }
        }

        private void Load() {
            if (!_loaded) {

                _loaded = true;
            }
        }

        public static Journal Create(Entity entity, string title, string body) {
            //TODO: create journal
            return null;
        }

        new public static Journal Retrieve(int id) {
            return new Journal(id, true);
        }
    }
}
