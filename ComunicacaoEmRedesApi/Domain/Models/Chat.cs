using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComunicacaoEmRedesApi.Domain.Models;

public class Chat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(30)]
    public required string Title { get; set; }

    public bool Active { get; set; }
    
    public ICollection<User> Users { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}