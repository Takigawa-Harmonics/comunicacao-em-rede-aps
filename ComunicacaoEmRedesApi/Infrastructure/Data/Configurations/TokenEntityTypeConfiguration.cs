using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Infrastructure.Data.Configurations.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations;

public class TokenEntityTypeConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable(DefaultSchemaProperties.TableNames.TokenSessionTable);

        builder.HasKey(e => e.UserId);

        builder.Property(e => e.UserId)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.UserIdColumnName)
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.TokenValueColumnName)
            .HasMaxLength(DefaultSchemaProperties.ColumnProperties.TokenValueMaxLength)
            .IsRequired();

        builder.Property(e => e.Expiration)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.ExpirationColumnName)
            .IsRequired();

        builder.Property(e => e.IsRevoked)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.IsRevokedColumnName)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Token>(e => e.UserId)
            .HasPrincipalKey<User>(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}