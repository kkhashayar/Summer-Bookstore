

using Summer_Bookstore.Application.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Application.Services;
public interface IUserService
{
    Task<User> RegisterUserAsync(UserRegisterDto userRegisterDto);
    Task<User> LoginAsync(string username, string password);
    Task<User?> GetUserByUsernameAsync(string username);    
}
