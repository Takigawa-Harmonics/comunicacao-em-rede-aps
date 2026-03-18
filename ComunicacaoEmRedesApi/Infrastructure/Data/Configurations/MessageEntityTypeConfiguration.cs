using ComunicacaoEmRedesApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne(e => e.Chat)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.ChatId)
            .HasPrincipalKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id);

        builder.Property(e => e.Value)
            .HasMaxLength(2000)
            .IsRequired();
    }
}