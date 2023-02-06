using ImageAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageAPI.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("image").HasKey(p => p.Id).HasName("PK_Image_id");
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasColumnName("name").IsRequired(true);
            builder.Property(p => p.Size).HasColumnName("size").IsRequired(true);
            builder.Property(p => p.Data).HasColumnName("data").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_Image_Id");

        }
    }
}
