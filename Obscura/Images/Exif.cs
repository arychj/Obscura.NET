using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Images {

    /// <summary>
    /// Exchangeable image file format
    /// </summary>
    public class Exif {
        private double _exposure, _aperture;
        private int _focalLength, _iso;
        private string _lens;

        #region accessors

        /// <summary>
        /// The aperture the image was captured at
        /// </summary>
        public double Aperture {
            get { return _aperture; }
        }

        /// <summary>
        /// The length of the exposure
        /// </summary>
        public double Exposure {
            get { return _exposure; }
        }

        /// <summary>
        /// The focal length the image was captured at
        /// </summary>
        public int FocalLength {
            get { return _focalLength; }
        }

        /// <summary>
        /// The ISO the image was captured at
        /// </summary>
        public int ISO {
            get { return _iso; }
        }

        /// <summary>
        /// The lens which was used to capture the image
        /// </summary>
        public string Lens {
            get { return _lens; }
        }

        #endregion accessors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aperture">the aperture the image was captured at</param>
        /// <param name="exposure">the length of the exposure</param>
        /// <param name="focalLength">the focal length the image was catured at</param>
        /// <param name="iso">the ISO the image was captured at</param>
        /// <param name="lens">the lens which was used to capture the image</param>
        internal Exif(double aperture, double exposure, int focalLength, int iso, string lens) {
            _aperture = aperture;
            _exposure = exposure;
            _focalLength = focalLength;
            _iso = iso;
            _lens = lens;
        }
    }
}
