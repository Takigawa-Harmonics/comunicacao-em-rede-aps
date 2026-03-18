using ComunicacaoEmRedesApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(e => e.Messages)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id);

        builder.HasMany(e => e.Chats)
            .WithMany(e => e.Users);

        builder.Property(e => e.Email)
            .HasMaxLength(252)
            .IsRequired();

        builder.Property(e => e.PasswordHash)
            .HasMaxLength(60)
            .IsRequired();
    }
}