using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// A collection of Entities belonging to another Entity
    /// </summary>
    public class EntityCollection<T> : IEnumerable where T : Entity {
        private int _parentid;
        private List<Entity> _members;
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
        /// <param name="parent">the parent of this collection</param>
        internal EntityCollection(T parent) : this(parent.Id) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentid">the id of the parent of this collection</param>
        internal EntityCollection(int parentid) {
            _parentid = parentid;
            _memberType = typeof(T);

            string resultcode = null;
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                ISingleResult<xspGetEntityMembersResult> members = db.xspGetEntityMembers(parentid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    _members = new List<Entity>();
                    foreach (xspGetEntityMembersResult member in members)
                        _members.Add(GetMember((int)member.id_member));
                }
                else
                    throw new ObscuraException(string.Format("Error retrieving EntityCollection for Entity ID {0}. ({1})", parentid, resultcode));
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
            int? id = -1;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateEntityMember(ref id, _parentid, member.Id, ref resultcode);
            }

            if(resultcode == "SUCCESS" ||resultcode == "EXISTS"){
                if(!_members.Contains(member))
                    _members.Add(member);
            }
            else
                throw new ObscuraException(string.Format("Unable to add member with Entity ID {0} to EntityCollection. ({0})", member.Id, resultcode));
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
        public void Remove(Entity member) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspDeleteEntityMember(_parentid, member.Id, ref resultcode);
            }

            if(resultcode == "SUCCESS")
                _members.Remove(member);
            else
                throw new ObscuraException(string.Format("Unable to remove member with Entity ID {0} from EntityCollection. ({0})", member.Id, resultcode));
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
        /// <param name="parent">the parent Entity to retrieve the collection for</param>
        /// <returns>the parent's EntityCollection</returns>
        public static EntityCollection<T> Retrieve(T parent) {
            return Retrieve(parent.Id);
        }

        /// <summary>
        /// Retrieves the specified EntityCollection
        /// </summary>
        /// <param name="parent">the id of the parent Entity to retrieve the collection for</param>
        /// <returns>the parent's EntityCollection</returns>
        public static EntityCollection<T> Retrieve(int parentid) {
            return new EntityCollection<T>(parentid);
        }
    }
}
