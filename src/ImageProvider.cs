using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AIGallery;


namespace AIGallery
{
    [Index(nameof(ImageProvider.Name), IsUnique = true)]
    public class ImageProvider
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public ICollection<Image> Images { get; set; } = null!;
    }
}
