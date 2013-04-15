using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        internal Collection(Entity entity, Image thumbnail, Image cover, EntityCollection<Album> albums)
            : base(entity) {
                _thumbnail = thumbnail;
                _cover = cover;
                _albums = albums;
        }

        /// <summary>
        /// Updates this Collection
        /// Specify null for any parameters to not update
        /// </summary>
        /// <param name="thumbnail">the thumbnail Image associated with this Collection</param>
        /// <param name="cover">the cover Image associated with this Collection</param>
        public void Update(Image thumbnail, Image cover) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspUpdateCollection(
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
                    throw new ObscuraException(string.Format("Unable to update Collection Entity ID {0}. ({1})", base.Id, resultcode));
            }
        }

        /// <summary>
        /// An XML representation of this Collection
        /// </summary>
        /// <returns>An XML document</returns>
        new public XmlDocument ToXml() {
            XmlDocument dom = base.ToXml();
            XmlElement xCollection = (XmlElement)dom.DocumentElement.AppendChild(dom.CreateElement(Type.ToString()));

            XmlElement xThumbnail = (XmlElement)xCollection.AppendChild(dom.CreateElement("thumbnail"));
            xThumbnail.SetAttribute("id", Thumbnail.Id.ToString());
            xThumbnail.AppendChild(dom.CreateElement("url")).InnerText = Thumbnail.Url.ToString();

            XmlElement xCover = (XmlElement)xCollection.AppendChild(dom.CreateElement("cover"));
            xCover.SetAttribute("id", Cover.Id.ToString());
            xCover.AppendChild(dom.CreateElement("url")).InnerText = Cover.Url.ToString();

            return dom;
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
                _albums = new EntityCollection<Album>(this);
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
        public static Collection Create(string title, string description, Image thumbnail, Image cover) {
            Collection collection = null;
            Entity entity;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                entity = Entity.Create(EntityType.Collection, title, description);
                db.xspUpdateCollection(
                    entity.Id,
                    (thumbnail == null ? null : (int?)thumbnail.Id),
                    (cover == null ? null : (int?)cover.Id),
                    ref resultcode
                );

                if (resultcode == "SUCCESS") {
                    collection = new Collection(entity, cover, thumbnail, new EntityCollection<Album>(entity));
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
