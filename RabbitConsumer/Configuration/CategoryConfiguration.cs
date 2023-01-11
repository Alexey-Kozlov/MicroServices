using RabbitConsumer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabbitConsumer.Configurations
{
    public class LogMessageConfiguration : IEntityTypeConfiguration<LogMessage>
    {
        public void Configure(EntityTypeBuilder<LogMessage> builder)
        {
            builder.ToTable("LogMessage").HasKey(p => p.Id).HasName("PK_LogMessage_id");
            builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(p => p.ActionName).HasColumnName("ActionName").IsRequired(true);
            builder.Property(p => p.TypeName).HasColumnName("TypeName").IsRequired(true);
            builder.Property(p => p.TypeData).HasColumnName("TypeData").IsRequired(true);
            builder.Property(p => p.UserName).HasColumnName("UserName").IsRequired(true);
            builder.Property(p => p.Date).HasColumnName("Date").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_LogMessage_Id");

        }
    }
}
