using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Infrastructure.Data;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace ComunicacaoEmRedesApi.Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly ApplicationDbContext _context;

    public TokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveTokenAsync(Token token)
    {
        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<Option<Token>> GetTokenByUserId(Guid userId)
    {
        var token = await _context.Tokens
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
        
        return token is null ? Option<Token>.None : Option<Token>.Some(token);
    }

    public async Task DeleteTokenByUserId(Guid userId)
    {
        var token = await GetTokenByUserId(userId);
        if (token.IsSome)
        {
            _context.Tokens.Remove(token.First());
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task RevokeTokenByUserId(Guid userId)
    {
        var token = await GetTokenByUserId(userId);
        if (token.IsSome)
        {
            token.First().IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }
}