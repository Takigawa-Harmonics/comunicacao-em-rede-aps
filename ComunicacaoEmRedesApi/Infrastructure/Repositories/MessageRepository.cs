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
        await _context.SaveChangesAsync();
    }

    public async Task<Option<Message>> GetMessageByIdAsync(Guid id)
    {
        var message = await _context.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        return message is null ? Option<Message>.None : Option<Message>.Some(message);
    }

    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        return await _context.Messages
            .Include(e => e.User)
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
}