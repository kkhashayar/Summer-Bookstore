
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Summer_Bookstore_Domain.Entities;
using System.Data.Common;

namespace Summer_Bookstore_Infrastructure.Data;

public class BookstoreDbContext: DbContext
{
    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options): base(options){ }

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
    }   

}
