using System;

namespace Domain.DTOs.BorrowRecords;

public class BorrowRecordGetDto
{
    public int Id { get; set; }
    public string MemberName { get; set; }
    public string BookTitle { get; set; }
    public DateTime BorrowDate { get; set; } 
    public DateTime? ReturnDate { get; set; }
}
