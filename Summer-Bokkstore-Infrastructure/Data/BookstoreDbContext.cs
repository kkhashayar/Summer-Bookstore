
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Summer_Bookstore_Domain.Entities;
using System.Data.Common;

namespace Summer_Bookstore_Infrastructure.Data;

public class BookstoreDbContext : DbContext
{
    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuring relations here: 
        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Author>()
            .HasKey(a => a.Id);
        // Example of configuring a relationship
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);// Deletes books when an author is deleted    


        // Seeding 
        modelBuilder.Entity<Author>().HasData(
        new Author { Id = 1, Name = "George Orwell" },
        new Author { Id = 2, Name = "Jane Austen" },
        new Author { Id = 3, Name = "Isac Asimov" }
        
         );

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "1984", PublishedDate = new DateTime(1949, 6, 8), AuthorId = 1 },
            new Book { Id = 2, Title = "Pride and Prejudice", PublishedDate = new DateTime(1813, 1, 28), AuthorId = 2 },
            new Book { Id = 3, Title = "Foundation", PublishedDate = new DateTime(1951, 6, 1), AuthorId = 3 }
        );
    }

}
