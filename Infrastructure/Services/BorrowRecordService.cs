using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using Domain.DTOs.Books;
using Domain.DTOs.BorrowRecords;
using Domain.Entities;
using Infrastructure.APIResponce;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BorrowRecordService:IBorrowRecordService
{
    private readonly DataContext _context;
    public BorrowRecordService(DataContext context)
    {
        _context = context;
    }

    public async Task<Responce<string>> CreateItemAsync(BorrowRecordCreateDto dto)
    {
        if (dto.MemberId == 0) return Responce<string>.Fail(401, "Wrong Id");
        if (dto.BookId == 0) return Responce<string>.Fail(401, "Wrong Id");

        var memberExist = await _context.Members.FindAsync(dto.MemberId);
        if (memberExist == null) return Responce<string>.Fail(404, $"Member with given id : {dto.MemberId} doesnt exist");

        var bookExist = await _context.Books.FindAsync(dto.BookId);
        if (bookExist == null) return Responce<string>.Fail(404, $"Book with given id : {dto.BookId} doesnt exist");
        if (dto.ReturnDate == DateTime.MinValue) return Responce<string>.Fail(401, "Wrong Time");

        var exist = await _context.BorrowRecords.FirstOrDefaultAsync(br => br.BookId == dto.BookId && br.MemberId == dto.MemberId && br.ReturnDate == dto.ReturnDate);
        if (exist != null) return Responce<string>.Fail(409, "Borrow record already exist");

        var newBorrowRecord = new BorrowRecord()
        {
            MemberId = dto.MemberId,
            BookId = dto.BookId,
            ReturnDate = dto.ReturnDate
        };
        await _context.BorrowRecords.AddAsync(newBorrowRecord);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Something goes wrong")
                : Responce<string>.Created("Created successfuly");

    }

    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        var exist = await _context.BorrowRecords.FindAsync(id);
        if (exist == null) return Responce<string>.Fail(404, "Borrow Record to delete not found");

        if (exist.ReturnDate > DateTime.Now) return Responce<string>.Fail(409, "You have active borrowing");


        _context.BorrowRecords.Remove(exist);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Not deleted")
                : Responce<string>.Created("Deleted successfuly");
    }

    public async Task<Responce<List<BorrowRecordGetDto>>> GetBorrowHistoryByBook(int id)
    {
        var historyBooks = await _context.BorrowRecords.Where(br => br.BookId==id).Select(br => new BorrowRecordGetDto()
        {
            Id = br.Id,
            MemberName = br.Member.Name,
            BookTitle = br.Book.Title,
            BorrowDate = br.BorrowDate,
            ReturnDate = br.ReturnDate
        }).ToListAsync();
        return Responce<List<BorrowRecordGetDto>>.Ok(historyBooks);
    }

    public async Task<Responce<List<BorrowRecordGetDto>>> GetBorrowHistoryByMember(int id)
    {
        var historyBooks = await _context.BorrowRecords.Where(br => br.MemberId==id).Select(br => new BorrowRecordGetDto()
        {
            Id = br.Id,
            MemberName = br.Member.Name,
            BookTitle = br.Book.Title,
            BorrowDate = br.BorrowDate,
            ReturnDate = br.ReturnDate
        }).ToListAsync();
        return Responce<List<BorrowRecordGetDto>>.Ok(historyBooks);
    }

    public async Task<Responce<BorrowRecordGetDto>> GetItemByIdAsync(int id)
    {
        var exist = await _context.BorrowRecords.FindAsync(id);
        if (exist == null) return Responce<BorrowRecordGetDto>.Fail(404, $"Borrow record with id {id} not found");
        var convert = new BorrowRecordGetDto()
        {
            Id = exist.Id,
            MemberName = exist.Member.Name,
            BookTitle = exist.Book.Title,
            BorrowDate = exist.BorrowDate,
            ReturnDate=exist.ReturnDate
        };
        return Responce<BorrowRecordGetDto>.Ok(convert);
    }

    public async Task<Responce<List<BorrowRecordGetDto>>> GetItemsAsync()
    {
        var items = await _context.BorrowRecords.Select(br => new BorrowRecordGetDto()
        {
            Id = br.Id,
            MemberName = br.Member.Name,
            BookTitle = br.Book.Title,
            BorrowDate = br.BorrowDate,
            ReturnDate = br.ReturnDate
        }).ToListAsync();
        return Responce<List<BorrowRecordGetDto>>.Ok(items);
    }

    public async Task<Responce<List<BorrowRecordGetDto>>> GetOverdueBorrows()
    {
        var items = await _context.BorrowRecords.Where(br=>br.ReturnDate<DateTime.Now).Select(br => new BorrowRecordGetDto()
        {
            Id = br.Id,
            MemberName = br.Member.Name,
            BookTitle = br.Book.Title,
            BorrowDate = br.BorrowDate,
            ReturnDate = br.ReturnDate
        }).ToListAsync();
        return Responce<List<BorrowRecordGetDto>>.Ok(items);
    }

    public async Task<Responce<string>> UpdateItemAsync(int id, BorrowRecordUpdateDto dto)
    {
        if (dto.MemberId == 0) return Responce<string>.Fail(401, "Wrong Id");
        if (dto.BookId == 0) return Responce<string>.Fail(401, "Wrong Id");

        var memberExist = await _context.Members.FindAsync(dto.MemberId);
        if (memberExist == null) return Responce<string>.Fail(404, $"Member with given id : {dto.MemberId} doesnt exist");

        var bookExist = await _context.Books.FindAsync(dto.BookId);
        if (bookExist == null) return Responce<string>.Fail(404, $"Book with given id : {dto.BookId} doesnt exist");
        if (dto.ReturnDate == DateTime.MinValue) return Responce<string>.Fail(401, "Wrong Time");

        var exist = await _context.BorrowRecords.FindAsync(id);
        if (exist == null) return Responce<string>.Fail(404, "Borrow record to update doesnt exist");

        var noChange = exist.MemberId == dto.MemberId && exist.BookId == dto.BookId && exist.ReturnDate == dto.ReturnDate;
        if (noChange) return Responce<string>.Fail(401, "No changes were made");

        exist.MemberId = dto.MemberId;
        exist.BookId = dto.BookId;
        exist.ReturnDate = dto.ReturnDate;

        var result = await _context.SaveChangesAsync();
         return result == 0
               ? Responce<string>.Fail(500, "Not updated")
               : Responce<string>.Created("Updated successfuly");
        

    }
}
