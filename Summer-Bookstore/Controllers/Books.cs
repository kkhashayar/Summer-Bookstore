using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Repositories;

namespace Summer_Bookstore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Books : ControllerBase
{
    readonly ILogger<Books> _logger;
    readonly IMapper _mapper;
    readonly IBookRepository _bookRepository;
    readonly IUnitOfWork _unitOfWork;
    public Books(IUnitOfWork unitOfWork,IBookRepository bookRepository,ILogger<Books> logger, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
       
    }


    [HttpGet("id")]
    public async Task<IActionResult> GetBookById(int id)
    {
        // Connected directly to book repository 
        var book = await _bookRepository.GetByIdAsync(id);
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
        // Connected directly to book repository 
        var book = await _bookRepository.GetByTitleAsync(title);
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
        // Connected directly to book repository 
        var books = await _bookRepository.GetAllBooksAsync();
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

        var response = await _unitOfWork.TryAddBookWithAuthorAsync(book, book.Author);
        return Ok(); 
    }
}
