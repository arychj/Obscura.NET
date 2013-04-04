using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Images {
    public class Image {
        private int _id;
        private Image _thumbnail;
        private Resolution _resolution;
        private Exif _exif;
        private string _path;

        #region accessors

        public Image Thumbnail {
            get {
                return new Image(0);
                //TODO: get thumbnail
            }
        }

        public byte[] Display {
            get {
                return new byte[0];
                //TODO: get image
            }
        }

        public Exif Exif {
            get {
                return null;
                //TODO: get exif
            }
        }

        public Resolution Resolution {
            get { return _resolution; }
        }

        #endregion

        public Image(int id) {
            _id = id;
            //TODO: constructor
        }

        public byte[] GetBytes() {
            //TODO: GetBytes()
            return new byte[0];
        }

        public string GetHtml() {
            //TODO: GetHtml()
            return null;
        }

        private static byte[] GetImageContents(string path) {
            //TODO: get image contents
            return new byte[0];
        }

        public static Image Create() {
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
