using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AIGallery
{
    public class AppDBContext : DbContext
    {
        public DbSet<ImageProvider> ImageProviders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Album> Albums { get; set; }

        public string DbPath { get; }

        public AppDBContext()
        {
            var folder = Environment.CurrentDirectory;
            var project = Directory.GetParent(folder).Parent.Parent.Parent.FullName;
            var dbFolder = Path.Combine(project, "data");
            Trace.WriteLine(dbFolder);
            Directory.CreateDirectory(dbFolder);
            DbPath = Path.Join(dbFolder, "aigallery.db");
            Trace.WriteLine($"DbPath: {DbPath}");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
