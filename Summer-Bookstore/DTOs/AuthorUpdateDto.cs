using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore.DTOs;

public class AuthorUpdateDto : AuthorCreateDto
{
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
    public int Id { get; set; }

}
