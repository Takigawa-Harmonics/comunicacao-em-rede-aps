using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComunicacaoEmRedesApi.Domain.Models;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public required string Content { get; set; }

    public bool Active { get; set; }
    
    public Guid ChatId { get; set; }
    public Chat? Chat { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}