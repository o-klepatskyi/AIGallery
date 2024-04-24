using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using StdImage = System.Drawing.Image;
namespace AIGallery
{
    public class LocalImageProvider : IApiImageProvider
    {
        public override string Name => "local";

        private string _imageDir;

        public LocalImageProvider()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            _imageDir = Path.Join(Path.Join(path, "AIGallery"), "images");
        }

        public async override Task<ImageDto> ProcessQuery(string query)
        {
            if (!Directory.Exists(_imageDir))
                throw new Exception("Directory not found.");
            if (query.Contains("err"))
                throw new Exception("There was an error");
            var imageFiles = Directory.GetFiles(_imageDir, "*.*", SearchOption.TopDirectoryOnly)
                                      .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"))
                                      .ToList();

            if (imageFiles.Count == 0)
                throw new Exception("No image files found in the directory.");

            var random = new Random();
            var selectedFile = imageFiles[random.Next(imageFiles.Count)];

            var image = StdImage.FromFile(selectedFile);

            return new ImageDto {
                ImageData = ToByteArray(image),
                ThumbnailData = ToByteArray(ToThumbnail(image)),
                CreatedAt = DateTime.Now,
                ImageProvider = Name
            };
        }

        private StdImage ToThumbnail(StdImage originalImage)
        {
            int thumbnailWidth = originalImage.Width / 4;
            int thumbnailHeight = originalImage.Height / 4;

            Bitmap thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight);

            using (Graphics graphics = Graphics.FromImage(thumbnail))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(originalImage, 0, 0, thumbnailWidth, thumbnailHeight);
            }

            return thumbnail;
        }

        private byte[] ToByteArray(StdImage image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
