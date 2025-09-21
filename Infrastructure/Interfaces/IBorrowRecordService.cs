using System;
using Domain.DTOs.BorrowRecords;
using Infrastructure.APIResponce;

namespace Infrastructure.Interfaces;

public interface IBorrowRecordService
{
    Task<Responce<List<BorrowRecordGetDto>>> GetItemsAsync();
    Task<Responce<string>> CreateItemAsync(BorrowRecordCreateDto dto);
    Task<Responce<string>> UpdateItemAsync(int id, BorrowRecordUpdateDto dto);
    Task<Responce<string>> DeleteItemAsync(int id);
    Task<Responce<BorrowRecordGetDto>> GetItemByIdAsync(int id);
    Task<Responce<List<BorrowRecordGetDto>>> GetOverdueBorrows();
    Task<Responce<List<BorrowRecordGetDto>>> GetBorrowHistoryByMember(int id);
    Task<Responce<List<BorrowRecordGetDto>>> GetBorrowHistoryByBook(int id);
}
