using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Infrastructure.Data.Configurations.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(DefaultSchemaProperties.TableNames.MessageTable);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.IdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.HasOne(e => e.Chat)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.ChatId)
            .HasPrincipalKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id);

        builder.Property(e => e.UserId)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.UserIdColumnName)
            .IsRequired();
        
        builder.Property(e => e.ChatId)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.ChatIdColumnName)
            .IsRequired();
        
        builder.Property(e => e.Content)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.ContentColumnName)
            .HasMaxLength(DefaultSchemaProperties.ColumnProperties.ContentMaxLength)
            .IsRequired();
    }
}