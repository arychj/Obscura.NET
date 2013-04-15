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
                Update(value, null);
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
                Update(null, value);
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
        internal Album(Entity entity, Image thumbnail, Image cover, EntityCollection<Photo> photos)
            : base(entity) {
                _thumbnail = thumbnail;
                _cover = cover;
                _photos = photos;
                _loaded = true;
        }

        /// <summary>
        /// Updates this Album
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="thumbnail">the thumbnail Image associated with this Album</param>
        /// <param name="cover">the cover Image associated with this Album</param>
        public void Update(Image thumbnail, Image cover) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateAlbum(
                    base.Id, 
                    (cover == null ? null : (int?)cover.Id), 
                    (thumbnail == null ? null : (int?)thumbnail.Id), 
                    ref resultcode
                );

                if (resultcode == "SUCCESS") {
                    if (thumbnail != null)
                        _thumbnail = thumbnail;
                    if (cover != null)
                        _cover = cover;
                }
                else
                    throw new ObscuraException(string.Format("Unable to update Album Entity ID {0}. ({1})", base.Id, resultcode));
            }
        }

        /// <summary>
        /// An XML representation of this Album
        /// </summary>
        /// <returns>An XML document</returns>
        new public XmlDocument ToXml(){
            XmlDocument dom = base.ToXml();
            XmlElement xAlbum = (XmlElement)dom.DocumentElement.AppendChild(dom.CreateElement(Type.ToString()));

            XmlElement xThumbnail = (XmlElement)xAlbum.AppendChild(dom.CreateElement("thumbnail"));
            xThumbnail.SetAttribute("id", Thumbnail.Id.ToString());
            xThumbnail.AppendChild(dom.CreateElement("url")).InnerText = Thumbnail.Url.ToString();

            XmlElement xCover = (XmlElement)xAlbum.AppendChild(dom.CreateElement("cover"));
            xCover.SetAttribute("id", Cover.Id.ToString());
            xCover.AppendChild(dom.CreateElement("url")).InnerText = Cover.Url.ToString();

            return dom;
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
                        _photos = new EntityCollection<Photo>(this);
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
        public static Album Create(string title, string description, Image thumbnail, Image cover) {
            Album album = null;
            Entity entity;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Album, title, description);
                db.xspUpdateAlbum(
                    entity.Id,
                    (thumbnail == null ? null : (int?)thumbnail.Id),
                    (cover == null ? null : (int?)cover.Id),
                    ref resultcode
                );

                if (resultcode == "SUCCESS") {
                    album = new Album(entity, cover, thumbnail, new EntityCollection<Photo>(entity));
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
