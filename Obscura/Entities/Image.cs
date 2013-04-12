using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Obscura.Common;

namespace Obscura.Entities {
    public class Image : Entity {
        private Resolution _resolution;
        private Exif _exif = null;
        private string _filePath, _mimeType, _url = null, _html = null;

        #region accessors

        /// <summary>
        /// The contents of the Image file
        /// </summary>
        public byte[] Bytes {
            get { return File.ReadAllBytes(_filePath); }
        }

        /// <summary>
        /// The mime type of the image
        /// </summary>
        public string MimeType {
            get { return _mimeType; }
        }

        /// <summary>
        /// The URL to the image
        /// </summary>
        public string Url {
            get {
                if (_url == null) {
                    _url = DataTools.BuildString(Settings.GetSetting("ImageUrlFormat"), new Dictionary<string, string>() {
                        {"id", base.Id.ToString()},
                        {"title", base.Title.Replace(" ", "-")}
                    });
                }

                return _url;
            }
        }

        /// <summary>
        /// The HTML tag for the image
        /// </summary>
        public string Html {
            get {
                if(_html == null)
                    _html = string.Format("<img src = \"{0}\" alt = \"{1}\"/>", Url, base.Title);

                return _html;
            }
        }

        /// <summary>
        /// The Exif data associated with the Image
        /// </summary>
        public Exif Exif {
            get {
                if(_exif == null)
                    _exif = new Exif(base.Id);

                return _exif;
            }
        }

        /// <summary>
        /// The resolution of the Image
        /// </summary>
        public Resolution Resolution {
            get { return _resolution; }
        }

        /// <summary>
        /// The local path to the Image file
        /// </summary>
        internal string FilePath {
            get { return string.Format("{0}/{1}", Settings.GetSetting("ImageDirectory"), _filePath); }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// Loads an image by id
        /// </summary>
        /// <param name="id">the id to load</param>
        public Image(int id) : base(id) {
            int? resolutionX = null, resolutionY = null;
            string resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                db.xspGetImage(base.Id, ref _filePath, ref _mimeType, ref resolutionX, ref resolutionY, ref resultcode);
            }

            if (resultcode == "SUCCESS") {
                _resolution = new Resolution((int)resolutionX, (int)resolutionY);
            }
            else
                throw new ObscuraException(string.Format("Unable to load Image ID {0}. ({1})", id, resultcode));
        }

        /// <summary>
        /// Constructor
        /// Self constructor
        /// </summary>
        /// <param name="entity">the Image's base Entity</param>
        /// <param name="path">the path to the Image's file</param>
        /// <param name="mimeType">the MimeType of the Image</param>
        /// <param name="exif">the exif data associated with the Image</param>
        internal Image(Entity entity, string path, string mimeType, Exif exif)
            : base(entity) {
                _filePath = path;
                _mimeType = mimeType;
                _exif = exif;
                _resolution = exif.Resolution;
        }

        /// <summary>
        /// Writes the Image's contents to the response stream
        /// </summary>
        /// <param name="response">the response object to write to</param>
        public void Write(HttpResponse response) {
            int seq;
            byte[] buffer = new byte[4096];
            FileStream fs = File.Open(FilePath, FileMode.Open);
            BinaryReader stream = new BinaryReader(fs);

            response.Clear();
            response.ContentType = MimeType; 

            seq = stream.Read(buffer, 0, 4096);
            while (seq > 0) {
                response.OutputStream.Write(buffer, 0, seq);
                seq = stream.Read(buffer, 0, 4096);
            }

            fs.Close();
        }

        /// <summary>
        /// Creates a new Image
        /// </summary>
        /// <param name="sourcePath">the source file for the image</param>
        /// <returns>a new Image</returns>
        public static Image Create(string sourcePath) {
            Image image = null;
            string extension, fileName, destPath, mimeType, resultcode = null;

            if (File.Exists(sourcePath)) {
                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    Entity entity = Entity.Create(EntityType.Image, string.Empty, string.Empty);

                    extension = Path.GetExtension(sourcePath).TrimStart('.');
                    mimeType = Common.MimeType.ParseExtension(extension);
                    fileName = string.Format("{0}.{1}", entity.Id, extension);
                    destPath = string.Format(@"{0}\{1}", Settings.GetSetting("ImageDirectory"), fileName);

                    File.Move(sourcePath, destPath);
                    Exif exif = new Exif(destPath);

                    db.xspUpdateImage(entity.Id, fileName, mimeType, exif.Resolution.X, exif.Resolution.Y, ref resultcode);

                    if (resultcode == "SUCCESS") {
                        image = new Image(entity, fileName, mimeType, exif);
                        exif.SaveToEntity(entity.Id);
                    }
                    else
                        throw new ObscuraException(string.Format("Unable to create Image. ({0})", resultcode));
                }
            }
            else
                throw new ObscuraException(string.Format("Unable to create Image from '{0}'. File does not exist.", sourcePath));

            return image;
        }

        /// <summary>
        /// Creates a new Image
        /// </summary>
        /// <param name="bytes">the byte contents of the new Image</param>
        /// <param name="mimeType">the MimeType of the new Image</param>
        /// <returns>a new Image</returns>
        public static Image Create(byte[] bytes, string mimeType) {
            Image image = null;
            string extension, fileName, destPath, resultcode = null;

            using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                Entity entity = Entity.Create(EntityType.Image, string.Empty, string.Empty);

                extension = Common.MimeType.LookupExtension(mimeType);
                fileName = string.Format("{0}.{1}", entity.Id, extension);
                destPath = string.Format(@"{0}\{1}", Settings.GetSetting("ImageDirectory"), fileName);

                File.WriteAllBytes(destPath, bytes);
                Exif exif = new Exif(destPath);

                db.xspUpdateImage(entity.Id, fileName, mimeType, exif.Resolution.X, exif.Resolution.Y, ref resultcode);

                if (resultcode == "SUCCESS") {
                    image = new Image(entity, destPath, mimeType, exif);
                    exif.SaveToEntity(entity.Id);
                }
                else
                    throw new ObscuraException(string.Format("Unable to create Image. ({0})", resultcode));

            }

            return image;
        }

        public static byte[] ResizeImage(byte[] image, int targetSize) {
            byte[] resized;
            int targetW, targetH;
            float scaler;
            
            System.Drawing.Image original =  System.Drawing.Image.FromStream(new MemoryStream(image));

            if (original.Width >= original.Height && original.Width > targetSize)
                scaler = (float)targetSize / (float)original.Width;
            else if (original.Height > original.Width && original.Height > targetSize)
                scaler = (float)targetSize / (float)original.Height;
            else
                scaler = 1;

            if(scaler != 1){
                targetW = (int)(original.Width * scaler);
                targetH = (int)(original.Height * scaler);

                Bitmap bmp = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
                bmp.SetResolution(original.HorizontalResolution, original.VerticalResolution);

                Graphics grp = Graphics.FromImage(bmp);
                grp.SmoothingMode = SmoothingMode.HighSpeed;
                grp.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grp.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                grp.DrawImage(original, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Jpeg);
                original.Dispose();
                bmp.Dispose();
                grp.Dispose();

                resized = ms.GetBuffer();
            }
            else
                resized = image;

            return resized;
        }
    }
}
