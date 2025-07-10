
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;

namespace Summer_Bookstore_Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    readonly BookstoreDbContext _bookstoreContext;
    readonly ILogger<AuthorRepository> _logger;

    public AuthorRepository(BookstoreDbContext bookstoreContext, ILogger<AuthorRepository> logger)
    {
        _bookstoreContext = bookstoreContext;
        _logger = logger;
    }

    public async Task<Author> GetByIdAsync(int id)
    {
        var result = await _bookstoreContext.Authors.FindAsync(id);
        if (result == null)
        {
            _logger.LogWarning($"Author with ID {id} not found at: {DateTime.Now}.");
            return result;
        }
        return result;
    }

    public async Task<Author> GetByNameAsync(string name)
    {
        var author = await _bookstoreContext.Authors.Where(author => author.Name == name)
            .Include(author => author.Books).FirstOrDefaultAsync();

        if (author == null)
        {
            _logger.LogWarning($"Author with name '{name}' not found at: {DateTime.Now}.");
        }

        return author;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        var result = await _bookstoreContext.Authors
            .Include(author => author.Books)
            .ToListAsync();

        if (result.Count == 0)
        {
            _logger.LogInformation($"The author list is empty at: {DateTime.Now}");
            return new List<Author>();
        }

        return result;
    }

    public async Task<int> AddAsync(Author author)
    {
        var existingAuthor = await _bookstoreContext.Authors
            .FirstOrDefaultAsync(a => a.Name == author.Name);
        if (existingAuthor is not null)
        {
            _logger.LogWarning($"Author with name '{author.Name}' already exists. Skipping insertion.");
            return 0; // Author already exists, return without adding
        }
        var result = await _bookstoreContext.Authors.AddAsync(author);
        if (result == null)
        {
            _logger.LogWarning($"Something went wrong while trying to add new author at {DateTime.Now}");
            return 0; // Author already exists, return without adding
        }
        return await _bookstoreContext.SaveChangesAsync();
    }

    // todo: Implement the Update method to update an existing author.
    public int Update(Author author)
    {
        var authorToUpdate = _bookstoreContext.Authors.FirstOrDefault(a => a.Id == author.Id); 
        if(authorToUpdate is null)
        {
            _logger.LogInformation($"Author with {author.Name} and {author.Id} not found at: {DateTime.Now}");       
            return 0;
        }
        authorToUpdate.Name = author.Name;      
      
        return _bookstoreContext.SaveChanges();   
    }
    public int Delete(int id)
    {
        var authorToDelete = _bookstoreContext.Authors.FirstOrDefault(author => author.Id == id);   
        if(authorToDelete is null)
        {
            _logger.LogInformation($"Author with Id:{id} not found");
            return 0;   
        }
        var relatedBooksToDelete =  _bookstoreContext.Books.Where(book => book.AuthorId == id).ToList();  
        if(relatedBooksToDelete.Count != 0)
        {
            _bookstoreContext.Books.RemoveRange(relatedBooksToDelete);
            _bookstoreContext.SaveChanges(true);    
        }

        _bookstoreContext.Remove(authorToDelete);
        return _bookstoreContext.SaveChanges(); 
        
    }
    
    

    
}
