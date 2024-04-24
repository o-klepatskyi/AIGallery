using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGallery
{
    public class GenerateViewModel : ObservableObject
    {
        private IApiImageProvider? ActiveProvider { get; set; }

        public List<string> GetProviderNames()
        {
            using (var context = new AppDBContext())
            {
                // We return only providers with api key set
                // Otherwise it will result in errors during query
                return context.ImageProviders
                    .Where(p => p.ApiKey.Length > 0)
                    .Select(e => e.Name).ToList();
            }
        }

        public void SetActiveProvider(string name)
        {
            ActiveProvider = Providers.First(e => e.Name == name);
        }

        public string? GetActiveProviderName()
        {
            return ActiveProvider?.Name;
        }

        public Task<ImageDto> ProcessQuery(string query)
        {
            if (ActiveProvider is null)
                return Task.FromException<ImageDto>(new Exception("Provider is not set. Please configure API keys and set active provider."));
            return ActiveProvider.ProcessQuery(query);
        }

        private static List<IApiImageProvider> Providers = new() {
            new UnsplashApiProvider()
        };

        private IApiImageProvider? ProviderByName(string name)
        {
            return Providers.FirstOrDefault(p => p.Name == name);
        }
    }
}
