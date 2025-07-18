using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Repositories;

namespace Summer_Bookstore.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{

    readonly ILogger<AuthorsController> _logger;
    readonly IAuthorRepository _authorRepository;
    readonly IMapper _mapper;
    public AuthorsController(IAuthorRepository authorRepository, IMapper mapper, ILogger<AuthorsController> logger)
    {
        _mapper = mapper;
        _authorRepository = authorRepository;
        _logger = logger;

    }

    /// <summary>
    /// Retrieves an author by their ID.
    /// </summary>
    /// <param name="id">The ID of the author.</param>
    /// <returns>200 OK with author data if found, otherwise 404 Not Found.</returns>
    /// Will keep this actionmethod only for admins
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")] 
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


    /// <summary>
    /// Retrieves an author by their name.
    /// </summary>
    /// <param name="name">The name of the author.</param>
    /// <returns>200 OK with author data if found, otherwise 404 Not Found.</returns>
    [AllowAnonymous]
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetAuthorByName(string name)
    {
        var author = await _authorRepository.GetByNameAsync(name);
        if (author == null)
        {
            _logger.LogWarning($"Author with name '{name}' not found at: {DateTime.Now}.");
            return NotFound($"Author with name '{name}' not found.");
        }
        var authorToReturn = _mapper.Map<AuthorReadDto>(author);
        return Ok(authorToReturn);
    }


    /// <summary>
    /// Retrieves all authors.
    /// </summary>
    /// <returns>200 OK with list of authors or 404 if none are found.</returns>
    [AllowAnonymous]
    [HttpGet()]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _authorRepository.GetAllAuthorsAsync();
        if (authors.Count == 0)
        {
            _logger.LogInformation($"The author list is empty at: {DateTime.Now}.");
            return NotFound("No authors found.");
        }
        var authorsToReturn = _mapper.Map<List<AuthorReadDto>>(authors); // Assuming you have a mapping for Author     
        return Ok(authorsToReturn);
    }


    /// <summary>
    /// Adds a new author.
    /// </summary>
    /// <param name="authorCreateDto">The new author data.</param>
    /// <returns>201 Created if successful, or 400 Bad Request on failure.</returns>
    [HttpPost()]
    [Authorize(Roles = "Admin")]    
    public async Task<IActionResult> AddNewAuthor([FromBody] AuthorCreateDto authorcreateDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Invalid model state for author creation at: {DateTime.Now}.");
            return BadRequest(ModelState);
        }
        var author = _mapper.Map<Author>(authorcreateDto);
        var response = await _authorRepository.AddAsync(author);
        if (response == 0)
        {
            _logger.LogInformation($"Something went wrong while trying to update author with id:{authorcreateDto.Name} at:{DateTime.Now}");
            return BadRequest();
        }
        // at least one record should have been created.
        return Ok($"Added new author with id:{response}");
    }


    /// <summary>
    /// Updates an existing author.
    /// </summary>
    /// <param name="authorUpdateDto">The updated author data including ID.</param>
    /// <returns>204 No Content if successful, or 400/404 on failure.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]    
    public async Task<IActionResult> Update([FromBody] AuthorUpdateDto authorUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Invalid model state for author update at: {DateTime.Now}.");
            return BadRequest(ModelState);
        }
        var author = _mapper.Map<Author>(authorUpdateDto);

        var response = _authorRepository.Update(author);
        if (response == 0)
        {
            return NotFound($"Author with ID {author.Id} not found");
        }
        return NoContent();
    }


    /// <summary>
    /// Deletes an author by ID.
    /// </summary>
    /// <param name="id">The ID of the author to delete.</param>
    /// <returns>204 No Content if successful, or 404 if not found.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]    
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning($"Invalid author ID {id} for deletion at: {DateTime.Now}.");
            return BadRequest("Invalid author ID.");
        }
        var response = _authorRepository.Delete(id);
        if(response == 0) 
        { 
            return NotFound(); 
        }   
        return Ok($"Author with Id:{id} deleted.");
    }
}
