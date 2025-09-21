using System;

namespace Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime MemberShip { get; set; } = DateTime.Now;

    public List<BorrowRecord> BorrowRecords = [];

    
}
