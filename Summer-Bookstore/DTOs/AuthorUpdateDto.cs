using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore.DTOs;

public class AuthorUpdateDto : AuthorCreateDto
{
    [Required(ErrorMessage = "Id is required")] 
    public int Id { get; set; }

}
