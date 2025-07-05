
using Summer_Bokkstore_Infrastructure.Interfaces;



namespace Summer_Bookstore_Infrastructure.Repositories;

public interface IUnitOfWork
{
    IBookRepository BookRepository { get; }
    IAuthorRepository AuthorRepository { get; }   
    Task<int> CompleteAsync(); // This method will save changes to the database 

}
