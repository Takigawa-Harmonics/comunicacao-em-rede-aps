using ComunicacaoEmRedesApi.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ComunicacaoEmRedesApi.Api.Controllers;

[ApiController]
[Route("/v1/chat")]
public class ChatController : ControllerBase
{
    private readonly IMessageRepository _messageRepository;
    private readonly ITokenRepository _tokenRepository;

    public ChatController(IMessageRepository messageRepository, ITokenRepository tokenRepository)
    {
        _messageRepository = messageRepository;
        _tokenRepository = tokenRepository;
    }

    [HttpGet("messages")]
    public async Task<IResult> GetMessages([FromQuery] Guid chatId)
    {
        var messages = await _messageRepository.GetMessagesByChatIdAsync(chatId);
        return Results.Ok(messages.Select(m => new
        {
            Text = $"{m.User!.Email}: {m.Content}",
            m.CreatedAt,
            m.UserId
        }));
    }
}