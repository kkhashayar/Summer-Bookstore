using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore_Domain.Entities;

public class Author
{
    public int Id { get; set; }
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Book> Books { get; set; } = new List<Book>();
}

//I use ICollection<T> over List<T> because: EF works with interfaces, not concrete types.
