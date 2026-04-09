using System.ComponentModel.DataAnnotations;

namespace ComunicacaoEmRedesApi.Domain.Models;

public class Token
{
    [Key]
    public Guid UserId { get; set; }
    public required string Value { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }

    public static Token Get(Guid userId, string value)
    {
        return new Token
        {
            UserId = userId,
            Value = value,
            Expiration = DateTime.UtcNow.AddMinutes(30),
            IsRevoked = false
        };
    }
}