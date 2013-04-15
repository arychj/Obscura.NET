using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    public class Video : Entity {
        private VideoSource _source;

        public enum VideoSource {
            Vimeo, 
            YouTube
        }

        #region accessores

        public VideoSource Source {
            get { return _source; }
            set {
                Update(null, value);
                _source = value;
            }
        }

        #endregion

        public Video(int id)
            : base(id) {

        }

        public string GetHtml() {
            //TODO: GetHtml()
            return null;
        }

        public void Update(string url, VideoSource? source) {
            //TODO: update
        }

        public static Video Create() {
            //TODO: create
            return null;
        }
    }
}
