
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
        var result = await _bookstoreContext.Authors.FirstOrDefaultAsync(a => a.Name == name);
        if (result == null)
        {
            _logger.LogWarning($"Author with name '{name}' not found at: {DateTime.Now}.");
            return result;
        }
        return result;
    }
    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        var result = await _bookstoreContext.Authors.ToListAsync();   
        if(result.Count == 0)
        {
            _logger.LogInformation($"The author list is empty at: {DateTime.Now}"); 
            return new List<Author>(); // Return an empty list if no authors are found  
        }
        return result; 
    }
    public async Task AddAsync(Author author)
    {
        var result = await _bookstoreContext.Authors.AddAsync(author);
        if (result == null)
        {
            _logger.LogWarning($"Author with name '{author.Name}' already exists. Skipping insertion.");
            return; // Author already exists, return without adding
        }
        await _bookstoreContext.SaveChangesAsync();
    }
    public async Task<int> Update(Author author)
    {
        throw new NotImplementedException();
    }
    public async Task<int> Delete(int id)
    {
        throw new NotImplementedException();
    }
    
    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    
}
