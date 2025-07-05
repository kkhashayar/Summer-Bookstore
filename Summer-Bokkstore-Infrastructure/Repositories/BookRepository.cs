
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;

namespace Summer_Bookstore_Infrastructure.Repositories;

public class BookRepository : IBookRepository
{

    private readonly BookstoreDbContext _bookContext;
    private readonly ILogger<BookRepository> _logger;
    public BookRepository(BookstoreDbContext bookContext, ILogger<BookRepository> logger)
    {
        _bookContext = bookContext; 
        _logger = logger;   
    }
    // One thing to consider is whether or not we should include author information.
    // We could add a flag like `bool includeAuthor` and use it to conditionally include the author.
    // Leaving it out for the sake of simplicity for now.

    public async Task<Book> GetById(int id)
    {
        var result = await _bookContext.Books.FindAsync(id);    
        if (result == null)
        {
            _logger.LogWarning($"Book with ID {id} not found at: {DateTime.Now}.");
            return result; // This will return null if not found, which is fine for this case.
        }
        return result; 
    }

    public async Task<Book> GetByTitle(string title)
    {
        var result = await _bookContext.Books.FirstOrDefaultAsync(b => b.Title == title);
        if(result == null)
        {
            _logger.LogWarning($"Book with title '{title}' not found at: {DateTime.Now}.");
            return result; // This will return null if not found, which is fine for this case.
        }   
        return result;
    }

    public async Task<List<Book>> GetAllBooks()
    {
        var result = await _bookContext.Books.ToListAsync();
        if (result.Count == 0)
        {
            _logger.LogInformation($"The book list empty at: {DateTime.Now}");
            return new List<Book>(); // Return an empty list if no books are found  
        }
        return result;  
    }

    // Add method should check 1) If the book already exists before adding it. 
    // Add method should add new Author in case the author does not exist.  
    public Task Add(Book book)
    {
        throw new NotImplementedException();
    }
    public Task<int> Update(Book book)
    {
        throw new NotImplementedException();
    }
    public Task<int> Delete(int id)
    {
        throw new NotImplementedException();
    }

    

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    
}
