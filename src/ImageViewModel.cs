using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ICommand SaveCommand { get; }

        public Image CurrentImage
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public ImageViewModel(Image image)
        {
            CurrentImage = image;
            DeleteImageCommand = new RelayCommand(DeleteImage);
            SaveCommand = new RelayCommand(SaveImage);
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

        private void SaveImage()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            saveFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                try
                {
                    File.WriteAllBytes(fileName, _image.ImageData);
                    MessageBox.Show("Image saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
