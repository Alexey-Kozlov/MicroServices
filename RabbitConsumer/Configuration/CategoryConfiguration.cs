using RabbitConsumer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabbitConsumer.Configurations
{
    public class LogMessageConfiguration : IEntityTypeConfiguration<LogMessage>
    {
        public void Configure(EntityTypeBuilder<LogMessage> builder)
        {
            builder.ToTable("logmessage").HasKey(p => p.Id).HasName("PK_LogMessage_id");
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.ActionName).HasColumnName("actionName").IsRequired(true);
            builder.Property(p => p.TypeName).HasColumnName("typeName").IsRequired(true);
            builder.Property(p => p.TypeData).HasColumnName("typeData").IsRequired(true);
            builder.Property(p => p.UserName).HasColumnName("userName").IsRequired(true);
            builder.Property(p => p.Date).HasColumnName("date").IsRequired(true);
            builder.HasIndex(p => p.Id).HasDatabaseName("IX_LogMessage_Id");

        }
    }
}
