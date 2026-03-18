using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ComunicacaoEmRedesApi.Domain.Models;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(2000)]
    public required string Value { get; set; }

    public Guid ChatId { get; set; }
    public Chat? Chat { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}