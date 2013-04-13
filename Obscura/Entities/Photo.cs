using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// A Photo object
    /// </summary>
    public class Photo : Entity {
        bool _loaded = false;
        private Image _image, _thumbnail;

        #region accessors

        /// <summary>
        /// The main Image associated with the Photo
        /// </summary>
        public Image Image {
            get {
                Load();
                return _image;
            }
            set {
                Load();
                Update(null, value.Id);
                _image = value;
            }
        }

        /// <summary>
        /// The thumbnail Image associated with the Photo
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

        #endregion

        /// <summary>
        /// Constructor
        /// Retrieves a Photo
        /// </summary>
        /// <param name="id">the id of the Photo</param>
        internal Photo(int id) : this(id, false) { }

        /// <summary>
        /// Constructor
        /// Retrieves a Photo
        /// </summary>
        /// <param name="id">the id of the Photo</param>
        /// <param name="loadImmediately">Load the Photo's contents immediately</param>
        internal Photo(int id, bool loadImmediately)
            : base(id, loadImmediately) {
                if (loadImmediately)
                    Load();
        }

        /// <summary>
        /// Constructor
        /// Builds a Photo using the specified parameters
        /// </summary>
        /// <param name="entity">the base Entity of this photo</param>
        /// <param name="photoid">the internal id of this photo</param>
        /// <param name="thumbnailid">the id of the thumbnail Image associated with this photo</param>
        /// <param name="imageid">the id of the main Image associated with this photos</param>
        internal Photo(Entity entity, int thumbnailid, int imageid)
            : base(entity) {
            _thumbnail = new Image(thumbnailid);
            _image = new Image(imageid);
            _loaded = true;
        }

        /// <summary>
        /// Updates this Photo
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="thumbnailid">the id of the thumbnail Image associated with this Photo</param>
        /// <param name="imageid">the id of the main Image associated with this Photo</param>
        public void Update(int? thumbnailid, int? imageid) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdatePhoto(base.Id, thumbnailid, imageid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    if (thumbnailid != null)
                        _thumbnail = new Image((int)thumbnailid);
                    if (imageid != null)
                        _image = new Image((int)imageid);
                }
                else
                    throw new ObscuraException(string.Format("Unable to update Photo Entity ID {0}. ({1})", base.Id, resultcode));
            }
        }

        /// <summary>
        /// Loads the Photo's details
        /// </summary>
        private void Load() {
            if (!_loaded) {
                int? entityid = base.Id;
                int? thumbnailid = null, imageid = null;
                string resultcode = null;

                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    ISingleResult<xspGetPhotoResult> images = db.xspGetPhoto(ref entityid, ref thumbnailid, ref imageid, ref resultcode);

                    if (resultcode == "SUCCESS") {
                        _thumbnail = new Image((int)thumbnailid);
                        _image = new Image((int)imageid);
                    }
                    else
                        throw new ObscuraException(string.Format("Photo Entity Id {0} does not exist. ({1})", base.Id, resultcode));
                }

                _loaded = true;
            }
        }

        /// <summary>
        /// Creates a new photo
        /// </summary>
        /// <param name="title">the title of Photo</param>
        /// <param name="description">the description of the Photo</param>
        /// <param name="thumbnailid">the id of the thumbnail Image associated with this Photo</param>
        /// <param name="imageid">the id of the main Image associated with this Photo</param>
        /// <returns>a new Photo object</returns>
        public static Photo Create(string title, string description, int thumbnailid, int imageid) {
            Photo photo = null;
            Entity entity;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Photo, title, description);
                db.xspUpdatePhoto(entity.Id, thumbnailid, imageid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    photo = new Photo(entity, thumbnailid, imageid);
                }
                else
                    throw new ObscuraException(string.Format("Unable to create Photo. ({0})", resultcode));
            }

            return photo;
        }

        /// <summary>
        /// Retrieves the specified Photo
        /// </summary>
        /// <param name="id">the id of the Photo to retrieve</param>
        /// <returns>the Photo</returns>
        new public static Photo Retrieve(int id) {
            return new Photo(id, true);
        }
    }
}
