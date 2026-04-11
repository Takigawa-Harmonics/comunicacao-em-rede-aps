using System.Text.Json.Serialization;
using ComunicacaoEmRedesApi.Domain.Models;

namespace ComunicacaoEmRedesApi.Application.Dtos.Profile;

public class ProfileDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public IEnumerable<VinculatedChatsDto> Chats { get; set; } = [];
    public IEnumerable<VinculatedChatsDto.MessagesInChatDto> Messages { get; set; } = [];

    [JsonPropertyName("viewed_at")]
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;

    public static ProfileDto Get(User user)
    {
        if (user.Chats.Count == 0)
        {
            return new ProfileDto
            {
                Id = user.Id,
                Email = user.Email
            };
        }
        
        return new ProfileDto
        {
            Id = user.Id,
            Email = user.Email,
            Chats = user.Chats.Select(e => new VinculatedChatsDto{ChatId = e.Id, Title = e.Title}),
            Messages = user.Messages.Select(e => new VinculatedChatsDto.MessagesInChatDto{MessageId = e.Id, Content = e.Content, ChatId = e.ChatId})
        };
    }
}