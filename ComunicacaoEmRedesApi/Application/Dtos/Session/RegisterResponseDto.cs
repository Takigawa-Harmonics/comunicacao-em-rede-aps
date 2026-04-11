using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Application.Dtos.Session;

public class RegisterResponseDto
{
    [JsonPropertyName("operation_id")]
    public Guid OperationId { get; } = Guid.NewGuid();
    
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateOnly CreatedAt { get; } = DateOnly.FromDateTime(DateTime.Today);

    public static RegisterResponseDto Get(string email)
    {
        return new RegisterResponseDto
        {
            Email = email
        };
    }
}