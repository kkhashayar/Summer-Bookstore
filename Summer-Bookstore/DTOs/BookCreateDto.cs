namespace Summer_Bookstore.DTOs;

public class BookCreateDto
{
    public string Title { get; set; } = string.Empty;
    public DateOnly PublishedDate { get; set; }
    public string? Description { get; set; }
    // Will add required later
    public string AuthorName { get; set; } = string.Empty;
}
