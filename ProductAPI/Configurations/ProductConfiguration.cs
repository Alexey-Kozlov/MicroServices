using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Domain;
using System.Diagnostics;

namespace ProductAPI.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("product").HasKey(p => p.Id).HasName("PK_Product_Id");
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasColumnName("name").IsRequired(true);
            builder.Property(p => p.Price).HasColumnName("price").IsRequired(true);
            builder.Property(p => p.Description).HasColumnName("description").IsRequired(false);
            builder.Property(p => p.CategoryId).HasColumnName("categoryId").IsRequired(true);
            builder.Property(p => p.ImageId).HasColumnName("imageId").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_Product_Id");
            builder.HasIndex(p => p.Name).HasDatabaseName("IX_Product_Name");
            builder.HasIndex(p => p.CategoryId).HasDatabaseName("IX_Product_CategoryId");
            builder.HasIndex(p => p.ImageId).HasDatabaseName("IX_Product_ImageId");
        }
    }
}
