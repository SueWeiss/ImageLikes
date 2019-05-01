using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Library.Data
{
    public class Manager
    {
        private string _connectionString;
        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Image> GetImages()
        {
            using (var context = new ImageContext(_connectionString))
            {
                return context.Images.ToList().OrderByDescending(d=>d.DateAdded);
            }
        }
        public void InsertImage(Image image)
        {
            using (var context = new ImageContext(_connectionString))
            {
                context.Images.Add(image);
                context.SaveChanges();
            }
        }
        public Image GetImageById(int id)
        {
            using (var context = new ImageContext(_connectionString))
            {
                return  context.Images.FirstOrDefault(p => p.Id == id);
                
            }
        }
        public void SetLike(int id)
        {
            using (var context = new ImageContext(_connectionString))
            {
                var image= context.Images.FirstOrDefault(p => p.Id == id);
                image.Likes++;
                context.SaveChanges();

            }
        }
        public int GetLike(int id)
        {
            using (var context = new ImageContext(_connectionString))
            {
                var image = context.Images.FirstOrDefault(p => p.Id == id);
               return image.Likes;

            }
        }

    }
    public class Image
    {
        public string ImageName { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
    }


    public class ImageContextFactory : IDesignTimeDbContextFactory<ImageContext>
    {
        public ImageContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}4_29ImageLikes"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new ImageContext(config.GetConnectionString("ConStr"));
        }
    }




    public class ImageContext : DbContext
    {
        private string _connectionString;
        public ImageContext(string connectionString)
        {
            _connectionString = connectionString;
        }


        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
         .UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=ImagesEF;Integrated Security=True");
        }
    }
}
