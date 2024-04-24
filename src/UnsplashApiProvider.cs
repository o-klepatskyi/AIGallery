using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIGallery
{
    public sealed class UnsplashApiProvider : IApiImageProvider
    {

        private class ImageUrls
        {
            [JsonPropertyName("raw")]
            public string Raw { get; set; }

            [JsonPropertyName("thumb")]
            public string Thumb { get; set; }
        }

        private class ImageResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("urls")]
            public ImageUrls ImageUrls { get; set; }
        }

        private class SearchResponse
        {
            [JsonPropertyName("results")]
            public List<ImageResponse> Results { get; set; }
        }

        public override string Name
        {
            get => "unsplash";
        }

        public async override Task<ImageDto> ProcessQuery(string query)
        {
            var baseUrl = "https://api.unsplash.com/search/photos";
            var url = $"{baseUrl}?query={Uri.EscapeDataString(query)}&client_id={ApiKey}";
            
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Trace.WriteLine(responseBody);
            var searchResponse = JsonSerializer.Deserialize<SearchResponse>(responseBody);

            if (searchResponse is null || !searchResponse.Results.Any())
            {
                throw new Exception("Empty response");
            }

            var imageResponse = searchResponse.Results.First();
            var rawImage = await getImage(imageResponse.ImageUrls.Raw);
            var thumbnailImage = await getImage(imageResponse.ImageUrls.Thumb);
            return new ImageDto {
                ImageData=rawImage,
                ThumbnailData= thumbnailImage,
                CreatedAt = DateTime.Now,
                ImageProvider = Name
            };
        }
    }
}
