using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersAPI.Domain;

namespace OrdersAPI.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order").HasKey(p => p.Id).HasName("PK_Order_Id");
            builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(p => p.OrderDate).HasColumnName("OrderDate").IsRequired(true);
            builder.Property(p => p.Description).HasColumnName("Description").IsRequired(false);
            builder.Property(p => p.UserId).HasColumnName("UserId").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_Order_Id");
            builder.HasIndex(p => p.OrderDate).HasDatabaseName("IX_Order_Date");
            builder.HasIndex(p => p.UserId).HasDatabaseName("IX_Order_UserId");
        }
    }
}
