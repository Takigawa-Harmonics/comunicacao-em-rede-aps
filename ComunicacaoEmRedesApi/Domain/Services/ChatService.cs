using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;

namespace ComunicacaoEmRedesApi.Domain.Services;

public class ChatService : IChatService
{
    private readonly ConcurrentDictionary<Guid, WebSocket> _clients = new();
    private readonly ConcurrentDictionary<Guid, List<Guid>> _chatRooms = new();

    public Task AddClientAsync(Guid userId, WebSocket socket)
    {
        _clients[userId] = socket;
        return Task.CompletedTask;
    }

    public Task RemoveClientAsync(Guid userId)
    {
        _clients.TryRemove(userId, out _);
        return Task.CompletedTask;
    }

    public async Task SendMessageToChatAsync(Guid chatId, string message, Guid senderId)
    {
        if (!_chatRooms.TryGetValue(chatId, out var users))
            return;

        var bytes = Encoding.UTF8.GetBytes(message);

        foreach (var userId in users)
        {
            if (_clients.TryGetValue(userId, out var socket) &&
                socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    public void AddUserToChat(Guid chatId, Guid userId)
    {
        _chatRooms.AddOrUpdate(chatId,
            _ => new List<Guid> { userId },
            (_, list) =>
            {
                if (!list.Contains(userId))
                    list.Add(userId);
                return list;
            });
    }
}