using System;
using Domain.DTOs.BorrowRecords;
using Infrastructure.APIResponce;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/borrow-record")]
public class BorrowRecordController
{
    private readonly IBorrowRecordService _service;
    public BorrowRecordController(IBorrowRecordService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<Responce<List<BorrowRecordGetDto>>> GetItemsAsync()
    {
        return await _service.GetItemsAsync();
    }
    [HttpPost]
    public async Task<Responce<string>> CreateItemAsync([FromForm]BorrowRecordCreateDto dto)
    {
        return await _service.CreateItemAsync(dto);
    }
    [HttpPut("{id:int}")]
    public async Task<Responce<string>> UpdateItemAsync(int id, [FromForm]BorrowRecordUpdateDto dto)
    {
        return await _service.UpdateItemAsync(id, dto);
    }
    [HttpDelete("{id:int}")]
    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        return await _service.DeleteItemAsync(id);
    }
    [HttpGet("{id:int}")]
    public async Task<Responce<BorrowRecordGetDto>> GetItemByIdAsync(int id)
    {
        return await _service.GetItemByIdAsync(id);
    }
    [HttpGet("overdue")]
    public async Task<Responce<List<BorrowRecordGetDto>>> GetOverdueBorrows()
    {
        return await _service.GetOverdueBorrows();
    }
    [HttpGet("member-{id:int}")]
    public async Task<Responce<List<BorrowRecordGetDto>>> GetBorrowHistoryByMember(int id)
    {
        return await _service.GetBorrowHistoryByMember(id);
    }
    [HttpGet("history/book/{id:int}")]
    public async Task<Responce<List<BorrowRecordGetDto>>> GetBorrowHistoryByBook(int id)
    {
        return await _service.GetBorrowHistoryByBook(id);
    }
}
