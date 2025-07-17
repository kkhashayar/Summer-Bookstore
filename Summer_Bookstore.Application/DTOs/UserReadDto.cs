using Summer_Bookstore_Domain.Enums;

namespace Summer_Bookstore.Application.DTOs;

public class UserReadDto
{
    public string Username { get; set; } = string.Empty;
    public Roles Role { get; set; } = Roles.none; // Default role, can be changed later
}
