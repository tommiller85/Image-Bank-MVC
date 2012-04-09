using System.Data.Entity;
using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class ImageBankContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(x => x.Images)
                .WithRequired(x => x.UploadedByUser)
                .HasForeignKey(x => x.UploadedByUsername);

            modelBuilder.Entity<Category>().HasMany(x => x.Images)
                .WithOptional(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}