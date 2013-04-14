using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// An Album Collection
    /// </summary>
    public class Collection : Entity {
        private bool _loaded = false;

        private EntityCollection<Album> _albums;
        private Image _thumbnail, _cover;

        #region accessors

        /// <summary>
        /// The thumbnail Image associated with this Album
        /// </summary>
        public Image Thumbnail {
            get {
                Load();
                return _thumbnail;
            }
            set {
                Load();
                Update(value.Id, null);
                _thumbnail = value;
            }
        }

        /// <summary>
        /// The cover Image associated with this Album
        /// </summary>
        public Image Cover {
            get {
                Load();
                return _cover;
            }
            set {
                Load();
                Update(null, value.Id);
                _cover = value;
            }
        }

        /// <summary>
        /// The collection of Photos associated with this Album
        /// </summary>
        public EntityCollection<Album> Albums {
            get {
                Load();
                return _albums;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// Retrieves a Collection
        /// </summary>
        /// <param name="id">the id of the Collection</param>
        internal Collection(int id) : this(id, false) { }

        /// <summary>
        /// Constructor
        /// Retrieves a Collection
        /// </summary>
        /// <param name="id">the id of the Collection</param>
        /// <param name="loadImmediately">Load the Collection's contents immediately</param>
        internal Collection(int id, bool loadImmediately)
            : base(id, loadImmediately) {
                if (loadImmediately)
                    Load();
        }

        /// <summary>
        /// Constructor
        /// Builds a Collection using the specified parameters
        /// </summary>
        /// <param name="entity">the base Entity of this Collection</param>
        /// <param name="cover">the cover Image associated with this Collection</param>
        /// <param name="thumbnail">the thumbnail Image associated with this Collection</param>
        /// <param name="albums">the collection of Albums associated with this Collection</param>
        internal Collection(Entity entity, Image cover, Image thumbnail, EntityCollection<Album> albums)
            : base(entity) {
                _cover = cover;
                _thumbnail = thumbnail;
                _albums = albums;
        }

        /// <summary>
        /// Updates this Collection
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="thumbnailid">the id of the thumbnail Image associated with this Collection</param>
        /// <param name="coverid">the id of the cover Image associated with this Collection</param>
        public void Update(int? thumbnailid, int? coverid) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateCollection(base.Id, coverid, thumbnailid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    if (thumbnailid != null)
                        _thumbnail = new Image((int)thumbnailid);
                    if (coverid != null)
                        _cover = new Image((int)coverid);
                }
                else
                    throw new ObscuraException(string.Format("Unable to update Collection Entity ID {0}. ({1})", base.Id, resultcode));
            }
        }

        /// <summary>
        /// Loads the Collections's details from the database
        /// </summary>
        private void Load() {
            if (!_loaded) {
                int? coverid = null, thumbid = null;
                string resultcode = null;

                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    db.xspGetCollection(base.Id, ref coverid, ref thumbid, ref resultcode);
                }

                _thumbnail = new Image((int)thumbid);
                _cover = new Image((int)coverid);
                _loaded = true;
            }
        }

        /// <summary>
        /// Creates a new Collection
        /// </summary>
        /// <param name="title">the title of the Collection</param>
        /// <param name="description">the description of the Collection</param>
        /// <param name="cover">the cover Image of the Collection</param>
        /// <param name="thumbnail">the thumbnail Image of the Collection</param>
        /// <returns>a new Collection</returns>
        public static Collection Create(string title, string description, Image cover, Image thumbnail) {
            Collection collection = null;
            Entity entity;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Collection, title, description);
                db.xspUpdateCollection(entity.Id, cover.Id, thumbnail.Id, ref resultcode);

                if (resultcode == "SUCCESS") {
                    collection = new Collection(entity, cover, thumbnail, new EntityCollection<Album>(entity.Id));
                }
                else {
                    entity.Delete();
                    throw new ObscuraException(string.Format("Unable to create Collection. ({0})", resultcode));
                }
            }

            return collection;
        }

        /// <summary>
        /// Retrieves the specified Collection
        /// </summary>
        /// <param name="id">the id of the Collection to retrieve</param>
        /// <returns>the Collection</returns>
        new public static Collection Retrieve(int id) {
            return new Collection(id);
        }
    }
}
