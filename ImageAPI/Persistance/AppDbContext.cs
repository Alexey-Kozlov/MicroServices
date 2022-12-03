using ImageAPI.Domain;
using Microsoft.EntityFrameworkCore;
using ImageAPI.Configurations;

namespace ImageAPI.Persistance
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }
        public DbSet<Image> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ImageConfiguration());
        }
    }
}
