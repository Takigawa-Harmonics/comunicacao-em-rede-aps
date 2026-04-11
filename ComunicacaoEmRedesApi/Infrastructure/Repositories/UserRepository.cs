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
        await _context.SaveChangesAsync(); // Change this asap
    }

    public async Task<bool> DoesEmailExists(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(e => e.Email == email);
    }

    public async Task<Option<User>> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Email == email);

        return user is null ? Option<User>.None : Option<User>.Some(user);
    }

    public async Task<Option<User>> GetProfileByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(e => e.Chats)
            .FirstOrDefaultAsync(e => e.Id == userId);

        return user is null ? Option<User>.None : Option<User>.Some(user);
    }
}