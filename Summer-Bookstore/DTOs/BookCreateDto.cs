using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore.DTOs;

public class BookCreateDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Title can not be longer than 100 characters")]   
    public string Title { get; set; } = string.Empty;
    
    public DateOnly PublishedDate { get; set; }
    [StringLength(1000, ErrorMessage = "Description can not be longer than 1000 characters")]
    public string? Description { get; set; }
   
    public string AuthorName { get; set; } = string.Empty;
}
