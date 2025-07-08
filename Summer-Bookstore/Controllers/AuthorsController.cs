using Microsoft.AspNetCore.Mvc;
using Summer_Bookstore_Infrastructure.Repositories;

namespace Summer_Bookstore.Controllers;

[ApiController] 
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
   
    readonly ILogger<AuthorsController> _logger;
    readonly IAuthorRepository _authorRepository;
    public AuthorsController(IAuthorRepository authorRepository, ILogger<AuthorsController> logger)
    {
        _authorRepository = authorRepository;
        _logger = logger;
        
    }

    // Might reserve this endpoint only for admin users later
    [HttpGet("id")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            _logger.LogWarning($"Author with ID {id} not found at: {DateTime.Now}.");
            return NotFound($"Author with ID {id} not found.");
        }
        return Ok(author);
    }

    [HttpGet("name")]
    public async Task<IActionResult> GetAuthorByName(string name)
    {
        var author = await _authorRepository.GetByNameAsync(name);
        if (author == null)
        {
            _logger.LogWarning($"Author with name '{name}' not found at: {DateTime.Now}.");
            return NotFound($"Author with name '{name}' not found.");
        }
        return Ok(author);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _authorRepository.GetAllAuthorsAsync();
        if (authors.Count == 0)
        {
            _logger.LogInformation($"The author list is empty at: {DateTime.Now}.");
            return NotFound("No authors found.");
        }
        return Ok(authors);
    }
}
