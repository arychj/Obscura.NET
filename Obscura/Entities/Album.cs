using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// A Photo Album
    /// </summary>
    public class Album : Entity {
        private bool _loaded = false;

        private EntityCollection<Photo> _photos;
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
        public EntityCollection<Photo> Photos {
            get {
                Load();
                return _photos;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// Retrieves an Album
        /// </summary>
        /// <param name="id">the id of the Album</param>
        internal Album(int id) : this(id, false) { }

        /// <summary>
        /// Constructor
        /// Retrieves an Album
        /// </summary>
        /// <param name="id">the id of the Album</param>
        /// <param name="loadImmediately">Load the Album's contents immediately</param>
        internal Album(int id, bool loadImmediately)
            : base(id, loadImmediately) {
                if (loadImmediately)
                    Load();
        }

        /// <summary>
        /// Constructor
        /// Builds an Album using the specified parameters
        /// </summary>
        /// <param name="entity">the base Entity of this Album</param>
        /// <param name="cover">the cover Image associated with this Album</param>
        /// <param name="thumbnail">the thumbnail Image associated with this Album</param>
        /// <param name="photos">the collection of Photos associated with this album</param>
        internal Album(Entity entity, Image cover, Image thumbnail, EntityCollection<Photo> photos)
            : base(entity) {
                _cover = cover;
                _thumbnail = thumbnail;
                _photos = photos;
                _loaded = true;
        }

        /// <summary>
        /// Updates this Album
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="thumbnailid">the id of the thumbnail Image associated with this Album</param>
        /// <param name="coverid">the id of the cover Image associated with this Album</param>
        public void Update(int? thumbnailid, int? coverid) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateAlbum(base.Id, coverid, thumbnailid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    if (thumbnailid != null)
                        _thumbnail = new Image((int)thumbnailid);
                    if (coverid != null)
                        _cover = new Image((int)coverid);
                }
                else
                    throw new ObscuraException(string.Format("Unable to update Album Entity ID {0}. ({1})", base.Id, resultcode));
            }
        }

        /// <summary>
        /// Loads the Album's details from the database
        /// </summary>
        private void Load() {
            if (!_loaded) {
                int? coverid = null, thumbid = null;
                string resultcode = null;

                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    db.xspGetAlbum(base.Id, ref thumbid, ref coverid, ref resultcode);

                    if (resultcode == "SUCCESS") {
                        _cover = new Image((int)coverid);
                        _thumbnail = new Image((int)thumbid);
                        _photos = new EntityCollection<Photo>(base.Id);
                    }
                    else
                        throw new ObscuraException(string.Format("Album Entity Id {0} does not exist. ({1})", base.Id, resultcode));

                }

                _loaded = true;
            }
        }

        /// <summary>
        /// Creates a new Album
        /// </summary>
        /// <param name="title">the title of the Album</param>
        /// <param name="description">the description of the Album</param>
        /// <param name="cover">the cover Image of the Album</param>
        /// <param name="thumbnail">the thumbnail Image of the Album</param>
        /// <returns>a new Album</returns>
        public static Album Create(string title, string description, Image cover, Image thumbnail) {
            Album album = null;
            Entity entity;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Album, title, description);
                db.xspUpdateAlbum(entity.Id, cover.Id, thumbnail.Id, ref resultcode);

                if (resultcode == "SUCCESS") {
                    album = new Album(entity, cover, thumbnail, new EntityCollection<Photo>(entity.Id));
                }
                else {
                    entity.Delete();
                    throw new ObscuraException(string.Format("Unable to create Album. ({0})", resultcode));
                }
            }

            return album;
        }

        /// <summary>
        /// Retrieves the specified Album
        /// </summary>
        /// <param name="id">the id of the Album to retrieve</param>
        /// <returns>the Album</returns>
        new public static Album Retrieve(int id) {
            return new Album(id, true);
        }
    }
}
