using System.ComponentModel.DataAnnotations;

namespace Summer_Bookstore.DTOs;

public class AuthorCreateDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
}
/*
 * Used Data Annotations for validation, specifically the Required attribute to ensure that the Name 
 * property is not null or empty.
 */