using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using ComunicacaoEmRedesApi.Infrastructure.Data;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace ComunicacaoEmRedesApi.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ApplicationDbContext _context;

    public ChatRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveChatAsync(Chat chat)
    {
        await _context.Chats.AddAsync(chat);
    }

    public async Task<Option<Chat>> GetChatByIdAsync(Guid id)
    {
        var chat = await _context.Chats
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        return chat is null ? Option<Chat>.None : Option<Chat>.Some(chat);
    }
}