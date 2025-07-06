using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Repositories;

namespace Summer_Bookstore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Books : ControllerBase
{
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<Books> _logger;
    readonly IMapper _mapper;
    public Books(IUnitOfWork unitOfWork, ILogger<Books> logger, IMapper mapper)
    {
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    [HttpGet("id")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
        if (book == null)
        {
            _logger.LogWarning($"Book with ID {id} not found at: {DateTime.Now}.");
            return NotFound($"Book with ID {id} not found.");
        }
        return Ok(book);
    }

    [HttpGet("title")]
    public async Task<IActionResult> GetBookByTitle(string title)
    {
        var book = await _unitOfWork.BookRepository.GetByTitleAsync(title);
        if (book == null)
        {
            _logger.LogWarning($"Book with title '{title}' not found at: {DateTime.Now}.");
            return NotFound($"Book with title '{title}' not found.");
        }
        return Ok(book);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _unitOfWork.BookRepository.GetAllBooksAsync();
        if (books.Count == 0)
        {
            _logger.LogInformation($"The book list is empty at: {DateTime.Now}.");
            return NotFound("No books found.");
        }
        return Ok(books);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddNewBook([FromBody] BookCreateDto bookcreateDto)
    {
        var book = _mapper.Map<Book>(bookcreateDto);    
        if (book == null)
        {
            // I will change this with proper object validation later 
            _logger.LogWarning("Received null book object at: {DateTime.Now}.");
            return BadRequest("Book object cannot be null.");
        }

        await _unitOfWork.TryAddBookWithAuthorAsync(book);
        await _unitOfWork.CompleteAsync();
        _logger.LogInformation($"Book '{book.Title}' added successfully at: {DateTime.Now}.");

        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }
}
