﻿namespace Summer_Bookstore_Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Book> Books { get; set; } = new List<Book>();
}

//I use ICollection<T> over List<T> because: EF works with interfaces, not concrete types.
