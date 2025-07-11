using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Summer_Bookstore_Domain.Entities;

public class Book
{
    
    public int Id { get; set; }
    [Required(ErrorMessage = "Title can not be empty")]
    [StringLength(100)]
    public string Title  { get; set; } = string.Empty;
    [StringLength(1000, ErrorMessage = "Description can not be longer than 1000 characters")]
    public string? Description { get; set; }


    [DataType(DataType.Date)] // Has no effect on how it stored in Database
    public DateTime PublishedDate { get; set; }

    // Foreign Key  
    public int AuthorId { get; set; }

    // Navigation Property
    public Author? Author { get; set; }

}

