using Microsoft.EntityFrameworkCore;

namespace ComunicacaoEmRedesApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

}