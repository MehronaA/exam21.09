using System;

namespace Domain.DTOs.Authors;

public class AuthorCreateDto
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
}
