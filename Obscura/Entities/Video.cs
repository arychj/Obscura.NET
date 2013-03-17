using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class Video : Entity {
        private EntityKey _key;
        private string _url;
        private VideoSource _type;

        public enum VideoSource {
            Vimeo, 
            YouTube
        }

        #region accessores

        public VideoSource Type {
            get { return _type; }
        }

        public string Url {
            get { return _url; }
        }

        #endregion

        public Video(EntityKey key)
            : base(key) {

        }

        public string GetHtml() {
            //TODO: GetHtml()
            return null;
        }

        public static Video Create() {
            //TODO: create
            return null;
        }
    }
}
