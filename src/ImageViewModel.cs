using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AIGallery
{
    public class ImageViewModel : ObservableObject
    {
        private Image _image;
        public event EventHandler ImageDeleted;
        public ICommand DeleteImageCommand { get; }

        public Image CurrentImage
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public ImageViewModel(Image image)
        {
            CurrentImage = image;
            DeleteImageCommand = new RelayCommand(DeleteImage);
        }

        private void DeleteImage()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this image?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new AppDBContext())
                    {
                        context.Images.Remove(_image);
                        context.SaveChanges();
                    }
                    OnImageDeleted();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex}", "Image was not deleted", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        protected virtual void OnImageDeleted()
        {
            ImageDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
