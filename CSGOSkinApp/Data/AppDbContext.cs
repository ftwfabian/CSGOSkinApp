using Microsoft.EntityFrameworkCore;
using CSGOSkinApp.Entities;

namespace CSGOSkinApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {}

        public DbSet<Skin> Skins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // You can add additional configuration here if needed
            base.OnModelCreating(modelBuilder);
        }
    }
}