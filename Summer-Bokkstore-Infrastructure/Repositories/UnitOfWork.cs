using Microsoft.Extensions.Logging;
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;
using Summer_Bookstore_Infrastructure.Repositories;


public class UnitOfWork : IUnitOfWork
{
    private readonly BookstoreDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;   
    public IBookRepository BookRepository { get; }
    public IAuthorRepository AuthorRepository { get; }



    public UnitOfWork(BookstoreDbContext context,
                      IBookRepository bookRepository,
                      IAuthorRepository authorRepository,
                      ILogger<UnitOfWork> logger)
    {
        _context = context;
        BookRepository = bookRepository;
        AuthorRepository = authorRepository;
        _logger = logger;   
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> TryAddBookWithAuthorAsync(Book book, Author author)
    {
        
        var existingBook = await BookRepository.GetByTitleAsync(book.Title);
        if (existingBook != null)
        {
            _logger.LogWarning($"Book with title '{book.Title}' already exists. Skipping insertion");
            return false; // Book already exists, return false
        }

        // Check if the author already exists
        var existingAuthor = await AuthorRepository.GetByNameAsync(author.Name);
        if (existingAuthor == null)
        {
            await AuthorRepository.AddAsync(author);
            book.Author = author;   
        }
        else
        {
            book.AuthorId = existingAuthor.Id; // Associate the existing author with the book   
        }

        // Add the book to the repository
        await BookRepository.AddAsync(book);    

        await CompleteAsync(); // Save the new book 

        return true; // Book and author added successfully
    }
}
