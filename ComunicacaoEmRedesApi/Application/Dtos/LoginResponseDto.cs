using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Application.Dtos;

public class LoginResponseDto
{
    [JsonPropertyName("operation_id")]
    public Guid OperationId { get; } = Guid.NewGuid();
    
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    
    [JsonPropertyName("login_at")]
    public DateTime LoginAt { get; } = DateTime.Now;
    
    [JsonPropertyName("last_login_made_at")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? LastLoginMadeAt { get; set; }
    
    public static LoginResponseDto Get(string message, DateTime lastLogin)
    {
        return new LoginResponseDto
        {
            Message = message,
            LastLoginMadeAt = lastLogin
        };
    }
}