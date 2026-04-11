using ComunicacaoEmRedesApi.Domain.Models;
using LanguageExt;

namespace ComunicacaoEmRedesApi.Domain.Repositories;

public interface IUserRepository
{
    Task SaveUserAsync(User user);
    Task<bool> DoesEmailExists(string email);
    Task<Option<User>> GetUserByEmailAsync(string email);
    Task<Option<User>> GetProfileByIdAsync(Guid userId);
}