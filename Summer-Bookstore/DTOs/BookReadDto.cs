namespace Summer_Bookstore.DTOs;

public class BookReadDto
{
    public string Title { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public string? Description { get; set; }
    public DateOnly PublishedDate { get; set; }

}
