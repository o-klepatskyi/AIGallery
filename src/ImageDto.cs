using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGallery
{
    public class ImageDto
    {
        public byte[] ImageData { get; set; } = null!;
        public byte[] ThumbnailData { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string ImageProvider { get; set; } = null!;
    }
}
