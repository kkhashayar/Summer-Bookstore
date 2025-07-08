namespace Summer_Bookstore.DTOs;

public class AuthorReadDto
{
    public string Name { get; set; } = string.Empty;    
    public List<BookReadDto>? ListOfBooks { get; set; }
}
