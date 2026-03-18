using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Infrastructure.Data.Configurations.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations;

public class ChatEntityTypeConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable(DefaultSchemaProperties.TableNames.ChatTable);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.IdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasMany(e => e.Users)
            .WithMany(e => e.Chats)
            .UsingEntity(e => e.ToTable(DefaultSchemaProperties.TableNames.UserChatTable));
        
        builder.HasMany(e => e.Messages)
            .WithOne(e => e.Chat)
            .HasForeignKey(e => e.ChatId)
            .HasPrincipalKey(e => e.Id);

        builder.Property(e => e.Active)
            .HasDefaultValue(true)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.ActiveColumnName);
        
        builder.Property(e => e.Title)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.TitleColumnName)
            .HasMaxLength(DefaultSchemaProperties.ColumnProperties.TitleMaxLength)
            .IsRequired();
    }
}