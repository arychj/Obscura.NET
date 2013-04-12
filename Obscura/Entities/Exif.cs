using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    //TODO: save exif to db

    /// <summary>
    /// Exchangeable image file format
    /// </summary>
    public class Exif {
        private Dictionary<string, string> _tags;

        private double? _shutterSpeed = null, _aperture = null, _focalLength = null, _latitude = null, _longitude = null;
        private int? _iso = null;
        private string _lens, _author, _copyright, _cameraMake, _cameraModel;
        private DateTime? _timeTaken = null;
        private Resolution _resolution;

        #region accessors

        /// <summary>
        /// The aperture the image was captured at
        /// </summary>
        public double Aperture {
            get { return (_tags.ContainsKey("Aperture") ? double.Parse(_tags["Aperture"]) : 0); }
        }

        /// <summary>
        /// The length of the exposure
        /// </summary>
        public double ShutterSpeed {
            get { return (_tags.ContainsKey("ShutterSpeed") ? double.Parse(_tags["ShutterSpeed"]) : 0); }
        }

        /// <summary>
        /// The focal length the image was captured at
        /// </summary>
        public double FocalLength {
            get { return (_tags.ContainsKey("FocalLength") ? double.Parse(_tags["FocalLength"]) : 0); }
        }

        /// <summary>
        /// The ISO the image was captured at
        /// </summary>
        public int ISO {
            get { return (_tags.ContainsKey("ISOSpeed") ? int.Parse(_tags["ISOSpeed"]) : 0); }
        }

        /// <summary>
        /// The time the image was taken
        /// </summary>
        public DateTime TimeTaken {
            get { return (_tags.ContainsKey("TimeTaken") ? DateTime.Parse(_tags["TimeTaken"]) : DateTime.MinValue); }
        }

        /// <summary>
        /// The make of the camera which captured the image
        /// </summary>
        public string CameraMake {
            get { return (_tags.ContainsKey("CameraMake") ? _tags["CameraMake"] : "unknown"); }
        }

        /// <summary>
        /// The model of the camera which captured the image
        /// </summary>
        public string CameraModel {
            get { return (_tags.ContainsKey("CameraModel") ? _tags["CameraModel"] : "unknown"); }
        }

        /// <summary>
        /// The type of lens which was used to capture the image
        /// </summary>
        public string Lens {
            get { return (_tags.ContainsKey("Lens") ? _tags["Lens"] : "unknown"); }
        }

        /// <summary>
        /// The author who captured the image
        /// </summary>
        public string Author {
            get { return (_tags.ContainsKey("Author") ? _tags["Author"] : "unknown"); }
        }

        /// <summary>
        /// THe copyright info associated with the image
        /// </summary>
        public string Copyright {
            get { return (_tags.ContainsKey("Copyright") ? _tags["Copyright"] : "unknown"); }
        }

        /// <summary>
        /// The latitude at which the image was taken
        /// </summary>
        public double Latitude {
            get { return (_tags.ContainsKey("Latitude") ? double.Parse(_tags["Latitude"]) : 0); }
        }

        /// <summary>
        /// The longitude at which the image was taken
        /// </summary>
        public double Longitude {
            get { return (_tags.ContainsKey("Longitude") ? double.Parse(_tags["Longitude"]) : 0); }
        }

        /// <summary>
        /// The resolution of the image
        /// </summary>
        public Resolution Resolution {
            get { return _resolution; }
        }

        /// <summary>
        /// The raw exif tags
        /// </summary>
        internal Dictionary<string, string> Tags {
            get { return _tags; }
        }

        #endregion accessors

        /// <summary>
        /// Constructor
        /// Parses the exif metadata from a file
        /// </summary>
        /// <param name="file">the file to read</param>
        public Exif(string file) {
            using (ExifReader reader = new ExifReader(file)) {
                ReadExif(reader);
            }
        }

        /// <summary>
        /// Constructor
        /// Parses the exif metadata from a stream
        /// </summary>
        /// <param name="stream">the stream to read</param>
        public Exif(Stream stream) {
            using (ExifReader reader = new ExifReader(stream)) {
                ReadExif(reader);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aperture">the aperture the image was captured at</param>
        /// <param name="shutterSpeed">the length of the exposure</param>
        /// <param name="focalLength">the focal length the image was catured at</param>
        /// <param name="iso">the ISO the image was captured at</param>
        /// <param name="lens">the lens which was used to capture the image</param>
        internal Exif(double aperture, int shutterSpeed, double focalLength, int iso, string lens, DateTime timeTaken, string author, string copyright, double latitude, double longitude) {
            _aperture = aperture;
            _shutterSpeed = shutterSpeed;
            _focalLength = focalLength;
            _iso = iso;
            _lens = lens;
            _timeTaken = timeTaken;
            _author = author;
            _copyright = copyright;
            _latitude = latitude;
            _longitude = longitude;
        }

        private void ReadExif(ExifReader exif) {
            _tags = new Dictionary<string, string>();

            double d; int i; string s; ushort u; DateTime dt;
            try {
                //exposure
                exif.GetTagValue(ExifTags.FNumber, out d);
                _tags.Add("Aperture", d.ToString());

                exif.GetTagValue(ExifTags.ExposureTime, out d);
                _tags.Add("ShutterSpeed", d.ToString());

                exif.GetTagValue(ExifTags.ISOSpeedRatings, out u);
                _tags.Add("ISOSpeed", u.ToString());

                exif.GetTagValue(ExifTags.FocalLength, out d);
                _tags.Add("FocalLength", d.ToString());

                exif.GetTagValue(ExifTags.DateTime, out dt);
                _tags.Add("TimeTaken", dt.ToString());

                //camera
                exif.GetTagValue(ExifTags.Make, out s);
                _tags.Add("CameraMake", s.ToString());

                exif.GetTagValue(ExifTags.Model, out s);
                _tags.Add("CameraModel", s.ToString());

                //location
                exif.GetTagValue(ExifTags.GPSLatitude, out d);
                _tags.Add("Latitude", d.ToString());

                exif.GetTagValue(ExifTags.GPSLongitude, out d);
                _tags.Add("Longitude", d.ToString());

                //author
                exif.GetTagValue(ExifTags.Artist, out s);
                _tags.Add("Author", s.ToString());

                exif.GetTagValue(ExifTags.Copyright, out s);
                _tags.Add("Copyright", s.ToString());

                //image details
                double x, y;
                exif.GetTagValue(ExifTags.XResolution, out x);
                exif.GetTagValue(ExifTags.YResolution, out y);
                _resolution = new Resolution((int)x, (int)y);
            }
            catch (ExifLibException) { }
        }
    }
}
