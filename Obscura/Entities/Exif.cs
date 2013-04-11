using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Entities {
    
    /// <summary>
    /// Exchangeable image file format
    /// </summary>
    public class Exif {
        private double _shutterSpeed, _aperture, _focalLength, _latitude, _longitude;
        private int _iso;
        private string _lens, _author, _copyright, _cameraMake, _cameraModel;
        private DateTime _timeTaken;
        private Resolution _resolution;

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
        public double ShutterSpeed {
            get { return _shutterSpeed; }
        }

        /// <summary>
        /// The focal length the image was captured at
        /// </summary>
        public double FocalLength {
            get { return _focalLength; }
        }

        /// <summary>
        /// The ISO the image was captured at
        /// </summary>
        public int ISO {
            get { return _iso; }
        }

        /// <summary>
        /// The time the image was taken
        /// </summary>
        public DateTime TimeTaken {
            get { return _timeTaken; }
        }

        /// <summary>
        /// The make of the camera which captured the image
        /// </summary>
        public string CamerMake {
            get { return _cameraMake; }
        }

        /// <summary>
        /// The model of the camera which captured the image
        /// </summary>
        public string CameraModel {
            get { return _cameraModel; }
        }

        /// <summary>
        /// The type of lens which was used to capture the image
        /// </summary>
        public string Lens {
            get { return _lens; }
        }

        /// <summary>
        /// The author who captured the image
        /// </summary>
        public string Author {
            get { return _author; }
        }

        /// <summary>
        /// THe copyright info associated with the image
        /// </summary>
        public string Copyright {
            get { return _copyright; }
        }

        /// <summary>
        /// The latitude at which the image was taken
        /// </summary>
        public double Latitude {
            get { return _latitude; }
        }

        /// <summary>
        /// The longitude at which the image was taken
        /// </summary>
        public double Longitude {
            get { return _longitude; }
        }

        /// <summary>
        /// The resolution of the image
        /// </summary>
        public Resolution Resolution {
            get { return _resolution; }
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
            try {
                //exposure
                exif.GetTagValue(ExifTags.ShutterSpeedValue, out _shutterSpeed);
                exif.GetTagValue(ExifTags.ApertureValue, out _aperture);

                ushort temp;
                exif.GetTagValue(ExifTags.ISOSpeedRatings, out temp);
                _iso = (int)temp;

                exif.GetTagValue(ExifTags.FocalLength, out _focalLength);
                exif.GetTagValue(ExifTags.DateTime, out _timeTaken);

                //camera
                exif.GetTagValue(ExifTags.Make, out _cameraMake);
                exif.GetTagValue(ExifTags.Model, out _cameraModel);

                //location
                exif.GetTagValue(ExifTags.GPSLatitude, out _latitude);
                exif.GetTagValue(ExifTags.GPSLongitude, out _longitude);

                //author
                exif.GetTagValue(ExifTags.Artist, out _author);
                exif.GetTagValue(ExifTags.Copyright, out _copyright);

                //image details
                double x, y;
                exif.GetTagValue(ExifTags.XResolution, out x);
                exif.GetTagValue(ExifTags.YResolution, out y);
                _resolution = new Resolution((int)x, (int)y);
            }
            catch (ExifLibException) {
                _shutterSpeed = -1;
                _aperture = -1;
                _iso = -1;
                _focalLength = -1;
                _timeTaken = DateTime.MinValue;
                _cameraMake = "unknown";
                _cameraModel = "unknown";
                _latitude = -1;
                _longitude = -1;
                _author = "unknown";
                _copyright = "unknown";
                _resolution = new Resolution(-1, -1);
            }
        }
    }
}
