using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Application.Dtos.Profile;

public class VinculatedChatsDto
{
    [JsonPropertyName("chat_id")]
    public Guid ChatId { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    public class MessagesInChatDto
    {
        [JsonPropertyName("message_id")]
        public Guid MessageId { get; set; }
    
        [JsonPropertyName("content")]
        public string? Content { get; set; }
    
        [JsonPropertyName("chat_id")]
        public Guid ChatId { get; set; }
    }
}