using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComunicacaoEmRedesApi.Domain.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(252)]
    public required string Email { get; set; }

    [Required]
    [StringLength(60)]
    public required string PasswordHash { get; set; }
    
    public bool Active { get; set; }

    public ICollection<Chat> Chats { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}