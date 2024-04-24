using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGallery
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; } = null!;
        public byte[] ThumbnailData { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public ImageProvider ImageProvider { get; set; } = null!;
        public ICollection<Album> Album { get; set; } = null!;
    }
}
