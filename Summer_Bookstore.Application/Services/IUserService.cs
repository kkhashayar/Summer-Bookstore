

using Summer_Bookstore.Application.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Application.Services;
public interface IUserService
{
    Task<User> RegisterUserAsync(User user);
    Task<User> LoginAsync(string username, string password);
    Task<User?> GetUserByUsernameAsync(string username);    
}
