using Summer_Bookstore_Domain.Entities;

namespace Summer_Bokkstore_Infrastructure.Interfaces;

public interface IBookRepository
{
    Task<Book> GetByIdAsync(int id);
    Task<Book> GetByTitleAsync(string title);
    Task<List<Book>> GetAllBooksAsync();
    Task AddAsync(Book book);
    Task<int> Update(Book book);
    Task<int> Delete(int id);
    Task<int> SaveChangesAsync();
}
