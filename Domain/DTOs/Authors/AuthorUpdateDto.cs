using System;

namespace Domain.DTOs.Authors;

public class AuthorUpdateDto
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
}
