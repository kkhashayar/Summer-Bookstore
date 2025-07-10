
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore_Infrastructure.Repositories;

public interface IAuthorRepository
{  
    Task<Author> GetByIdAsync(int id);
    Task<Author> GetByNameAsync(string name);   
    Task<List<Author>> GetAllAuthorsAsync();
    Task<int> AddAsync(Author author);       
    // I leave this two methods without async because they wont directly interact with IO
    int Update(Author author);
    int Delete(int id);

}
