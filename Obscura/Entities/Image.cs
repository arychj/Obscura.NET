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
        private Exif _exif;
        private string _filePath, _mimeType, _url = null, _html = null;

        #region accessors

        public byte[] Bytes {
            get { return File.ReadAllBytes(_filePath); }
        }

        public string MimeType {
            get { return _mimeType; }
        }

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

        public string Html {
            get {
                if(_html == null)
                    _html = string.Format("<img src = \"{0}\" alt = \"{1}\"/>", Url, base.Title);

                return _html;
            }
        }

        public Exif Exif {
            get {
                //TODO: get exif
                return new Exif(FilePath);
            }
        }

        public Resolution Resolution {
            get { return _resolution; }
        }

        internal string FilePath {
            get { return string.Format("{0}/{1}", Settings.GetSetting("ImageDirectory"), _filePath); }
        }

        #endregion

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

        internal Image(Entity entity, string path, string mimeType, Resolution resolution)
            : base(entity) {
                _filePath = path;
                _mimeType = mimeType;
                _resolution = resolution;
        }

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

        public static Image Create(string originalPath) {
            //TODO: create
            Image image = null;
            string extension, newPath, mimeType, resultcode = null;

            if (File.Exists(originalPath)) {
                using (ObscuraLinqDataContext db = new ObscuraLinqDataContext(Config.ConnectionString)) {
                    Entity entity = Entity.Create(EntityType.Image, string.Empty, string.Empty);

                    extension = Path.GetExtension(originalPath).TrimStart('.');
                    mimeType = Common.MimeType.ParseExtension(extension);
                    newPath = string.Format(@"{0}\{1}.{2}", Settings.GetSetting("ImageDirectory"), entity.Id, extension);

                    File.Move(originalPath, newPath);
                    Exif exif = new Exif(newPath);

                    db.xspUpdateImage(entity.Id, newPath, exif.Resolution.X, exif.Resolution.Y, ref resultcode);

                    if (resultcode == "SUCCESS") {
                        image = new Image(entity, newPath, mimeType, exif.Resolution);
                    }
                    else
                        throw new ObscuraException(string.Format("Unable to create Image. ({0})", resultcode));

                }
            }
            else
                throw new ObscuraException(string.Format("Unable to create Image from '{0}'. File does not exist.", originalPath));

            return image;
        }

        public static Image Create(byte[] bytes) {
            //TODO: create
            return null;
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
