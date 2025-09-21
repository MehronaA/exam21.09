using System;

namespace Domain.DTOs.BorrowRecords;

public class BorrowRecordCreateDto
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime? ReturnDate { get; set; }

}
