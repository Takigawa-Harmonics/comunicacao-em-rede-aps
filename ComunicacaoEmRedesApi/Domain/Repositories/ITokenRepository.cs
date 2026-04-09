using ComunicacaoEmRedesApi.Domain.Models;
using LanguageExt;

namespace ComunicacaoEmRedesApi.Domain.Repositories;

public interface ITokenRepository
{
    Task SaveTokenAsync(Token token);
    Task<Option<Token>> GetTokenByUserId(Guid userId);
    Task DeleteTokenByUserId(Guid userId);
    Task RevokeTokenByUserId(Guid userId);
}