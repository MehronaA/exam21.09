using System;

namespace Domain.DTOs.Books;

public class BookCreateDto
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public int  PublishedYear { get; set; }
    public int AuthorId { get; set; }
}
