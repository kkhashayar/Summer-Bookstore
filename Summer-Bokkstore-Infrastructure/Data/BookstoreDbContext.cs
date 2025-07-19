
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.EventLogs;
using System.Data.Common;

namespace Summer_Bookstore_Infrastructure.Data;

public class BookstoreDbContext : DbContext
{
    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    public DbSet<User> Users { get; set; }

    public DbSet<AuditEntry> AuditEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuring relations here: 
        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Author>()
            .HasKey(a => a.Id);

        // set it to unique 
        modelBuilder.Entity<Author>()
            .HasIndex(a => a.Name)
            .IsUnique(); 
        
        // Example of configuring a relationship
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);// Deletes books when an author is deleted    

        // Indicator that datetime should be delt with as Date 
        modelBuilder.Entity<Book>()
            .Property(b => b.PublishedDate)
            .HasColumnType("date"); // This will store only the date part without time  


        // User entity configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired(false); // I dont know why using migration did not set it to nullable, so I set it here




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
