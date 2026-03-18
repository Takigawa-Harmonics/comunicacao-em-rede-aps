using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Application.Dtos;

public class RegisterRequestDto
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}