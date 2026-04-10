using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;
using ComunicacaoEmRedesApi.Infrastructure.Security;

namespace ComunicacaoEmRedesApi.Domain.Services;

public class TokenService : ITokenService
{
    private readonly ITokenRepository _tokenRepository;

    public TokenService(ITokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }
    
    public async Task<Token> ManageTokenCreationFlow(Guid userId)
    {
        var searchForToken = await _tokenRepository.GetTokenByUserId(userId);
        
        if (searchForToken.IsNone)
        {
            return await RegisterNewTokenForSession(userId);
        }
         
        if (searchForToken.IsSome)
        {
            var token = searchForToken.First();
            
            if (token.Expiration < DateTime.UtcNow || token.IsRevoked)
            {
                await DeleteTokenFromOldSession(userId);
            }
            else
            {
                return token;
            }
        }
        
        return await RegisterNewTokenForSession(userId);
    }

    public async Task SetTokenAsRevoked(Guid userId)
    {
        await _tokenRepository.RevokeTokenByUserId(userId);
    }

    private async Task<Token> RegisterNewTokenForSession(Guid userId)
    {
        var value = SessionTokenGenerator.Generate();
        var token = Token.Get(userId, value);
        
        await _tokenRepository.SaveTokenAsync(token);
        
        return token;
    }

    private async Task DeleteTokenFromOldSession(Guid userId)
    {
        await _tokenRepository.DeleteTokenByUserId(userId);
    }
}