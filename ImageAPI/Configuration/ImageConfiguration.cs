using ImageAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageAPI.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Image").HasKey(p => p.Id).HasName("PK_Image_id");
            builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired(true);
            builder.Property(p => p.Size).HasColumnName("Size").IsRequired(true);
            builder.Property(p => p.Data).HasColumnName("Data").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_Image_Id");

        }
    }
}
