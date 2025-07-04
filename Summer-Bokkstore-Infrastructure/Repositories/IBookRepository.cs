using Summer_Bookstore_Domain.Entities;

namespace Summer_Bokkstore_Infrastructure.Interfaces;

public interface IBookRepository
{
    Task<Book> GetById(int id);
    Task<Book> GetByTitle(string title);
    Task<List<Book>> GetAllBooks();
    Task Add(Book book);
    Task<int> Update(Book book);
    Task<int> Delete(int id);
    Task<int> SaveChangesAsync();
}
