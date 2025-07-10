using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;
namespace Summer_Bookstore.Controllers;



[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    readonly ILogger<BooksController> _logger;
    readonly IMapper _mapper;
    readonly IBookRepository _bookRepository;
   
    public BooksController(IBookRepository bookRepository,ILogger<BooksController> logger, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _logger = logger;
       
    }

    /// <summary>
    /// Retrieves a book by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the book.</param>
    /// <returns>Returns 200 OK with the book if found, otherwise 404 Not Found.</returns>
    [HttpGet("{id}")]
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


    /// <summary>
    /// Retrieves a book by its title.
    /// </summary>
    /// <param name="title">The title of the book.</param>
    /// <returns>Returns 200 OK with the book if found, otherwise 404 Not Found.</returns>
    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetBookByTitle(string title)
    {
        // Connected directly to book repository 
        var book = await _bookRepository.GetByTitleAsync(title);
        if (book == null)
        {
            _logger.LogWarning($"Book with title '{title}' not found at: {DateTime.Now}.");
            return NotFound($"Book with title '{title}' not found.");
        }
        var bootToReturn = _mapper.Map<BookReadDto>(book);      
        return Ok(bootToReturn);
    }

    /// <summary>
    /// Retrieves all books from the database.
    /// </summary>
    /// <returns>Returns 200 OK with a list of books, or 404 if none are found.</returns>
    [HttpGet()]
    public async Task<IActionResult> GetAllBooks()
    {
        // Connected directly to book repository 
        var books = await _bookRepository.GetAllBooksAsync();
        if (books.Count == 0)
        {
            _logger.LogInformation($"The book list is empty at: {DateTime.Now}.");
            return NotFound("No books found.");
        }
        // readonly books 
        var booksToReturn = _mapper.Map<List<BookReadDto>>(books);
        return Ok(booksToReturn);
        
    }

    /// <summary>
    /// Adds a new book to the system.
    /// </summary>
    /// <param name="bookCreateDto">The book details to create.</param>
    /// <returns>Returns 201 Created with location header if successful, 400 Bad Request if input is invalid.</returns>
    [HttpPost()]
    public async Task<IActionResult> AddNewBook([FromBody] BookCreateDto bookcreateDto)
    {
        var book = _mapper.Map<Book>(bookcreateDto);
        if (book == null)
        {
            // I will change this with proper object validation later 
            _logger.LogWarning("Received null book object at: {DateTime.Now}.");
            return BadRequest("Book object cannot be null.");

        }

        var response = await _bookRepository.AddAsync(book);
        return Created(); 
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="bookUpdateDto">The updated book data.</param>
    /// <returns>Returns 204 No Content if successful, 400 Bad Request for ID mismatch, or 404 Not Found if book doesn't exist.</returns>
    [HttpPut()]
    public async Task<IActionResult> UpdateBook([FromBody] BookUpdateDto bookupdateDto)
    {
        var book = _mapper.Map<Book>(bookupdateDto);        
        var response = await _bookRepository.Update(book);
        if (response == 0)
        {
            _logger.LogWarning($"Failed to update book with ID {book.Id} at: {DateTime.Now}.");
            return NotFound($"Book with ID {book.Id} not found.");
        }
        
        return Ok("Book updated successfully."); // 
    }

    /// <summary>
    /// Deletes a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <returns>Returns 204 No Content if successful, or 404 Not Found if the book doesn't exist.</returns>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var response = await _bookRepository.Delete(id);
        if (response == 0)
        {
            _logger.LogWarning($"Failed to delete book with ID {id} at: {DateTime.Now}.");
            return NotFound($"Book with ID {id} not found.");
        }
        
        return Ok("Book deleted successfully."); // 
    }

}
