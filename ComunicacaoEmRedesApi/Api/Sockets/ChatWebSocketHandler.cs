using ComunicacaoEmRedesApi.Domain.Services;
using ComunicacaoEmRedesApi.Domain.Services.Interfaces;

namespace ComunicacaoEmRedesApi.Api.Sockets;

using System.Net.WebSockets;
using System.Text;

public class ChatWebSocketHandler
{
    private readonly IChatService _chatService;

    public ChatWebSocketHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task HandleAsync(HttpContext context)
    {
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var buffer = new byte[4096];

        // receive auth
        var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);

        var auth = System.Text.Json.JsonSerializer.Deserialize<AuthDto>(json)!;

        await _chatService.AddClientAsync(auth.UserId, socket);
        ((ChatService)_chatService).AddUserToChat(auth.ChatId, auth.UserId);

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var receive = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (receive.MessageType == WebSocketMessageType.Close)
                    break;

                var message = Encoding.UTF8.GetString(buffer, 0, receive.Count);

                await _chatService.SendMessageToChatAsync(
                    auth.ChatId,
                    message,
                    auth.UserId
                );
            }
        }
        finally
        {
            await _chatService.RemoveClientAsync(auth.UserId);

            await socket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closed",
                CancellationToken.None
            );
        }
    }

    private record AuthDto(Guid UserId, string Email, Guid ChatId);
}