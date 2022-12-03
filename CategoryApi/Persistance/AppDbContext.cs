using CategoryAPI.Domain;
using Microsoft.EntityFrameworkCore;
using CategoryAPI.Configurations;

namespace CategoryAPI.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
