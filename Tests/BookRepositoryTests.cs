using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;
using Summer_Bookstore_Infrastructure.Repositories;

namespace Tests;

public class BookRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenBookNotFound()
    {
        // Arrange
        var (bookRepository, _) = CreateBookRepositoryWithEmptyDb();

        // Act
        var result = await bookRepository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBook_WhenExists()
    {
        // Arrange
        var (bookRepository, context) = CreateBookRepositoryWithEmptyDb();

        var author = new Author { Name = "Author_Test" };
        context.Authors.Add(author);
        context.SaveChanges();

        var book = new Book
        {
            Title = "Book_Test",
            AuthorId = author.Id,
            PublishedDate = new DateTime(2023, 1, 1)
        };
        context.Books.Add(book);
        context.SaveChanges();

        // Act
        var result = await bookRepository.GetByIdAsync(book.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Book_Test");
        result.AuthorId.Should().Be(author.Id);
    }

    [Fact]
    public async Task AddAsync_AddsBookSuccessfully_WhenValidBookAndAuthorExist()
    {
        // Arrange 
        var (bookRepository, context) = CreateBookRepositoryWithEmptyDb();
        var author = new Author { Name = "isaac asimov" };
        context.Authors.Add(author);    
        await context.SaveChangesAsync(); // Ensure the author is saved before adding the book


        var book = new Book
        {
            Title = "Foundation",
            AuthorId = author.Id,
            PublishedDate = new DateTime(1951, 6, 1),
            Author = author // Set the Author property to ensure the book is linked to the author
        };
        // Act 
        var result = await bookRepository.AddAsync(book);       

        // Assert 
        result.Should().BeGreaterThan(0);
        
    }

    private static (BookRepository repo, BookstoreDbContext context) CreateBookRepositoryWithEmptyDb()
    {
        //creating new database for each test to ensure isolation   
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<BookstoreDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new BookstoreDbContext(options);
        var logger = new Mock<ILogger<BookRepository>>().Object;
        var repo = new BookRepository(context, logger);

        return (repo, context);
    }
}
