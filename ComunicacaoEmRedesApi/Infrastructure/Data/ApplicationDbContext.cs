using ComunicacaoEmRedesApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ComunicacaoEmRedesApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Token> Tokens { get; set; }
    
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}