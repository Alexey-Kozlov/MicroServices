using Identity.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Identity.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken").HasKey(p => p.Id).HasName("Id");
            builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(p => p.UserId).HasColumnName("UserId").HasColumnType("TEXT").IsRequired(true);
            builder.Property(p => p.Token).HasColumnName("Token").HasColumnType("TEXT").IsRequired(true);
            builder.Property(p => p.Expires).HasColumnType("DateTime").HasColumnName("Expires").IsRequired(true);
            builder.Property(p => p.Revoked).HasColumnType("DateTime").HasColumnName("Revoked").IsRequired(false);
            builder.HasOne(p => p.User).WithMany(p => p.RefreshTokens).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_RefreshToken_Id");
            builder.HasIndex(p => p.UserId).HasDatabaseName("IX_RefreshToken_UserId");
        }
    }
}
