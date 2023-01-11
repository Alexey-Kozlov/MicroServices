using RabbitConsumer.Domain;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Configurations;

namespace RabbitConsumer.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<LogMessage> LogMessage { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new LogMessageConfiguration());
        }
    }
}
