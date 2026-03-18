using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Infrastructure.Data.Configurations.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(DefaultSchemaProperties.TableNames.UserTable);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.IdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.HasMany(e => e.Messages)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id);

        builder.HasMany(e => e.Chats)
            .WithMany(e => e.Users);

        builder.Property(e => e.Email)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.EmailColumnName)
            .HasMaxLength(DefaultSchemaProperties.ColumnProperties.EmailMaxLength)
            .IsRequired();

        builder.Property(e => e.PasswordHash)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.PasswordColumnName)
            .HasMaxLength(DefaultSchemaProperties.ColumnProperties.PasswordHashMaxLength)
            .IsRequired();

        builder.Property(e => e.Active)
            .HasColumnName(DefaultSchemaProperties.ColumnNames.ActiveColumnName)
            .HasDefaultValue(false);
    }
}