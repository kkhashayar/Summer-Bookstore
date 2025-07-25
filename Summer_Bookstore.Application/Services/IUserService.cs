﻿

using Summer_Bookstore.Application.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Application.Services;
public interface IUserService
{
    Task<User> RegisterUserAsync(User user);
    Task<User> LoginAsync(string username, string password);
    Task<List<User>> GetAllUsersAsync();    

    Task<UserReadDto> UpdateUserByUsernameAsync(UserLoginDto userLoginDto, UserUpdateDto userUpdateDto);
    Task<int> DeleteUserByUsername(UserLoginDto userLoginDto);

}
