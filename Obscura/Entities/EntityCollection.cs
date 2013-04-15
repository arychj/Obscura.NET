using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// A collection of Entities belonging to another Entity
    /// </summary>
    public class EntityCollection<T> : IEnumerable where T : Entity {
        private Entity _entity;
        private List<T> _members;
        private Type _memberType;

        #region operator overloads

        /// <summary>
        /// Returns the Entity with id if it is in the collection
        /// </summary>
        /// <param name="id">the id to get</param>
        /// <returns>the entity or sub-entity, or null if not found</returns>
        public T this[int id] {
            get { return (T)_members.Select(m => m.Id == id); }
        }

        #endregion

        #region accessors

        /// <summary>
        /// Returns a list of all members in the collections
        /// </summary>
        public List<T> Members {
            get { return _members.Cast<T>().ToList(); }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">the Entity to retrieve the collection for</param>
        internal EntityCollection(Entity entity) {
            _entity = entity;
            _memberType = typeof(T);

            string resultcode = null;
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                ISingleResult<xspGetEntityMembersResult> members = db.xspGetEntityMembers(_entity.Id, ref resultcode);

                if (resultcode == "SUCCESS") {
                    _members = new List<T>();
                    foreach (xspGetEntityMembersResult member in members)
                        _members.Add(GetMember((int)member.id_member));
                }
                else
                    throw new ObscuraException(string.Format("Error retrieving EntityCollection for Entity ID {0}. ({1})", _entity.Id, resultcode));
            }
        }

        /// <summary>
        /// Constructor
        /// Retrieves all Entites of the specified EntityType
        /// </summary>
        /// <param name="type">the type of entities to retrieve</param>
        internal EntityCollection(){
            _entity = null;
            _memberType = typeof(T);

            string type = _memberType.ToString();
            type = type.Substring(type.LastIndexOf('.') + 1);

            string resultcode = null;
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                ISingleResult<xspGetEntitiesResult> members = db.xspGetEntities(type, ref resultcode);

                if (resultcode == "SUCCESS") {
                    _members = new List<T>();
                    foreach (xspGetEntitiesResult member in members)
                        _members.Add(GetMember((int)member.id));
                }
                else
                    throw new ObscuraException(string.Format("Error retrieving null EntityCollection for EntityType {0}. ({1})", type, resultcode));
            }
        }

        /// <summary>
        /// Checks if the collection contains an member with the specified id
        /// </summary>
        /// <param name="id">the id to check for</param>
        /// <returns>true if the collection contains a memeber with id, false otherwise</returns>
        public bool ContainsId(int id) {
            return Contains(GetMember(id));
        }

        /// <summary>
        /// Checks if the collection contains the specified member
        /// </summary>
        /// <param name="member">the member to check for</param>
        /// <returns>true if the collection contains the memeber, false otherwise</returns>
        public bool Contains(T member) {
            return _members.Contains(member);
        }

        /// <summary>
        /// Adds a member to the EntityCollection
        /// </summary>
        /// <param name="member">The member to add</param>
        public void Add(T member) {
            if (_entity != null) {
                int? id = -1;
                string resultcode = null;

                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    db.xspUpdateEntityMember(ref id, _entity.Id, member.Id, ref resultcode);
                }

                if (resultcode == "SUCCESS" || resultcode == "EXISTS") {
                    if (!_members.Contains(member))
                        _members.Add(member);
                }
                else
                    throw new ObscuraException(string.Format("Unable to add member with Entity ID {0} to EntityCollection. ({1})", member.Id, resultcode));
            }
            else
                throw new ObscuraException("Cannot add an Entity to the null collection.");

        }

        /// <summary>
        /// Removes an member from the EntityCollection
        /// </summary>
        /// <param name="id">the id of the member to remove</param>
        public void Remove(int id) {
            Remove(GetMember(id));
        }

        /// <summary>
        /// Removes a member from the EntityCollection
        /// </summary>
        /// <param name="member">the member to remove</param>
        public void Remove(T member) {
            if (_entity != null) {
                string resultcode = null;

                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    db.xspDeleteEntityMember(_entity.Id, member.Id, ref resultcode);
                }

                if (resultcode == "SUCCESS")
                    _members.Remove(member);
                else
                    throw new ObscuraException(string.Format("Unable to remove member with Entity ID {0} from EntityCollection. ({1})", member.Id, resultcode));
            }
            else
                throw new ObscuraException("Cannot remove an Entity from the null collection.");
        }

        /// <summary>
        /// Gets the enumerator for this EntityCollection
        /// </summary>
        /// <returns>the enumerator</returns>
        public IEnumerator GetEnumerator() {
            foreach (Entity member in _members)
                yield return (T)member;
        }

        /// <summary>
        /// Gets an enumerator for a sub-section of this EntityCollection
        /// </summary>
        /// <param name="start">the index of the Member to start at</param>
        /// <param name="size">the maximum number of members to return</param>
        /// <returns>the enumerator</returns>
        public IEnumerator GetEnumerator(int start, int size) {
            for (int i = start; i < start + size && i < _members.Count; i++)
                yield return (T)_members.ElementAt(i);
        }

        /// <summary>
        /// An XML representation of this EntityCollection
        /// </summary>
        /// <returns>An XML document</returns>
        public XmlDocument ToXml() {
            XmlDocument dom = new XmlDocument();
            XmlElement xCollection = (XmlElement)dom.AppendChild(dom.CreateElement("EntityCollection"));

            if(_entity != null)
                xCollection.SetAttribute("id", _entity.Id.ToString());

            T entity;
            XmlElement xEntity;
            IEnumerator enumerator = GetEnumerator();
            while (enumerator.MoveNext()) {
                entity = ((T)enumerator.Current);
                xEntity = (XmlElement)xCollection.AppendChild(dom.CreateElement(entity.Type.ToString()));
                xEntity.SetAttribute("id", entity.Id.ToString());
                xEntity.AppendChild(dom.CreateElement("title")).InnerText = entity.Title.ToString();
                xEntity.AppendChild(dom.CreateElement("url")).InnerText = entity.Url.ToString();
            }

            return dom;
        }

        /// <summary>
        /// An XML representation of this EntityCollection
        /// </summary>
        /// <returns>An XML document</returns>
        public XmlDocument ToXml(int start, int size) {
            XmlDocument dom = new XmlDocument();
            XmlElement xCollection = (XmlElement)dom.AppendChild(dom.CreateElement("EntityCollection"));

            if (_entity != null)
                xCollection.SetAttribute("id", _entity.Id.ToString());

            T entity;
            XmlElement xEntity;
            IEnumerator enumerator = GetEnumerator(start, size);
            while (enumerator.MoveNext()) {
                entity = ((T)enumerator.Current);
                xEntity = (XmlElement)xCollection.AppendChild(dom.CreateElement(entity.Type.ToString()));
                xEntity.SetAttribute("id", entity.Id.ToString());
                xEntity.AppendChild(dom.CreateElement("title")).InnerText = entity.Title.ToString();
                xEntity.AppendChild(dom.CreateElement("url")).InnerText = entity.Url.ToString();
            }

            return dom;
        }

        /// <summary>
        /// Gets a new instance of the member type with the specified id
        /// </summary>
        /// <param name="id">the id of the member to get</param>
        /// <returns>the new member instance</returns>
        private T GetMember(int id) {
            return (T)Activator.CreateInstance(_memberType, (BindingFlags.NonPublic | BindingFlags.Instance), null, new object[] { id }, null);
        }

        /// <summary>
        /// Retrieves the specified EntityCollection
        /// </summary>
        /// <param name="entity">the Entity to retrieve the collection for</param>
        /// <returns>the parent's EntityCollection</returns>
        public static EntityCollection<T> Retrieve(Entity entity) {
            return new EntityCollection<T>(entity);
        }

        /// <summary>
        /// Retrieves the specified EntityCollection
        /// </summary>
        /// <param name="entityid">the id of the Entity to retrieve the collection for</param>
        /// <returns>the parent's EntityCollection</returns>
        public static EntityCollection<T> Retrieve(int entityid) {
            return new EntityCollection<T>(new Entity(entityid));
        }
    }
}
