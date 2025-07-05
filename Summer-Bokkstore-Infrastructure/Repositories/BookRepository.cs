
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;

namespace Summer_Bookstore_Infrastructure.Repositories;

public class BookRepository : IBookRepository
{

    private readonly BookstoreDbContext _bookContext;
    public BookRepository(BookstoreDbContext bookContext)
    {
        _bookContext = bookContext; 
    }
    // One thing to consider is whether or not we should include author information.
    // We could add a flag like `bool includeAuthor` and use it to conditionally include the author.
    // Leaving it out for the sake of simplicity for now.

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
