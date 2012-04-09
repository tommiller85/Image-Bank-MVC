using System.Data.Entity;
using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class ImageBankContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}