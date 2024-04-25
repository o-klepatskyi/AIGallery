using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AIGallery
{
    public class ImageItemViewModel : ObservableObject
    {
        public int Id { get; }
        public BitmapImage Thumbnail { get; }

        public ImageItemViewModel(int id, byte[] thumbnailData)
        {
            Id = id;
            Thumbnail = ConvertToBitmapImage(thumbnailData);
        }

        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var bitmapImage = new BitmapImage();
            using (var memoryStream = new MemoryStream(imageData))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
            bitmapImage.Freeze(); // Freeze the BitmapImage to prevent further modifications

            return bitmapImage;
        }
    }
}
