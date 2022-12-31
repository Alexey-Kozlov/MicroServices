using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersAPI.Domain;

namespace OrdersAPI.Configurations
{
    public class ProductRefConfiguration : IEntityTypeConfiguration<ProductRef>
    {
        public void Configure(EntityTypeBuilder<ProductRef> builder)
        {
            builder.ToTable("ProductRef").HasKey(p => p.Id).HasName("PK_ProductRef_Key");
            builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(p => p.Quantity).HasColumnName("Quantity");
            builder.HasOne(p => p.Order).WithMany(p => p.Products).HasForeignKey(p => p.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_ProductRef_Id");
            builder.HasIndex(p => p.OrderId).HasDatabaseName("IX_ProductRef_OrderId");
            builder.HasIndex(p => p.ProductId).HasDatabaseName("IX_ProductRef_ProductId");
        }
    }
}
