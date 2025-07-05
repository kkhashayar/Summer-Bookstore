
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore_Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    public Task AddAsync(Author author)
    {
        throw new NotImplementedException();
    }

    public int Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Author>> GetAllAuthorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Author> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Author> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public int Update(Author author)
    {
        throw new NotImplementedException();
    }
}
