using System;
using Domain.DTOs.Members;
using Domain.Entities;
using Infrastructure.APIResponce;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class MemberService : IMemberService
{
    private readonly DataContext _context;
    public MemberService(DataContext context)
    {
        _context = context;
    }
    public async Task<Responce<string>> CreateItemAsync(MemberCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return Responce<string>.Fail(409, "Name is required");
        if (string.IsNullOrWhiteSpace(dto.Email)) return Responce<string>.Fail(409, "Email is required");

        if (dto.Name.Trim().Length > 150) return Responce<string>.Fail(401, "Name must have less than 150 characters");
        if (dto.Email.Trim().Length > 200) return Responce<string>.Fail(401, "Email must have less than 200 characters");

        var exist = await _context.Members.FirstOrDefaultAsync(m => m.Name == dto.Name && m.Email == dto.Email);
        if (exist != null) return Responce<string>.Fail(409, "Member is already exist");

        var newMember = new Member()
        {
            Name = dto.Name,
            Email = dto.Email
        };

        await _context.Members.AddAsync(newMember);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Something goes wrong")
                : Responce<string>.Created("Created successfuly");
    }

    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        var exist = await _context.Members.FindAsync(id);
        if (exist == null) return Responce<string>.Fail(404, "memberto delete not found");

        var find = await _context.BorrowRecords.FirstOrDefaultAsync(br => br.MemberId == id);
        if (find != null) return Responce<string>.Fail(409, "Cannot delete member because od borrowing");

        _context.Members.Remove(exist);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Not deleted")
                : Responce<string>.Created("Deleted successfuly");
    }

    public Task<Responce<MemberGetDto>> GetItemByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Responce<List<MemberGetDto>>> GetItemsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Responce<List<MemberGetDto>>> GetMembersWithRecentBorrows(int years)
    {
        throw new NotImplementedException();
    }

    public Task<Responce<List<MemberGetDto>>> GetTopNMembersByBorrows(int n)
    {
        throw new NotImplementedException();
    }

    public Task<Responce<string>> UpdateItemAsync(int id, MemberUpdateDto dto)
    {
        throw new NotImplementedException();
    }
}
