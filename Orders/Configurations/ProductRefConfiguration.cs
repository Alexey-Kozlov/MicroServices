using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersAPI.Domain;

namespace OrdersAPI.Configurations
{
    public class ProductRefConfiguration : IEntityTypeConfiguration<ProductRef>
    {
        public void Configure(EntityTypeBuilder<ProductRef> builder)
        {
            builder.ToTable("ProductRef").HasKey(p => new { p.OrderId, p.ProductId }).HasName("PK_ProductRef_Key");
            builder.HasOne(p => p.Order).WithMany(p => p.ProductIdList).HasForeignKey(p => p.OrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(p => p.OrderId).HasDatabaseName("IX_ProductRef_OrderId");
            builder.HasIndex(p => p.ProductId).HasDatabaseName("IX_ProductRef_ProductId");
        }
    }
}
