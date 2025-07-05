
using Summer_Bokkstore_Infrastructure.Interfaces;
using System.Net;

namespace Summer_Bookstore_Infrastructure.Repositories;

public interface IUnitOfWork
{
    IBookRepository Books { get; }  

    Task<int> CompleteAsync(); // This method will save changes to the database 

}
