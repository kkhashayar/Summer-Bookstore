using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Summer_Bookstore.Application.DTOs;
using Summer_Bookstore.Application.Services;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    readonly IMapper _mapper;
    readonly ILogger<UsersController> _logger;
    readonly IUserService _userService;
    readonly ITokenService _tokenService;

    public UsersController(ILogger<UsersController> logger, IMapper mapper, IUserService userService, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _userService = userService;
        _logger = logger;
        _mapper = mapper;
    }


    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid user registration attempt: {Errors}", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<UserRegisterDto, User>(userRegisterDto);
        var response = await _userService.RegisterUserAsync(user);
        if (user == null)
        {
            return BadRequest("User registration failed.");
        }
        return Ok(user.Id);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _userService.LoginAsync(userLoginDto.Username, userLoginDto.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }
        var token = _tokenService.CreateToken(user);
        return Ok(new { token });

        //return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedUser = await _userService.UpdateUserByUsernameAsync(requestDto.UserLogin, requestDto.UserUpdate);
            if (updatedUser == null)
                return NotFound("User not found.");

            return Ok(updatedUser);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating user.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var response = await _userService.GetAllUsersAsync();
        if (response.Count == 0)
        {
            _logger.LogInformation("No users found in the system.");
            return NotFound("No users found.");
        }
        var usersToReturn = _mapper.Map<List<UserReadDto>>(response);   
        return Ok(usersToReturn);
    }




}
