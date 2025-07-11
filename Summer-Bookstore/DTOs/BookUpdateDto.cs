using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore.DTOs;

public class BookUpdateDto
{
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string AuthorName { get; set; } = string.Empty;
}
