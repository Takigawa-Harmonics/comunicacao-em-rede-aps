using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Application.Dtos.Session;

public class RegisterRequestDto
{
    [JsonPropertyName("email")]
    [Required(ErrorMessage = "Email field is required!")]
    public required string Email { get; set; }
    
    [JsonPropertyName("password")] 
    [Required(ErrorMessage = "Password field is required!")]
    public required string Password { get; set; }
}