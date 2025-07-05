
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore_Infrastructure.Repositories;

public interface IAuthorRepository
{
    // I used async methods for better performance in case of I/O operations.   
    Task<Author> GetByIdAsync(int id);
    Task<Author> GetByNameAsync(string name);   
    Task<List<Author>> GetAllAuthorsAsync();
    // AddAsync prepares the entity for tracking asynchronously
    Task AddAsync(Author author);       

    // no direct io interaction
    int Update(Author author);
    int Delete(int id);
    Task SaveChangesAsync(); // This method will save changes to the database   

}
