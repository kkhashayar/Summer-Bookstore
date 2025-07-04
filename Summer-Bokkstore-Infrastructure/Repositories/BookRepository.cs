
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore_Infrastructure.Repositories;

public class BookRepository : IBookRepository
{

    public Task<Book> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Book> GetByTitle(string title)
    {
        throw new NotImplementedException();
    }

    public Task Add(Book book)
    {
        throw new NotImplementedException();
    }
    public Task<int> Update(Book book)
    {
        throw new NotImplementedException();
    }
    public Task<int> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Book>> GetAllBooks()
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    
}
