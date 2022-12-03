using Microsoft.EntityFrameworkCore;
using ProductAPI.Configurations;
using ProductAPI.Domain;
using System.Diagnostics;

namespace ProductAPI.Persistance
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
