using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        /// <summary>
        /// The body text of the Journal
        /// </summary>
        public string Body {
            get {
                Load();
                return _body;
            }
            set {
                Load();
                Update(null, value);
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// Retrieves an Journal
        /// </summary>
        /// <param name="id">the id of the Journal</param>
        internal Journal(int id) : base(id, false) { }

        /// <summary>
        /// Constructor
        /// Retrieves an Journal
        /// </summary>
        /// <param name="id">the id of the Journal</param>
        /// <param name="loadImmediately">Load the Journal's contents immediately</param>
        internal Journal(int id, bool loadImmediately)
            : base(id, loadImmediately){
            if(loadImmediately)
                Load();
        }

        /// <summary>
        /// Constructor
        /// Builds an Journal using the specified parameters
        /// </summary>
        /// <param name="entity">the base Entity of this Journal</param>
        /// <param name="cover">the cover Image associated with this Journal</param>
        /// <param name="body">the body text associated with this Journal</param>
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

        /// <summary>
        /// An XML representation of this Journal
        /// </summary>
        /// <returns>An XML document</returns>
        new public XmlDocument ToXml() {
            XmlDocument dom = base.ToXml();
            XmlElement xJournal = (XmlElement)dom.DocumentElement.AppendChild(dom.CreateElement(Type.ToString()));

            XmlElement xCover = (XmlElement)xJournal.AppendChild(dom.CreateElement("cover"));
            xCover.SetAttribute("id", Cover.Id.ToString());
            xCover.AppendChild(dom.CreateElement("url")).InnerText = Cover.Url.ToString();

            xJournal.AppendChild(dom.CreateCDataSection("body")).AppendChild(dom.CreateCDataSection(Body));

            return dom;
        }

        /// <summary>
        /// Loads the Journal's details
        /// </summary>
        private void Load() {
            if (!_loaded) {
                string resultcode = null;
                int? coverid = null;
                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    db.xspGetJournal(base.Id, ref coverid, ref _body, ref resultcode);

                    if (resultcode == "SUCCESS") {
                        _cover = new Image((int)coverid);
                        _body = Body;
                    }
                    else
                        throw new ObscuraException(string.Format("Journal Entity Id {0} does not exist. ({1})", base.Id, resultcode));

                }

                _loaded = true;
            }
        }

        /// <summary>
        /// Creates a new Journal
        /// </summary>
        /// <param name="cover">the id of the cover Image associated with this Journal</param>
        /// <param name="title">the title of Journal</param>
        /// <param name="description">the description of the Journal</param>
        /// <param name="body">the body text of the Journal</param>
        /// <returns>a new Journal object</returns>
        public static Journal Create(Image cover, string title, string description, string body) {
            Journal journal = null;
            Entity entity;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Journal, title, description);
                db.xspUpdateJournal(
                    entity.Id,
                    (cover == null ? null : (int?)cover.Id),
                    body,
                    ref resultcode
                );

                if (resultcode == "SUCCESS") {
                    journal = new Journal(entity, cover, body);
                }
                else {
                    entity.Delete();
                    throw new ObscuraException(string.Format("Unable to create Journal. ({0})", resultcode));
                }
            }

            return journal;
        }

        /// <summary>
        /// Retrieves the specified Journal
        /// </summary>
        /// <param name="id">the id of the Journal to retrieve</param>
        /// <returns>the Journal</returns>
        new public static Journal Retrieve(int id) {
            return new Journal(id, true);
        }
        
        /// <summary>
        /// Retrieves all Journals from the database
        /// </summary>
        /// <returns>An EntityCollection containing all Journals</returns>
        public static EntityCollection<Journal> All() {
            return new EntityCollection<Journal>();
        }
    }
}
