using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Obscura.Common;

namespace Obscura.Entities {

    /// <summary>
    /// A Photo object
    /// </summary>
    public class Photo : Entity {
        bool _loaded = false;
        private Image _image, _thumbnail;
        private EntityCollection<Image> _resolutions;

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
                Update(null, value);
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
                Update(value, null);
            }
        }

        public EntityCollection<Image> Resolutions {
            get {
                Load();
                return _resolutions;
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
        /// <param name="thumbnail">the thumbnail Image associated with this photo</param>
        /// <param name="image">the main Image associated with this photo</param>
        /// <param name="resolutions">the collection of different Image resolutions associated with this Photo</param>
        internal Photo(Entity entity, Image thumbnail, Image image, EntityCollection<Image> resolutions)
            : base(entity) {
            _thumbnail = thumbnail;
            _image = image;
            _resolutions = resolutions;
            _loaded = true;
        }

        /// <summary>
        /// Updates this Photo
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="thumbnail">the thumbnail Image associated with this Photo</param>
        /// <param name="image">the main Image associated with this Photo</param>
        public void Update(Image thumbnail, Image image) {
            string resultcode = null;

            if (image == null)
                throw new ObscuraException("A Photo's Image may not be null");

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdatePhoto(
                    base.Id, 
                    (thumbnail == null ? null : (int?)thumbnail.Id),
                    (image == null ? null : (int?)image.Id), 
                    ref resultcode
                );

                if (resultcode == "SUCCESS") {
                    if (thumbnail != null)
                        _thumbnail = thumbnail;
                    if (image != null)
                        _image = image;
                }
                else
                    throw new ObscuraException(string.Format("Unable to update Photo Entity ID {0}. ({1})", base.Id, resultcode));
            }
        }

        /// <summary>
        /// An XML representation of this Photo
        /// </summary>
        /// <returns>An XML document</returns>
        new public XmlDocument ToXml() {
            XmlDocument dom = base.ToXml();
            XmlElement xPhoto = (XmlElement)dom.DocumentElement.AppendChild(dom.CreateElement(Type.ToString()));

            XmlElement xThumbnail = (XmlElement)xPhoto.AppendChild(dom.CreateElement("thumbnail"));
            xThumbnail.SetAttribute("id", Thumbnail.Id.ToString());
            xThumbnail.AppendChild(dom.CreateElement("url")).InnerText = Thumbnail.Url.ToString();

            XmlElement xResolution, xResolutions = (XmlElement)xPhoto.AppendChild(dom.CreateElement("resolutions"));
            foreach(Image resolution in Resolutions){
                xResolution = (XmlElement)xResolutions.AppendChild(dom.CreateElement("image"));
                xResolution.SetAttribute("id", resolution.Id.ToString());
                xResolution.AppendChild(dom.CreateElement("url")).InnerText = resolution.Url.ToString();
            }

            return dom;
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
                        _resolutions = new EntityCollection<Image>(this);
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
        /// <param name="thumbnail">the thumbnail Image associated with this Photo</param>
        /// <param name="image">the main Image associated with this Photo</param>
        /// <returns>a new Photo object</returns>
        public static Photo Create(string title, string description, Image thumbnail, Image image) {
            Photo photo = null;
            Entity entity;
            string resultcode = null;

            if(image == null)
                throw new ObscuraException("A Photo's Image may not be null");

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Photo, title, description);
                db.xspUpdatePhoto(
                    entity.Id,
                    (thumbnail == null ? null : (int?)thumbnail.Id),
                    (image == null ? null : (int?)image.Id),
                    ref resultcode);

                if (resultcode == "SUCCESS") {
                    photo = new Photo(entity, thumbnail, image, new EntityCollection<Image>(entity));
                }
                else {
                    entity.Delete();
                    throw new ObscuraException(string.Format("Unable to create Photo. ({0})", resultcode));
                }
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

        /// <summary>
        /// Retrieves all Photos from the database
        /// </summary>
        /// <returns>An EntityCollection containing all Phtoos</returns>
        public static EntityCollection<Photo> All() {
            return new EntityCollection<Photo>();
        }
    }
}
