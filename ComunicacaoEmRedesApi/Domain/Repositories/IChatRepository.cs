using ComunicacaoEmRedesApi.Domain.Models;
using LanguageExt;

namespace ComunicacaoEmRedesApi.Domain.Repositories;

public interface IChatRepository
{
    Task SaveChatAsync(Chat chat);
    Task<Option<Chat>> GetChatByIdAsync(Guid id);
}