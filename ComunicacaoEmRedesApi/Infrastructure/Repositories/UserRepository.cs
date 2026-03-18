using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Infrastructure.Data;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace ComunicacaoEmRedesApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<Option<User>> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        return user is null ? Option<User>.None : Option<User>.Some(user);
    }
}