using System.ComponentModel.DataAnnotations;

namespace ComunicacaoEmRedesApi.Domain.Models;

public class Token
{
    [Key]
    public Guid UserId { get; set; }
    public required string Value { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
}