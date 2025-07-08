
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

    public BookRepository(BookstoreDbContext bookContext, ILogger<BookRepository> logger)
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
        if (result == null)
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
        var existingBook = await _bookContext.Books.FirstOrDefaultAsync(b => b.Title == book.Title && b.AuthorId == book.AuthorId);

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


    public async Task<int> Update(Book book)
    {
        var bookToUpdate = await _bookContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
        if (bookToUpdate is null)
        {
            _logger.LogInformation($"book with id: {book.Id} not found at: {DateTime.Now}");
            return 0; // Return 0 if book not found
        }
        string authorName = "";

        if (!string.IsNullOrEmpty(book.Author.Name))
        {
            authorName = book.Author.Name.Trim();
        }
        else
        {
            authorName = book.Author.Name;
        }

        var existingAuthor = await _bookContext.Authors.FirstOrDefaultAsync(a => a.Name.ToLower() == authorName.ToLower());

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

        UpdateBookproperties(book, bookToUpdate);

        _bookContext.Books.Update(bookToUpdate); // Mark the book as modified
        return await _bookContext.SaveChangesAsync();
    }

    private static void UpdateBookproperties(Book book, Book bookToUpdate)
    {
        bookToUpdate.Title = book.Title;
        bookToUpdate.Description = book.Description;
        bookToUpdate.PublishedDate = book.PublishedDate;
        bookToUpdate.Author = book.Author; // Update the navigation property if needed
    }


    public async Task<int> Delete(int id)
    {
        var bookToDelete = await _bookContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (bookToDelete is null)
        {
            _logger.LogInformation($"book with id: {id} not found at: {DateTime.Now}");
            return 0;
        }
        _bookContext.Remove(bookToDelete);
        return await SaveChangesAsync();
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _bookContext.SaveChangesAsync();
    }


}
