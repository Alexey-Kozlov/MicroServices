using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersAPI.Domain;

namespace OrdersAPI.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("order").HasKey(p => p.Id).HasName("PK_Order_Id");
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.OrderDate).HasColumnName("orderDate").IsRequired(true);
            builder.Property(p => p.Description).HasColumnName("description").IsRequired(false);
            builder.Property(p => p.UserId).HasColumnName("userId").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_Order_Id");
            builder.HasIndex(p => p.OrderDate).HasDatabaseName("IX_Order_Date");
            builder.HasIndex(p => p.UserId).HasDatabaseName("IX_Order_UserId");
        }
    }
}
