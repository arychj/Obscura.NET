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
    /// A collection of tags belonging to an Entity
    /// </summary>
    public class TagCollection : IEnumerable {
        private bool _loaded = false;
        private Entity _entity;
        private List<string> _tags;

        #region accessors

        /// <summary>
        /// Returns a list of all tags in the collections
        /// </summary>
        public List<string> Tags {
            get {
                Load(); 
                return _tags;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">the Entity</param>
        internal TagCollection(Entity entity) : this(entity, null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">the Entity</param>
        /// <param name="tags">the tags that are associated with this Entity</param>
        internal TagCollection(Entity entity, List<string> tags) {
            _entity = entity;
            _tags = tags;
            _loaded = true;
        }

        /// <summary>
        /// Checks if the collection contains the specified tag
        /// </summary>
        /// <param name="tag">the tag to check for</param>
        /// <returns>true if the collection contains the tag, false otherwise</returns>
        public bool Contains(string tag) {
            Load();
            return _tags.Contains(tag);
        }

        /// <summary>
        /// Adds a tag to the TagCollection
        /// </summary>
        /// <param name="member">The tag to add</param>
        public void Add(string tag) {
            Load();

            int? id = -1;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateEntityTag(ref id, _entity.Id, tag, ref resultcode);
            }

            if(resultcode == "SUCCESS" ||resultcode == "EXISTS"){
                if(!_tags.Contains(tag))
                    _tags.Add(tag);
            }
            else
                throw new ObscuraException(string.Format("Unable to add tag to TagCollection for Entity ID {0}. ({1})", _entity.Id, resultcode));
        }

        /// <summary>
        /// Removes a member from the EntityCollection
        /// </summary>
        /// <param name="member">the member to remove</param>
        public void Remove(string tag) {
            Load();

            string resultcode = null;
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspDeleteEntityTag(_entity.Id, tag, ref resultcode);
            }

            if(resultcode == "SUCCESS")
                _tags.Remove(tag);
            else
                throw new ObscuraException(string.Format("Unable to remove tag from TagCollection for Entity ID {0}. ({1})", _entity.Id, resultcode));
        }

        /// <summary>
        /// Gets the enumerator for this TagCollection
        /// </summary>
        /// <returns>the enumerator</returns>
        public IEnumerator GetEnumerator() {
            Load();

            foreach (string tag in _tags)
                yield return tag;
        }

        /// <summary>
        /// Gets an enumerator for a sub-section of this TagCollection
        /// </summary>
        /// <param name="start">the index of the tag to start at</param>
        /// <param name="size">the maximum number of members to return</param>
        /// <returns>the enumerator</returns>
        public IEnumerator GetEnumerator(int start, int size) {
            Load();

            for (int i = start; i < start + size && i < _tags.Count; i++)
                yield return _tags.ElementAt(i);
        }

        /// <summary>
        /// Loads the TagCollections
        /// </summary>
        private void Load() {
            if (!_loaded) {
                string resultcode = null;
                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    ISingleResult<xspGetEntityTagsResult> tags = db.xspGetEntityTags(_entity.Id, ref resultcode);

                    if (resultcode == "SUCCESS") {
                        _tags = new List<string>();
                        foreach (xspGetEntityTagsResult tag in tags)
                            _tags.Add(tag.Tag);
                    }
                    else
                        throw new ObscuraException(string.Format("Error retrieving TagCollection for Entity ID {0}. ({1})", _entity.Id, resultcode));
                }

                _loaded = true;
            }
        }

        /// <summary>
        /// Retrieves the TagCollection for the specified Entity
        /// </summary>
        /// <param name="entity">the Entity to retrieve the TagCollection for</param>
        /// <returns>the Entity's TagCollection</returns>
        public static TagCollection Retrieve(Entity entity) {
            return new TagCollection(entity);
        }

        /// <summary>
        /// Retrieves the TagCollection for the specified Entity
        /// </summary>
        /// <param name="entityid">the id of the Entity to retrieve the TagCollection for</param>
        /// <returns>the Entity's TagCollection</returns>
        public static TagCollection Retrieve(int entityid) {
            return Retrieve(new Entity(entityid));
        }
    }
}
