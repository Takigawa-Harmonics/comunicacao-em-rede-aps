namespace ComunicacaoEmRedesApi.Domain.Services.Interfaces;

public interface ITokenService
{
    Task ManageTokenCreationFlow(Guid userId);
}