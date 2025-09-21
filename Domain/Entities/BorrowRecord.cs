using System;

namespace Domain.Entities;

public class BorrowRecord
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; }

    public Member Member { get; set; }
    public Book Book { get; set; } 
}
