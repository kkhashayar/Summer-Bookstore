namespace Summer_Bookstore.DTOs;

public class AuthorCreateDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
}
