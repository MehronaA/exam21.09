using System;

namespace Domain.DTOs.Books;

public class BookGetDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int PublishedYear { get; set; }
    public string AuthorName { get; set; }

}
