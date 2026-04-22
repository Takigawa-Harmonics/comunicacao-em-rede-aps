using System.Net.WebSockets;

namespace ComunicacaoEmRedesApi.Domain.Services.Interfaces;

public interface IChatService
{
    Task AddClientAsync(Guid userId, WebSocket socket);
    Task RemoveClientAsync(Guid userId);
    Task SendMessageToChatAsync(Guid chatId, string message, Guid senderId);
}