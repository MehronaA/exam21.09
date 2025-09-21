using System;

namespace Domain.DTOs.Members;

public class MemberGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime MemberShip { get; set; }
}
