using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AIGallery
{
    public class GenerateViewModel : ObservableObject
    {
        private IApiImageProvider? _activeProvider;
        private BitmapImage? _displayedImage;
        private ImageDto? _displayedImageDto;
        private List<string> _providerNames;
        private bool _isImageDisplayed;

        public ICommand ProcessQueryCommand { get; }
        public ICommand SaveImageCommand { get; }

        public GenerateViewModel()
        {
            ProcessQueryCommand = new RelayCommand<string>(ProcessQuery, CanProcessQuery);
            SaveImageCommand = new RelayCommand(SaveImage);
            IsImageDisplayed = false;
            InitializeProviders();
        }

        public BitmapImage? DisplayedImage
        {
            get => _displayedImage;
            private set
            {
                SetProperty(ref _displayedImage, value);
                IsImageDisplayed = (value != null);
            }
        }

        public bool IsImageDisplayed
        {
            get => _isImageDisplayed;
            private set => SetProperty(ref _isImageDisplayed, value);
        }

        public List<string> ProviderNames
        {
            get => _providerNames;
            private set => SetProperty(ref _providerNames, value);
        }

        public string? SelectedProvider
        {
            set
            {
                _activeProvider = ProviderByName(value);
                OnPropertyChanged(nameof(CanProcessQuery));
            }
        }

        private bool CanProcessQuery(string query) => _activeProvider != null && !string.IsNullOrWhiteSpace(query);

        private async void ProcessQuery(string query)
        {
            if (CanProcessQuery(query))
            {
                try
                {
                    _displayedImageDto = await _activeProvider.ProcessQuery(query);
                    if (_displayedImageDto?.ImageData != null)
                    {
                        using (var ms = new MemoryStream(_displayedImageDto.ImageData))
                        {
                            var image = new BitmapImage();
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();
                            DisplayedImage = image;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorDialog(ex.Message);
                }
            }
        }

        private void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Image generation failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void InitializeProviders()
        {
            using (var context = new AppDBContext())
            {
                ProviderNames = context.ImageProviders
                    .Where(p => p.ApiKey.Length > 0)
                    .Select(e => e.Name).ToList();
            }
        }

        private IApiImageProvider? ProviderByName(string? name)
        {
            return Providers.FirstOrDefault(p => p.Name == name);
        }

        private static List<IApiImageProvider> Providers = new List<IApiImageProvider>
        {
            new UnsplashApiProvider(),
            new LocalImageProvider()
        };

        private async void SaveImage()
        {
            try
            {
                if (_displayedImageDto == null)
                {
                    ShowErrorDialog("No image to save.");
                    return;
                }

                using (var context = new AppDBContext())
                {
                    var imageProvider = context.ImageProviders.FirstOrDefault(p => p.Name == _displayedImageDto.ImageProvider);
                    if (imageProvider == null)
                    {
                        ShowErrorDialog("Image provider not found.");
                        return;
                    }
                    var imageEntity = new Image
                    {
                        ImageData = _displayedImageDto.ImageData,
                        ThumbnailData = _displayedImageDto.ThumbnailData,
                        CreatedAt = _displayedImageDto.CreatedAt,
                        ImageProvider = imageProvider
                    };
                    context.Images.Add(imageEntity);
                    await context.SaveChangesAsync();
                }

                MessageBox.Show("Image saved to gallery successfully.", "Image saved", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayedImage = null;
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Failed to save image to database: " + ex.Message);
            }
        }
    }
}
