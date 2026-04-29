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
        var buffer = new byte[65536];

        try
        {
            int bytesRead = await stream.ReadAsync(buffer, ct);
            if (bytesRead == 0) return;

            var authJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            var auth = JsonSerializer.Deserialize<AuthPayload>(authJson);

            if (auth is null || auth.UserId == Guid.Empty || auth.ChatId == Guid.Empty)
            {
                await SendTextAsync(stream, "ERRO: autenticação inválida", ct);
                tcpClient.Close();
                return;
            }

            var client = new ConnectedClient
            {
                TcpClient = tcpClient,
                UserId = auth.UserId,
                Email = auth.Email,
                ChatId = auth.ChatId
            };
            _clients[clientId] = client;

            Console.WriteLine($"[Socket] {auth.Email} entrou no chat {auth.ChatId}");
            await BroadcastTextAsync(auth.ChatId, $"{auth.Email} entrou no chat.", clientId, ct);

            while (!ct.IsCancellationRequested)
            {
                bytesRead = await stream.ReadAsync(buffer, ct);
                if (bytesRead == 0) break;

                var headerCheck = Encoding.UTF8.GetString(buffer, 0, Math.Min(bytesRead, 512));

                if (headerCheck.StartsWith("FILE:"))
                {
                    // Formato: FILE:nomeDoArquivo:tamanhoEmBytes\n[bytes do arquivo]
                    var newlineIndex = headerCheck.IndexOf('\n');
                    if (newlineIndex < 0) continue;

                    var headerLine = headerCheck[..newlineIndex];
                    var parts = headerLine.Split(':', 3);
                    if (parts.Length < 3) continue;

                    var fileName = parts[1];
                    if (!long.TryParse(parts[2], out var fileSize)) continue;

                    Console.WriteLine($"[Socket] {auth.Email} enviando arquivo: {fileName} ({fileSize} bytes)");

                    var headerByteCount = Encoding.UTF8.GetByteCount(headerLine + "\n");
                    var fileBytes = new byte[fileSize];
                    var fileBytesReceived = bytesRead - headerByteCount;

                    Array.Copy(buffer, headerByteCount, fileBytes, 0, Math.Min(fileBytesReceived, (int)fileSize));

                    while (fileBytesReceived < fileSize)
                    {
                        var chunk = await stream.ReadAsync(fileBytes, fileBytesReceived, (int)(fileSize - fileBytesReceived), ct);
                        if (chunk == 0) break;
                        fileBytesReceived += chunk;
                    }

                    await BroadcastFileAsync(auth.ChatId, auth.Email, fileName, fileBytes, clientId, ct);
                }
                else
                {
                    var content = headerCheck.Trim();
                    Console.WriteLine($"[Socket] {auth.Email}: {content}");

                    await SaveMessageAsync(auth.UserId, auth.ChatId, content);
                    await BroadcastTextAsync(auth.ChatId, $"{auth.Email}: {content}", null, ct);
                }
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
                await BroadcastTextAsync(removed.ChatId, $"{removed.Email} saiu do chat.", clientId, ct);
            }
            tcpClient.Close();
        }
    }

    private static async Task BroadcastTextAsync(Guid chatId, string message, Guid? excludeClientId, CancellationToken ct)
    {
        var bytes = Encoding.UTF8.GetBytes("TEXT:" + message);
        foreach (var (id, client) in _clients)
        {
            if (client.ChatId != chatId) continue;
            if (excludeClientId.HasValue && id == excludeClientId.Value) continue;
            try { await client.Stream.WriteAsync(bytes, ct); } catch { }
        }
    }

    private static async Task BroadcastFileAsync(Guid chatId, string senderEmail, string fileName, byte[] fileBytes, Guid? excludeClientId, CancellationToken ct)
    {
        var header = $"FILE:{senderEmail}:{fileName}:{fileBytes.Length}\n";
        var headerBytes = Encoding.UTF8.GetBytes(header);
        var packet = new byte[headerBytes.Length + fileBytes.Length];
        Array.Copy(headerBytes, packet, headerBytes.Length);
        Array.Copy(fileBytes, 0, packet, headerBytes.Length, fileBytes.Length);

        foreach (var (id, client) in _clients)
        {
            if (client.ChatId != chatId) continue;
            if (excludeClientId.HasValue && id == excludeClientId.Value) continue;
            try { await client.Stream.WriteAsync(packet, ct); } catch { }
        }
    }

    private static async Task SendTextAsync(NetworkStream stream, string message, CancellationToken ct)
    {
        var bytes = Encoding.UTF8.GetBytes("TEXT:" + message);
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
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await messageRepo.SaveMessageAsync(message);
        });
    }

    private record AuthPayload(Guid UserId, string Email, Guid ChatId);
}