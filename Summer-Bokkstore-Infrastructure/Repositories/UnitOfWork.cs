using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore_Infrastructure.Data;
using Summer_Bookstore_Infrastructure.Repositories;


public class UnitOfWork : IUnitOfWork
{
    private readonly BookstoreDbContext _context;

    public IBookRepository _BookRepository { get; }
    public IAuthorRepository _AuthorRepository { get; }



    public UnitOfWork(BookstoreDbContext context,
                      IBookRepository bookRepository,
                      IAuthorRepository authorRepository)
    {
        _context = context;
        _BookRepository = bookRepository;
        _AuthorRepository = authorRepository;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
