﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// The base class for all Entities
    /// </summary>
    public class Entity {
        private int _id, _typeid;
        private EntityType _type;
        private DateTimeSet _dates;
        private bool _active;
        private string _title, _description;
        private int _hitcount;

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
            get { return _active; }
            set {
                Update(value, null, null);
                _active = value;
            }
        }

        /// <summary>
        /// This Entity's type
        /// </summary>
        public EntityType Type {
            get { return _type; }
        }

        /// <summary>
        /// The ID of this Entity's EntityType
        /// </summary>
        internal int TypeId {
            get { return _typeid; }
        }

        /// <summary>
        /// The number of times this Entity has been accesses
        /// </summary>
        public int HitCount {
            get { return _hitcount; }
        }

        /// <summary>
        /// This Entity's title
        /// </summary>
        public string Title {
            get { return _title; }
            set {
                Update(null, value, null);
                _title = value;
            }
        }

        /// <summary>
        /// This Entity's description
        /// </summary>
        public string Description {
            get { return _description; }
            set {
                Update(null, null, value);
                _description = value;
            }
        }

        /// <summary>
        /// Important dates associated with this Entity
        /// </summary>
        public DateTimeSet Dates {
            get { return _dates; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// Retrieves an Entity from the database
        /// </summary>
        /// <param name="id">the id of the Entity</param>
        public Entity(int id) {
            _id = id;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                int? typeid = null, hitcount = null;
                string type = null, title = null, description = null, resultcode = null;
                DateTime? dtCreated = null, dtModified = null;
                bool? tfActive = null;

                db.xspGetEntity(id, ref typeid, ref type, ref title, ref description, ref hitcount, ref dtCreated, ref dtModified, ref tfActive, ref resultcode);

                if (resultcode == "SUCCESS") {
                    _id = id;
                    _typeid = (typeid == null ? 0 : (int)typeid);
                    _type = (EntityType)Enum.Parse(typeof(EntityType), type);
                    _title = Title;
                    _description = description;
                    _hitcount = (hitcount == null ? 0 : (int)hitcount);
                    _dates = new DateTimeSet((DateTime)dtCreated, (DateTime)dtModified);
                    _active = (tfActive == null ? false : (bool)tfActive);
                }
                else
                    throw new ObscuraException(string.Format("Entity ID {0} does not exist. ({1})", id, resultcode));
            }
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
            int? id = _id, typeid = null;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateEntity(ref id, ref typeid, null, title, description, active, ref resultcode);
            }

            if (resultcode == "SUCCESS") {
                _dates.Modified = DateTime.Now;
            }
            else
                throw new ObscuraException(string.Format("Unable to update Entity ID {0}. ({1})", _id, resultcode));
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
        /// Creates a new Entity
        /// </summary>
        /// <param name="type">the type of Entity</param>
        /// <param name="title">the title of the Entity</param>
        /// <param name="description">the description of the entity</param>
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
    }
}