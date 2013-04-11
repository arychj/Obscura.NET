using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Common {
    public class MimeType {
        public static string ParseExtension(string extension) {
            switch (extension) {
                case "bmp": return "image/bmp";
                case "cr2": return "image/x-canon-cr2";
                case "gif": return "image/gif";
                case "jpeg": return "image/jpeg";
                case "jpg": return "image/jpeg";
                case "png": return "image/png";
                case "tiff": return "image/tiff";
                default: return "text/plain";
            }
        }
    }
}
