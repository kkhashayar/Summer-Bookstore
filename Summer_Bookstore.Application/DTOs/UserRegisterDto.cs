

using Summer_Bookstore_Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore.Application.DTOs;

public class UserRegisterDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
    [MinLength(6, ErrorMessage = "Username must be at least 3 characters long.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(24, ErrorMessage = "Password can't be longer than 24 characters.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    public Roles Role { get; set; }
}
