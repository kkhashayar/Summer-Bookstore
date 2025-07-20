
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;
using Summer_Bookstore_Infrastructure.EventLogs;

namespace Summer_Bookstore_Infrastructure.Repositories;

public class BookRepository : IBookRepository
{

    private readonly BookstoreDbContext _bookContext;
    private readonly ILogger<BookRepository> _logger;
    readonly AuditLogger _auditLogger;
    readonly IHttpContextAccessor _httpContextAccessor;

    public BookRepository(BookstoreDbContext bookContext, ILogger<BookRepository> logger, AuditLogger auditLogger, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _auditLogger = auditLogger;
        _bookContext = bookContext;
        _logger = logger;
    }


    // One thing to consider is whether or not we should include author information.
    // We could add a flag like `bool includeAuthor` and use it to conditionally include the author.
    // Leaving it out for the sake of simplicity for now.

    public async Task<Book> GetByIdAsync(int id)
    {
        string username = GetUser();

        var result = await _bookContext.Books.FindAsync(id);
        if (result == null)
        {
            var message = $"Book with id: {id} not found";
            _logger.LogWarning(message);
            await _auditLogger.LogAsync(message, LogType.Warning, new User { Username = username });
        }
        else
        {
            var message = $"Book with id: {id} retrieved successfully.";
            _logger.LogInformation(message);
            await _auditLogger.LogAsync(message, LogType.Information, new User { Username = username });
        }

        return result;
    }


    public async Task<Book> GetByTitleAsync(string title)
    {
        string username = GetUser();

        var result = await _bookContext.Books.FirstOrDefaultAsync(b => b.Title == title);
        if (result == null)
        {
            var message = $"Book with title '{title}' not found.";
            _logger.LogWarning(message);
            await _auditLogger.LogAsync(message, LogType.Warning, new User { Username = username });

        }
        else
        {
            var message = $"Book with id: {title} retrieved successfully.";
            _logger.LogInformation(message);
            await _auditLogger.LogAsync(message, LogType.Information, new User { Username = username });
        }
        return result;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        var user = GetUser();
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
        var username = GetUser();

        if (book is null)
        {
            var message = "Received null book object.";
            await _auditLogger.LogAsync(message, LogType.Error, new User { Username = username });
            throw new ArgumentNullException(nameof(book), "Book object cannot be null.");
        }

        // Validate author presence and name
        if (book.Author == null || book.Author.Name == null || string.IsNullOrWhiteSpace(book.Author.Name.Trim()))
        {
            var message = "Author name is required.";
            await _auditLogger.LogAsync(message, LogType.Error, new User { Username = username });
            throw new InvalidOperationException(message);
        }

        var authorName = book.Author.Name.Trim();

        // Try to find existing author
        var author = await _bookContext.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
        if (author == null)
        {
            author = new Author { Name = authorName };
            await _bookContext.Authors.AddAsync(author);
            await _bookContext.SaveChangesAsync(); // Ensure Author ID is generated

            var authorMessage = $"New author '{authorName}' added.";
            await _auditLogger.LogAsync(authorMessage, LogType.Information, new User { Username = username });
        }

        // Check if book with same title AND author already exists
        var duplicateBook = await _bookContext.Books
            .FirstOrDefaultAsync(b => b.Title == book.Title && b.AuthorId == author.Id);

        if (duplicateBook != null)
        {
            var message = $"Duplicate book detected: '{book.Title}' by '{authorName}' already exists.";
            _logger.LogWarning(message);
            await _auditLogger.LogAsync(message, LogType.Warning, new User { Username = username });
            return 0;
        }

        // Set the correct author ID and avoid re-adding author
        book.AuthorId = author.Id;
        book.Author = null;

        await _bookContext.Books.AddAsync(book);
        var result = await _bookContext.SaveChangesAsync();

        var successMessage = $"Book '{book.Title}' by '{authorName}' added successfully by user '{username}' at {DateTime.UtcNow}.";
        await _auditLogger.LogAsync(successMessage, LogType.Information, new User { Username = username });

        return result;
    }



    public async Task<int> Update(Book book)
    {
        var username = GetUser(); // Retrieve current username for audit logging

        // Check if the book exists
        var bookToUpdate = await _bookContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
        if (bookToUpdate is null)
        {
            _logger.LogWarning($"Book with id: {book.Id} not found at: {DateTime.UtcNow}.");
            await _auditLogger.LogAsync($"Book with id: {book.Id} not found", LogType.Warning, new User { Username = username });
            return 0; // Return 0 if book not found
        }

        // Extract and trim the author's name
        var authorName = book.Author?.Name?.Trim();
        if (string.IsNullOrWhiteSpace(authorName))
        {
            _logger.LogWarning("Author name is missing or empty during update.");
            await _auditLogger.LogAsync("Author name is missing or empty during update", LogType.Warning, new User { Username = username });
            throw new InvalidOperationException("Author name is required.");
        }

        // Check if author already exists
        var existingAuthor = await _bookContext.Authors
            .FirstOrDefaultAsync(a => a.Name.ToLower() == authorName.ToLower());

        // If we can't find the author we have to create a new one 
        if (existingAuthor is null)
        {
            var newAuthor = new Author { Name = authorName };
            await _bookContext.Authors.AddAsync(newAuthor); // Add new author if not exists
            await _bookContext.SaveChangesAsync(); // Save changes to ensure author is added
            bookToUpdate.AuthorId = newAuthor.Id; // Set the AuthorId for the book
        }
        else
        {
            bookToUpdate.AuthorId = existingAuthor.Id; // Set the AuthorId for the book
        }

        UpdateBookproperties(book, bookToUpdate); // Apply new values to the book

        _bookContext.Books.Update(bookToUpdate); // Mark the book as modified
        var result = await _bookContext.SaveChangesAsync();

        _logger.LogInformation($"Book with ID {book.Id} updated successfully by {username} at {DateTime.UtcNow}.");
        await _auditLogger.LogAsync($"Book with ID {book.Id} updated successfully", LogType.Information, new User { Username = username });

        return result;
    }


    public async Task<int> Delete(int id)
    {
        var username = GetUser(); // Retrieve current username for audit logging

        // Try to find the book by ID
        var bookToDelete = await _bookContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (bookToDelete is null)
        {
            var message = $"Book with id: {id} not found at: {DateTime.UtcNow}";
            _logger.LogWarning(message);
            await _auditLogger.LogAsync(message, LogType.Warning, new User { Username = username });
            return 0;
        }

        _bookContext.Remove(bookToDelete); // Remove the book from the context

        var result = await SaveChangesAsync(); // Save the changes to the database

        _logger.LogInformation($"Book with ID {id} deleted successfully by {username} at {DateTime.UtcNow}.");
        await _auditLogger.LogAsync($"Book with ID {id} deleted successfully", LogType.Information, new User { Username = username });

        return result;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _bookContext.SaveChangesAsync();
    }

    private static void UpdateBookproperties(Book book, Book bookToUpdate)
    {
        bookToUpdate.Title = book.Title;
        bookToUpdate.Description = book.Description;
        bookToUpdate.PublishedDate = book.PublishedDate;
        bookToUpdate.Author = book.Author; // Update the navigation property if needed
    }

    private string GetUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        string username = "Unknown";
        if (user != null && user.Identity != null && user.Identity.Name != null)
        {
            username = user.Identity.Name;
        }

        return username;
    }




}
