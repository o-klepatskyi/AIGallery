using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AIGallery
{

    public abstract class IApiImageProvider
    {
        protected static readonly HttpClient httpClient = new HttpClient();
        public abstract string Name { get; }
        protected string ApiKey {
            get
            {
                using (var context = new AppDBContext())
                {
                    return context.ImageProviders
                                    .Where(p => p.Name == Name)
                                    .Select(p => p.ApiKey)
                                    .First();
                }
            }
        }
        public abstract Task<ImageDto> ProcessQuery(string query);

        protected static async Task<byte[]> getImage(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var contentType = response.Content.Headers.ContentType;
            if (!contentType?.MediaType?.StartsWith("image/") ?? true)
            {
                throw new Exception("The content is NOT an image.");
            }
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
