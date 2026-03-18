using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Infrastructure.Data;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace ComunicacaoEmRedesApi.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveMessageAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
    }

    public async Task<Option<Message>> GetMessageByIdAsync(Guid id)
    {
        var message = await _context.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        return message is null ? Option<Message>.None : Option<Message>.Some(message);
    }
}