
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Summer_Bookstore.Application.DTOs;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Domain.Enums;
using Summer_Bookstore_Infrastructure.Data;
using System.Security.Cryptography;

namespace Summer_Bookstore.Application.Services;

public class UserService : IUserService
{
    readonly BookstoreDbContext _context;
    readonly ILogger<UserService> _logger;
    public UserService(BookstoreDbContext context, ILogger<UserService> logger)
    {
        _context = context;
         _logger = logger;
    }
   

    public async Task<User> LoginAsync(string username, string password)
    {
        var passwordHash = HashPassword(password);  
        var userByUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if(userByUsername is null)
        {
            _logger.LogWarning("User with username {Username} not found.", username);
            throw new InvalidOperationException($"User with username {username} not found."); // Not sure about this, I guess it is better to back something like service response object
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash.SequenceEqual(passwordHash));    
        if(user is null)
        {
            _logger.LogWarning($"Something went wrong while trying to login with{username} {password} at {DateTime.Now}");
            throw new InvalidOperationException($"Username or password is wrong");
        }

        return user;

    }

    public async Task<User> RegisterUserAsync(User user)
    {
        // 1) Check if the user already exists, we could have same username, but I thought maybe better they are unique
        var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
        if (existingUser is not null)
        {
            _logger.LogWarning("User with username {Username} already exists.", user.Username);
            throw new InvalidOperationException($"User with username {user.Username} already exists.");
        }

        // 2) hash the password 
        var hashedPasword = HashPassword(user.Password);

        // 3) Create a new user entity
        // Setting a default role, can be changed later 
        if (user.Role is Roles.none)
        {
            user.Role = Roles.User;
        }
        User newUser = new User
        {
            Username = user.Username,
            PasswordHash = hashedPasword, // Store the hashed password
            Role = user.Role
        };
        _context.Users.Add(newUser); // Add the new user to the context 
        await _context.SaveChangesAsync(); // Save changes to the database      
        return newUser;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var result = await _context.Users.ToListAsync();    
        if(result.Count == 0)
        {
            return new List<User>(); // Return an empty list if no users are found  
        }

        return result;  
    }

    // Using the SHA256 algorithm to hash the password, also each time user tries to login,
    // we will hash the password and compare it with the stored hash in the database.
    private byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public Task<UserReadDto> UpdateUserByUsernameAsync(UserReadDto userReadDto)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteUserByUsername(UserReadDto userReadDto)
    {
        throw new NotImplementedException();
    }
}
