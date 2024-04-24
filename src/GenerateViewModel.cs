using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AIGallery
{
    public class GenerateViewModel : ObservableObject
    {
        private IApiImageProvider? _activeProvider;
        private BitmapImage? _displayedImage;
        private List<string> _providerNames;

        public GenerateViewModel()
        {
            ProcessQueryCommand = new RelayCommand<string>(ProcessQuery, CanProcessQuery);
            InitializeProviders();
        }

        public RelayCommand<string> ProcessQueryCommand { get; }

        public BitmapImage? DisplayedImage
        {
            get => _displayedImage;
            private set => SetProperty(ref _displayedImage, value);
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
                    var result = await _activeProvider.ProcessQuery(query);
                    if (result?.ImageData != null)
                    {
                        using (var ms = new MemoryStream(result.ImageData))
                        {
                            var image = new BitmapImage();
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();
                            image.Freeze(); // Necessary for UI thread usage
                            DisplayedImage = image;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            }
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
    }
}
