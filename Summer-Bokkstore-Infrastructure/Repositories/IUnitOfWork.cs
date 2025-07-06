
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore_Domain.Entities;
namespace Summer_Bookstore_Infrastructure.Repositories;

public interface IUnitOfWork
{
    IBookRepository BookRepository { get; }
    IAuthorRepository AuthorRepository { get; }

    Task<bool> TryAddBookWithAuthorAsync(Book book, Author author);
    Task<int> CompleteAsync(); // This method will save changes to the database 

}
