using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGallery
{
    [Index(nameof(Album.Name), IsUnique = true)]
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Image> Images { get; set; } = null!;
    }
}
