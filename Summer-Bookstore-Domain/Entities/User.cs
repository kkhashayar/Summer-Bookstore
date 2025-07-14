using Summer_Bookstore_Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore_Domain.Entities;

public class User
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 100 characters.")]
    public string Username { get; set; } = string.Empty;
    
    
    
    [Required]
    public byte[] PasswordHash { get; set; }
    public Roles Role { get; set; }
}
