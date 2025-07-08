
using Azure.Identity;
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
   
    public BookRepository(BookstoreDbContext bookContext,ILogger<BookRepository> logger)
    {
       
        _bookContext = bookContext; 
        _logger = logger;   
    }
    // One thing to consider is whether or not we should include author information.
    // We could add a flag like `bool includeAuthor` and use it to conditionally include the author.
    // Leaving it out for the sake of simplicity for now.

    public async Task<Book> GetByIdAsync(int id)
    {
        var result = await _bookContext.Books.FindAsync(id);    
        if (result == null)
        {
            _logger.LogWarning($"Book with ID {id} not found at: {DateTime.Now}.");
            return result; // This will return null if not found, which is fine for this case.
        }
        return result; 
    }

    public async Task<Book> GetByTitleAsync(string title)
    {
        var result = await _bookContext.Books.FirstOrDefaultAsync(b => b.Title == title);
        if(result == null)
        {
            _logger.LogWarning($"Book with title '{title}' not found at: {DateTime.Now}.");
            return result; // This will return null if not found, which is fine for this case.
        }   
        return result;
    }

    public async Task<List<Book>> GetAllBooksAsync()
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
    // Initially I went with Unit of work but I think it would be overkill for this simple repository pattern.
    public async Task<int> AddAsync(Book book)
    {
        // I will move this validation to the controller later.
        if (book is null) throw new ArgumentNullException(nameof(book), "Book object cannot be null.");

        // Same for author name validation
        var authorName = book.Author?.Name?.Trim();
        if (string.IsNullOrWhiteSpace(authorName))
            throw new InvalidOperationException("AuthorName is required.");

        // check if exact book with same author name is already exists.
        var existingBook = await _bookContext.Books .FirstOrDefaultAsync(b => b.Title == book.Title && b.AuthorId == book.AuthorId);

        if (existingBook != null)
        {
            _logger.LogWarning($"Book with title '{book.Title}' and author '{authorName}' already exists at: {DateTime.Now}.");
            return 0; // Return 0 or some indication that no new book was added
        }

        /********************************************************************/ 
        // Author check, find or create
        var author = await _bookContext.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
        if (author is null)
        {
            author = book.Author;
            await _bookContext.Authors.AddAsync(author); // Add author if not exists
            await _bookContext.SaveChangesAsync(); // Save changes to ensure author is added    
        }
        book.AuthorId = author.Id; // Set the AuthorId for the book

        await _bookContext.Books.AddAsync(book); // Add the book to the context
        return await _bookContext.SaveChangesAsync(); // Save changes to the database   
    }


    public Task<int> Update(Book book)
    {
        var bookToUpdate = _bookContext.Books.FirstOrDefault(b => b.Id == book.Id);
        if (bookToUpdate is null)
        {
            return Task.FromResult(0); // Return 0 if book not found
        }

        // Update the book properties
        bookToUpdate.Title = book.Title;
        bookToUpdate.Description = book.Description;
        bookToUpdate.PublishedDate = book.PublishedDate;
        _bookContext.Books.Update(bookToUpdate); // Mark the book as modified
        return _bookContext.SaveChangesAsync();

    }
    public Task<int> Delete(int id)
    {
        throw new NotImplementedException();
    }

    

    public async Task<int> SaveChangesAsync()
    {
       return  await _bookContext.SaveChangesAsync(); 
    }

    
}
