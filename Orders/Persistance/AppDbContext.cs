using Microsoft.EntityFrameworkCore;
using OrdersAPI.Configurations;
using OrdersAPI.Domain;

namespace OrdersAPI.Persistance
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }
        public DbSet<Order> Order { get; set; }
        public DbSet<ProductRef> ProductRef { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new ProductRefConfiguration());
        }
    }
}
