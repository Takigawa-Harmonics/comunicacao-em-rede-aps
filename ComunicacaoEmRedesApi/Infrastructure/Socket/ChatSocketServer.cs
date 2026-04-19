using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ComunicacaoEmRedesApi.Domain.Models;
using ComunicacaoEmRedesApi.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComunicacaoEmRedesApi.Infrastructure.Socket;

// Representa um cliente conectado
public class ConnectedClient
{
    public TcpClient TcpClient { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Email { get; set; } = "";
    public Guid ChatId { get; set; }
    public NetworkStream Stream => TcpClient.GetStream();
}

public class ChatSocketServer : BackgroundService
{
    // Dicionário de clientes conectados: clientId -> ConnectedClient
    private static readonly ConcurrentDictionary<Guid, ConnectedClient> _clients = new();

    private readonly IServiceScopeFactory _scopeFactory;
    private TcpListener? _listener;

    public ChatSocketServer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener = new TcpListener(IPAddress.Any, 5002);
        _listener.Start();
        Console.WriteLine("[Socket] Servidor TCP rodando na porta 5002");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var tcpClient = await _listener.AcceptTcpClientAsync(stoppingToken);
                Console.WriteLine("[Socket] Novo cliente conectado");
                _ = HandleClientAsync(tcpClient, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _listener.Stop();
    }

    private async Task HandleClientAsync(TcpClient tcpClient, CancellationToken ct)
    {
        var clientId = Guid.NewGuid();
        var stream = tcpClient.GetStream();
        var buffer = new byte[4096];

        try
        {
            // Primeira mensagem: autenticação
            // Cliente deve mandar: {"userId":"...","email":"...","chatId":"..."}
            int bytesRead = await stream.ReadAsync(buffer, ct);
            if (bytesRead == 0) return;

            var authJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            var auth = JsonSerializer.Deserialize<AuthPayload>(authJson);

            if (auth is null || auth.UserId == Guid.Empty || auth.ChatId == Guid.Empty)
            {
                await SendAsync(stream, "ERRO: autenticação inválida", ct);
                tcpClient.Close();
                return;
            }

            // Registra cliente
            var client = new ConnectedClient
            {
                TcpClient = tcpClient,
                UserId = auth.UserId,
                Email = auth.Email,
                ChatId = auth.ChatId
            };
            _clients[clientId] = client;

            Console.WriteLine($"[Socket] {auth.Email} entrou no chat {auth.ChatId}");
            await BroadcastAsync(auth.ChatId, $"{auth.Email} entrou no chat.", clientId, ct);

            // Loop de mensagens
            while (!ct.IsCancellationRequested)
            {
                bytesRead = await stream.ReadAsync(buffer, ct);
                if (bytesRead == 0) break;

                var content = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"[Socket] {auth.Email}: {content}");

                // Salva no banco
                await SaveMessageAsync(auth.UserId, auth.ChatId, content);

                // Broadcast pra todos no mesmo chat
                var msg = $"{auth.Email}: {content}";
                await BroadcastAsync(auth.ChatId, msg, null, ct);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Socket] Erro no cliente {clientId}: {ex.Message}");
        }
        finally
        {
            if (_clients.TryRemove(clientId, out var removed))
            {
                await BroadcastAsync(removed.ChatId, $"{removed.Email} saiu do chat.", clientId, ct);
            }
            tcpClient.Close();
        }
    }

    // Envia mensagem para todos no mesmo chatId (exceto o remetente se excludeId != null)
    private static async Task BroadcastAsync(Guid chatId, string message, Guid? excludeClientId, CancellationToken ct)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        foreach (var (id, client) in _clients)
        {
            if (client.ChatId != chatId) continue;
            if (excludeClientId.HasValue && id == excludeClientId.Value) continue;

            try
            {
                await client.Stream.WriteAsync(bytes, ct);
            }
            catch
            {
                // cliente desconectado, ignora
            }
        }
    }

    private static async Task SendAsync(NetworkStream stream, string message, CancellationToken ct)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(bytes, ct);
    }

    private async Task SaveMessageAsync(Guid userId, Guid chatId, string content)
    {
        using var scope = _scopeFactory.CreateScope();
        var messageRepo = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
        var chatRepo = scope.ServiceProvider.GetRequiredService<IChatRepository>();

        var chatOption = await chatRepo.GetChatByIdAsync(chatId);
        chatOption.IfSome(async _ =>
        {
            var message = new Message
            {
                Content = content,
                Active = true,
                ChatId = chatId,
                UserId = userId
            };
            await messageRepo.SaveMessageAsync(message);
        });
    }

    private record AuthPayload(Guid UserId, string Email, Guid ChatId);
}