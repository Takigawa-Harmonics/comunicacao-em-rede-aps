using ComunicacaoEmRedesApi.Domain.Models;
using LanguageExt;

namespace ComunicacaoEmRedesApi.Domain.Repositories;

public interface IMessageRepository
{
    Task SaveMessageAsync(Message message);
    Task<Option<Message>> GetMessageByIdAsync(Guid id);
}