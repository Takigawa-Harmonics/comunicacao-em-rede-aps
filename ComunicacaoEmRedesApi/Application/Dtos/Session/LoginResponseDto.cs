using System.Text.Json.Serialization;
using ComunicacaoEmRedesApi.Domain.Models;

namespace ComunicacaoEmRedesApi.Application.Dtos.Session;

public class LoginResponseDto
{
    [JsonPropertyName("operation_id")]
    public Guid OperationId { get; } = Guid.NewGuid();
    
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    
    [JsonIgnore]
    public Token? Token { get; set; }
    
    [JsonPropertyName("login_at")]
    public DateTime LoginAt { get; } = DateTime.UtcNow;
    
    [JsonPropertyName("last_login_made_at")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? LastLoginMadeAt { get; set; }
    
    public static LoginResponseDto Get(string message, Token token, DateTime lastLogin)
    {
        return new LoginResponseDto
        {
            Message = message,
            Token = token,
            LastLoginMadeAt = lastLogin
        };
    }
}