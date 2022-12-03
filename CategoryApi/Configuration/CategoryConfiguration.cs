using CategoryAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CategoryAPI.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category").HasKey(p => p.Id).HasName("PK_Category_id");
            builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_Category_Id");

        }
    }
}
