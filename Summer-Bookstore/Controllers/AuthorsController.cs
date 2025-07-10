using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Repositories;

namespace Summer_Bookstore.Controllers;

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
        var authorToReturn = _mapper.Map<AuthorReadDto>(author);
        return Ok(authorToReturn);
    }

    [HttpGet("all")]
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

    [HttpPost("add")]
    public async Task<IActionResult> AddNewAuthor([FromBody] AuthorCreateDto authorcreateDto)
    {
        var author = _mapper.Map<Author>(authorcreateDto);
        var response = await _authorRepository.AddAsync(author);
        if(response == 0)
        {
            _logger.LogInformation($"Something went wrong while trying to update author with id:{authorcreateDto.Name} at:{DateTime.Now}");
            return NoContent();  
        }
        // at least one record should have been created.
        return Ok($"Added new author with id:{response}");     
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AuthorUpdateDto authorUpdateDto)
    {
        var author = _mapper.Map<Author>(authorUpdateDto);

        var response = _authorRepository.Update(author);    
        if(response == 0)
        {
            return BadRequest(response);
        }
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAuthor(int authorId)
    {
        var response = _authorRepository.Delete(authorId);
        if(response == 0) { return NoContent(); }   
        return Ok($"Author with Id:{authorId} deleted.");
    }
}
