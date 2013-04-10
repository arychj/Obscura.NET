using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;
using Obscura.Images;

namespace Obscura.Entities {

    /// <summary>
    /// A Photo object
    /// </summary>
    public class Photo : Entity{
        private int _id;
        private Image _image, _thumbnail;

        #region accessors

        /// <summary>
        /// The internal photo-id
        /// </summary>
        internal int PhotoId {
            get { return _id; }
        }

        /// <summary>
        /// The main Image associated with the Photo
        /// </summary>
        public Image Image {
            get { return _image; }
            set {
                _image = value;
                Update(null, value.Id);
            }
        }

        /// <summary>
        /// The thumbnail Image associated with the Photo
        /// </summary>
        public Image Thumbnail {
            get { return _thumbnail; }
            set {
                _thumbnail = value;
                Update(value.Id, null);
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// Retrieves a Photo
        /// </summary>
        /// <param name="id">the id of the Photo</param>
        public Photo(int id) 
            : base(id) {
            int? photoid = null, entityid = id;
            int? thumbnailid = null, imageid = null;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                ISingleResult<xspGetPhotoResult> images = db.xspGetPhoto(ref entityid, ref thumbnailid, ref imageid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    _id = (int)photoid;
                    _thumbnail = new Image((int)thumbnailid);
                    _image = new Image((int)imageid);
                }
                else
                    throw new ObscuraException(string.Format("Photo Entity Id {0} does not exist. ({1})", id, resultcode));
            }
        }

        /// <summary>
        /// Constructor
        /// Builds a Photo using the specified parameters
        /// </summary>
        /// <param name="entity">the base Entity of this photo</param>
        /// <param name="photoid">the internal id of this photo</param>
        /// <param name="thumbnailid">the id of the thumbnail Image associated with this photo</param>
        /// <param name="imageid">the id of the main Image associated with this photos</param>
        internal Photo(Entity entity, int photoid, int thumbnailid, int imageid)
            : base(entity.Id, entity.TypeId, entity.Type, entity.Title, entity.Description, entity.HitCount, entity.Dates, entity.IsActive) {
            _id = photoid;
            _thumbnail = new Image(thumbnailid);
            _image = new Image(imageid);
        }

        /// <summary>
        /// Updates this Photo
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="thumbnailid">the id of the thumbnail Image associated with this Photo</param>
        /// <param name="imageid">the id of the main Image associated with this Photo</param>
        public void Update(int? thumbnailid, int? imageid) {
            int? entityid = base.Id, photoid = _id;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdatePhoto(entityid, thumbnailid, imageid, ref resultcode);

                if(resultcode != "SUCCESS")
                    throw new ObscuraException(string.Format("Unable to update Photo Entity ID {0}. ({1})", base.Id, resultcode));
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
            int? photoid = -1;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Photo, title, description);
                db.xspUpdatePhoto(entity.Id, thumbnailid, imageid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    photo = new Photo(entity, (int)photoid, thumbnailid, imageid);
                }
                else
                    throw new ObscuraException(string.Format("Unable to create Photo. ({0})", resultcode));
            }

            return photo;
        }
    }
}
