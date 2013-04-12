using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obscura.Common;

namespace Obscura.Entities {
    //TODO: save exif to db

    /// <summary>
    /// Exchangeable image file format
    /// </summary>
    public class Exif {
        private Dictionary<string, string> _tags;
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
        /// Loads exif data from an Entity
        /// </summary>
        /// <param name="entityid">the Entity id to load</param>
        internal Exif(int entityid) {
            string resultcode = null;

            _tags = new Dictionary<string, string>();
            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                ISingleResult<xspGetImageExifDataResult> tags = db.xspGetImageExifData(entityid, ref resultcode);

                if (resultcode == "SUCCESS") {
                    foreach (xspGetImageExifDataResult tag in tags)
                        _tags.Add(tag.Type, tag.Value);
                }
                else
                    throw new ObscuraException(string.Format("Unable to retreive exif data for Entity Id {0}. ({1})", entityid, resultcode));
            }
        }

        /// <summary>
        /// Saves the exif data to an Entity
        /// </summary>
        /// <param name="entityid">the Entity id to save to</param>
        internal void SaveToEntity(int entityid) {
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                foreach (KeyValuePair<string, string> tag in _tags) {
                    db.xspUpdateImageExifData(entityid, tag.Key, tag.Value, ref resultcode);

                    if (resultcode != "SUCCESS")
                        throw new ObscuraException(string.Format("Unable to create Image exif entry ({0}/{1}). ({2})", tag.Key, tag.Value, resultcode));
                }
            }
        }

        /// <summary>
        /// Reade the Image's exif data
        /// </summary>
        /// <param name="reader">the reader to read from</param>
        private void ReadExif(ExifReader reader) {
            _tags = new Dictionary<string, string>();

            double d; int i; string s; ushort u; DateTime dt;
            try {
                //exposure
                reader.GetTagValue(ExifTags.FNumber, out d);
                _tags.Add("Aperture", d.ToString());

                reader.GetTagValue(ExifTags.ExposureTime, out d);
                _tags.Add("ShutterSpeed", d.ToString());

                reader.GetTagValue(ExifTags.ISOSpeedRatings, out u);
                _tags.Add("ISOSpeed", u.ToString());

                reader.GetTagValue(ExifTags.FocalLength, out d);
                _tags.Add("FocalLength", d.ToString());

                reader.GetTagValue(ExifTags.DateTime, out dt);
                _tags.Add("TimeTaken", dt.ToString());

                //camera
                reader.GetTagValue(ExifTags.Make, out s);
                _tags.Add("CameraMake", s.ToString());

                reader.GetTagValue(ExifTags.Model, out s);
                _tags.Add("CameraModel", s.ToString());

                //location
                reader.GetTagValue(ExifTags.GPSLatitude, out d);
                _tags.Add("Latitude", d.ToString());

                reader.GetTagValue(ExifTags.GPSLongitude, out d);
                _tags.Add("Longitude", d.ToString());

                //author
                reader.GetTagValue(ExifTags.Artist, out s);
                _tags.Add("Author", s.ToString());

                reader.GetTagValue(ExifTags.Copyright, out s);
                _tags.Add("Copyright", s.ToString());

                //image details
                double x, y;
                reader.GetTagValue(ExifTags.XResolution, out x);
                reader.GetTagValue(ExifTags.YResolution, out y);
                _resolution = new Resolution((int)x, (int)y);
            }
            catch (ExifLibException) { }
        }
    }
}
