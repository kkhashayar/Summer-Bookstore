
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore_Infrastructure.Repositories;

public interface IAuthorRepository
{  
    Task<Author> GetByIdAsync(int id);
    Task<Author> GetByNameAsync(string name);   
    Task<List<Author>> GetAllAuthorsAsync();
    Task AddAsync(Author author);       
    Task<int> Update(Author author);
    Task<int> Delete(int id);
    Task SaveChangesAsync(); // This method will save changes to the database   

}
