using ComunicacaoEmRedesApi.Domain.Models;

namespace ComunicacaoEmRedesApi.Domain.Services.Interfaces;

public interface ITokenService
{
    Task<Token> ManageTokenCreationFlow(Guid userId);
    Task SetTokenAsRevoked(Guid userId);
}