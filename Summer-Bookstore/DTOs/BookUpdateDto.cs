namespace Summer_Bookstore.DTOs;

public class BookUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string AuthorName { get; set; } = string.Empty;
}
