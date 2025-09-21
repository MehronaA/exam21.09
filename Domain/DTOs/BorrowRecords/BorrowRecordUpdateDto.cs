using System;

namespace Domain.DTOs.BorrowRecords;

public class BorrowRecordUpdateDto
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime? ReturnDate { get; set; }
}
