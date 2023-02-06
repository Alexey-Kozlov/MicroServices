using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersAPI.Domain;

namespace OrdersAPI.Configurations
{
    public class ProductRefConfiguration : IEntityTypeConfiguration<ProductRef>
    {
        public void Configure(EntityTypeBuilder<ProductRef> builder)
        {
            builder.ToTable("productref").HasKey(p => p.Id).HasName("PK_ProductRef_Key");
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.Quantity).HasColumnName("quantity");
            builder.Property(p => p.OrderId).HasColumnName("orderId");
            builder.Property(p => p.ProductId).HasColumnName("productId");
            builder.HasOne(p => p.Order).WithMany(p => p.Products)
                .HasForeignKey(p => p.OrderId)
                .HasConstraintName("FK_ProductRef_Order_OrderId")
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_ProductRef_Id");
            builder.HasIndex(p => p.OrderId).HasDatabaseName("IX_ProductRef_OrderId");
            builder.HasIndex(p => p.ProductId).HasDatabaseName("IX_ProductRef_ProductId");
        }
    }
}
