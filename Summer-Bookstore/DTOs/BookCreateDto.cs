namespace Summer_Bookstore.DTOs;

public class BookCreateDto
{
    public string Title { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string AuthorName { get; set; }
}
