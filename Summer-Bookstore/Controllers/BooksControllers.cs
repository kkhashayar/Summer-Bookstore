
using Microsoft.AspNetCore.Mvc;
using Summer_Bookstore_Infrastructure.Repositories;

namespace Summer_Bookstore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksControllers : ControllerBase
{
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<BooksControllers> logger;
    public BooksControllers(IUnitOfWork unitOfWork, ILogger<BooksControllers> logger)
    {
        logger = logger;
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
}
