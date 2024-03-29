﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// The base class for all Entities
    /// </summary>
    public class Entity {
        private bool _loaded = false;

        private int _id, _typeid;
        private EntityType _type;
        private DateTimeSet _dates;
        private bool _active;
        private string _title, _description;
        private int _hitcount;
        private Url _url = null;
        private TagCollection _tags;

        #region accessors

        /// <summary>
        /// The unique ID associated with this Wntity
        /// </summary>
        public int Id {
            get { return _id; }
        }

        /// <summary>
        /// Is this Entity active?
        /// </summary>
        public bool IsActive {
            get { 
                Load(); 
                return _active; }
            set {
                Load(); 
                Update(value, null, null);
            }
        }

        /// <summary>
        /// This Entity's type
        /// </summary>
        public EntityType Type {
            get {
                Load(); 
                return _type;
            }
        }

        /// <summary>
        /// The ID of this Entity's EntityType
        /// </summary>
        internal int TypeId {
            get {
                Load(); 
                return _typeid;
            }
        }

        /// <summary>
        /// The number of times this Entity has been accesses
        /// </summary>
        public int HitCount {
            get {
                Load(); 
                return _hitcount;
            }
        }

        /// <summary>
        /// This Entity's title
        /// </summary>
        public string Title {
            get { 
                Load(); 
                return _title; 
            }
            set {
                Load();
                Update(null, value, null);
            }
        }

        /// <summary>
        /// This Entity's description
        /// </summary>
        public string Description {
            get { 
                Load(); 
                return _description;
            }
            set {
                Load();
                Update(null, null, value);
            }
        }

        /// <summary>
        /// The URL to this Entity
        /// </summary>
        public Url Url {
            get {
                Load();
                if (_url == null)
                    _url = new Url(this);

                return _url;
            }
        }

        /// <summary>
        /// Important dates associated with this Entity
        /// </summary>
        public DateTimeSet Dates {
            get {
                Load();
                return _dates; 
            }
        }

        /// <summary>
        /// This Entity's TagCollections
        /// </summary>
        public TagCollection Tags {
            get {
                Load();
                return _tags;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// Retrieves an Entity from the database
        /// </summary>
        /// <param name="id">the id of the Entity</param>
        internal Entity(int id) : this(id, false) { }

        /// <summary>
        /// Constructor
        /// Retrieves an Entity from the database
        /// </summary>
        /// <param name="id">the id of the Entity</param>
        /// <param name="loadImmediately">Load the Entity's contents immediately</param>
        internal Entity(int id, bool loadImmediately) {
            _id = id;

            if (loadImmediately)
                Load();
        }

        /// <summary>
        /// Constructor
        /// Builds an Entity using the specified entity
        /// </summary>
        /// <param name="entity">the entity to use</param>
        internal Entity(Entity entity) {
            _id = entity.Id;
            _type = entity.Type;
            _title = entity.Title;
            _description = entity.Description;
            _hitcount = entity.HitCount;
            _dates = entity.Dates;
            _active = entity.IsActive;
        }

        /// <summary>
        /// Constructor
        /// Builds an Entity using the specified parameters
        /// </summary>
        /// <param name="id">the ID of the Entity</param>
        /// <param name="typeid">the ID of the Entity's type</param>
        /// <param name="type">the type of the Entity</param>
        /// <param name="title">the title of the Entity</param>
        /// <param name="description">the description of the Entity</param>
        /// <param name="hitcount">the number of hits on this Entity</param>
        /// <param name="dates">the important dates associated with this Entity</param>
        /// <param name="active">the Entity's status</param>
        internal Entity(int id, int typeid, EntityType type, string title, string description, int hitcount, DateTimeSet dates, bool active) {
            _id = id;
            _typeid = typeid;
            _type = type;
            _title = title;
            _description = description;
            _hitcount = hitcount;
            _dates = dates;
            _active = active;

            _tags = new TagCollection(this);
        }

        /// <summary>
        /// Increment this Entity's hit counter
        /// </summary>
        public void Hit() {
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspLogHit(_id);
            }

            _hitcount++;
        }

        /// <summary>
        /// Update this Entity
        /// Specify null for any parameter to not update
        /// </summary>
        /// <param name="active">the new active status</param>
        /// <param name="title">the new title</param>
        /// <param name="description">the new description</param>
        public void Update(bool? active, string title, string description) {
            if (active != null || title != null || description != null) {
                int? id = _id, typeid = null;
                string resultcode = null;

                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    db.xspUpdateEntity(ref id, ref typeid, null, title, description, active, ref resultcode);
                }

                if (resultcode == "SUCCESS") {
                    _dates.Modified = DateTime.Now;

                    if (title != null)
                        _title = title;
                    if (description != null)
                        _description = description;
                    if (active != null)
                        _active = (bool)active;
                }
                else
                    throw new ObscuraException(string.Format("Unable to update Entity ID {0}. ({1})", _id, resultcode));
            }
        }

        /// <summary>
        /// Deletes this Entity and removes it from the database
        /// </summary>
        public void Delete() {
            string resultcode = null;
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspDeleteEntity(_id, ref resultcode);
            }

            if (resultcode != "SUCCESS")
                throw new ObscuraException(string.Format("Unable to delete Entity ID {0}", _id));
        }

        /// <summary>
        /// An XML representation of this Entity
        /// </summary>
        /// <returns>An XML document</returns>
        public XmlDocument ToXml() {
            XmlDocument dom = new XmlDocument();
            XmlElement xEntity = dom.CreateElement("Entity");
            dom.AppendChild(xEntity);

            xEntity.SetAttribute("id", Id.ToString());
            xEntity.SetAttribute("type", Type.ToString());
            xEntity.SetAttribute("active", IsActive.ToString());
            xEntity.AppendChild(dom.CreateElement("title")).AppendChild(dom.CreateCDataSection(Title));
            xEntity.AppendChild(dom.CreateElement("description")).AppendChild(dom.CreateCDataSection(Description));
            xEntity.AppendChild(dom.CreateElement("url")).InnerText = Url.ToString();
            xEntity.AppendChild(dom.CreateElement("hitcount")).InnerText = HitCount.ToString();
            xEntity.AppendChild(dom.CreateElement("datecreated")).InnerText = Dates.Created.ToString();
            xEntity.AppendChild(dom.CreateElement("datemodified")).InnerText = Dates.Modified.ToString();

            XmlElement xTags = (XmlElement)xEntity.AppendChild(dom.CreateElement("tags"));
            foreach (string tag in Tags)
                xTags.AppendChild(dom.CreateElement("tag")).InnerText = tag;

            return dom;
        }

        /// <summary>
        /// Loads the Entity's details
        /// </summary>
        private void Load() {
            if (!_loaded) {
                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    int? typeid = null, hitcount = null;
                    string type = null, title = null, description = null, resultcode = null;
                    DateTime? dtCreated = null, dtModified = null;
                    bool? tfActive = null;

                    ISingleResult<xspGetEntityResult> results = db.xspGetEntity(_id, ref typeid, ref type, ref title, ref description, ref hitcount, ref dtCreated, ref dtModified, ref tfActive, ref resultcode);

                    if (resultcode == "SUCCESS") {
                        _typeid = (typeid == null ? 0 : (int)typeid);
                        _type = (EntityType)Enum.Parse(typeof(EntityType), type);
                        _title = title;
                        _description = description;
                        _hitcount = (hitcount == null ? 0 : (int)hitcount);
                        _dates = new DateTimeSet((DateTime)dtCreated, (DateTime)dtModified);
                        _active = (tfActive == null ? false : (bool)tfActive);

                        List<string> tags = new List<string>();
                        foreach (xspGetEntityResult result in results)
                            tags.Add(result.Tag);

                        _tags = new TagCollection(this, tags);
                    }
                    else
                        throw new ObscuraException(string.Format("Entity ID {0} does not exist. ({1})", _id, resultcode));
                }

                _loaded = true;
            }
        }
        
        /// <summary>
        /// Creates a new Entity
        /// </summary>
        /// <param name="type">the type of Entity</param>
        /// <param name="title">the title of the Entity</param>
        /// <param name="description">the description of the Entity</param>
        /// <returns>the new Entity object</returns>
        internal static Entity Create(EntityType type, string title, string description) {
            Entity entity = null;
            int? id = -1, typeid = null;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateEntity(ref id, ref typeid, type.ToString(), title, description, true, ref resultcode);
            }

            if (resultcode == "SUCCESS") {
                entity = new Entity(
                            (int)id, 
                            (int)typeid, 
                            type, 
                            title, 
                            description, 
                            0, 
                            new DateTimeSet(DateTime.Now, DateTime.Now), 
                            true
                        );
            }
            else
                throw new ObscuraException(string.Format("Unable to create Entity. ({0})", resultcode));

            return entity;
        }

        /// <summary>
        /// Retrieves the specified Entity
        /// </summary>
        /// <param name="id">the id of the Entity to retrieve</param>
        /// <returns>the Entity</returns>
        public static Entity Retrieve(int id) {
            return new Entity(id, true);
        }

        /// <summary>
        /// Retrieves all Entities from the database
        /// </summary>
        /// <returns>An EntityCollection containing all Entities</returns>
        public static EntityCollection<Entity> All() {
            return new EntityCollection<Entity>();
        }

        #region equality

        public static bool operator ==(Entity a, Entity b) {
            if (System.Object.ReferenceEquals(a, b))
                return true;
            else if (((object)a == null) || ((object)b == null))
                return false;
            else
                return a.Id == b.Id;
        }

        public static bool operator !=(Entity a, Entity b) {
            return !(a == b);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            Entity e = obj as Entity;
            if (e == null)
                return false;

            return _id == e.Id;
        }

        public bool Equals(Entity e) {
            return _id == e.Id;
        }

        #endregion
    }
}