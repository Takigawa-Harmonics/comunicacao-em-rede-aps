using ComunicacaoEmRedesApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations;

public class ChatEntityTypeConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasMany(e => e.Users)
            .WithMany(e => e.Chats);

        builder.HasMany(e => e.Messages)
            .WithOne(e => e.Chat)
            .HasForeignKey(e => e.ChatId)
            .HasPrincipalKey(e => e.Id);

        builder.Property(e => e.Title)
            .HasMaxLength(40)
            .IsRequired();
    }
}