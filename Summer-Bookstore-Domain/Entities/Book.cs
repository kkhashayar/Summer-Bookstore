﻿namespace Summer_Bookstore_Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title  { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime PublishedDate { get; set; }

    // Foreign Key  
    public int AuthorId { get; set; }

    // Navigation Property
    public Author? Author { get; set; }

}

