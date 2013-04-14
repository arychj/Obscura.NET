using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {
    
    /// <summary>
    /// An Entity Url
    /// </summary>
    public class Url {
        private Entity _entity;
        private string _pretty, _actual;

        private enum UrlType {
            Actual,
            Pretty
        }

        #region accessors

        /// <summary>
        /// The pretty formatted URL of this Entity. (UrlRewrite)
        /// </summary>
        public string Pretty {
            get {
                if (_pretty == null)
                    _pretty = BuildUrl(UrlType.Pretty);

                return _pretty;
            }
        }

        /// <summary>
        /// The actual URL of this Entity
        /// </summary>
        public string Actual {
            get {
                if (_actual == null)
                    _actual = BuildUrl(UrlType.Actual);

                return _actual;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">the Entity to build a URL for</param>
        internal Url(Entity entity) {
            _entity = entity;
        }

        public override string ToString() {
            UrlType type;

            if (Enum.TryParse(Settings.GetSetting("UrlFormat"), out type))
                return BuildUrl(type);
            else
                return Actual;
        }

        /// <summary>
        /// Builds the URL for the Entity
        /// </summary>
        /// <param name="type">The URL type to build</param>
        /// <returns>the URL to the Entity</returns>
        private string BuildUrl(UrlType type) {
            string key = string.Format("UrlFormat{0}{1}", _entity.Type.ToString(), (type == UrlType.Actual ? string.Empty : type.ToString()));
            string format = Settings.GetSetting(key);

            if (key != null) {
                return DataTools.BuildString(format, new Dictionary<string, string>() {
                    {"base", Settings.GetSetting("UrlBase")},
                    {"id", _entity.Id.ToString()},
                    {"title", _entity.Title.Replace(" ", "-")}
                });
            }
            else
                throw new ObscuraException(string.Format("Unable to build URL for EntityType '{0}'. Format not found.", _entity.Type.ToString()));
        }
    }
}
