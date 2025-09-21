using System;
using System.Diagnostics;
using Domain.DTOs.Authors;
using Domain.Entities;
using Infrastructure.APIResponce;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthorService : IAuthorService
{
    private readonly DataContext _context;
    public AuthorService(DataContext context)
    {
        _context = context;
    }
    public async Task<Responce<string>> CreateItemAsync(AuthorCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return Responce<string>.Fail(409, "Name is required");
        if ((DateTime.Now.Year - dto.BirthDate.Year) < 18) return Responce<string>.Fail(401, "Author cannot be under 18");
        if (dto.BirthDate == DateTime.MinValue) return Responce<string>.Fail(401, "Wrong date");

        if (dto.Name.Trim().Length > 150) return Responce<string>.Fail(400, "Name must have less 150 characters");

        var exist = await _context.Authors.FirstOrDefaultAsync(a => a.Name == dto.Name && a.BirthDate == dto.BirthDate);

        if (exist != null) return Responce<string>.Fail(422, "Author already exist");

        var newAuhtor = new Author()
        {
            Name = dto.Name,
            BirthDate = dto.BirthDate
        };

        await _context.Authors.AddAsync(newAuhtor);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Something goes wrong")
                : Responce<string>.Created("Created successfuly");

    }

    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        var exist = await _context.Authors.FindAsync(id);
        if (exist == null) return Responce<string>.Fail(404, "Author to delete not found");

        var hasBook = await _context.Books.FirstOrDefaultAsync(b => b.AuthorId == id);
        if (hasBook != null) return Responce<string>.Fail(409, "Cannot delete author because of books");

        _context.Authors.Remove(exist);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Not deleted")
                : Responce<string>.Created("Deleted successfuly");
    }

    public async Task<Responce<List<AuthorGetDto>>> GetAuthorsWithMostBooks()
    {

        var maxCount = await _context.Authors
        .Select(a => a.Books.Count)
        .MaxAsync();

        var items = await _context.Authors
        .Where(a => a.Books.Count == maxCount)
        .Select(a => new AuthorGetDto
        {
            Name = a.Name,
            BirthDate = a.BirthDate
        })
        .ToListAsync();
        return Responce<List<AuthorGetDto>>.Ok(items);
        
    }

    public async Task<Responce<AuthorGetDto>> GetItemByIdAsync(int id)
    {
        var exist = await _context.Authors.FindAsync(id);
        if (exist == null) return Responce<AuthorGetDto>.Fail(404, $"Author with this id: {id} not found");

        var convert = new AuthorGetDto()
        {
            Id = exist.Id,
            Name = exist.Name,
            BirthDate = exist.BirthDate
        };

        return Responce<AuthorGetDto>.Ok(convert);
        

    }

    public async Task<Responce<List<AuthorGetDto>>> GetItemsAsync()
    {
        var items = await _context.Authors.Select(a => new AuthorGetDto()
        {
            Id = a.Id,
            Name = a.Name,
            BirthDate = a.BirthDate
        }).ToListAsync();
        return Responce<List<AuthorGetDto>>.Ok(items);

    }

    public async Task<Responce<string>> UpdateItemAsync(int id, AuthorUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return Responce<string>.Fail(409, "Name is required");

        if ((DateTime.Now.Year - dto.BirthDate.Year) < 18) return Responce<string>.Fail(401, "Author cannot be under 18");

        if (dto.BirthDate == DateTime.MinValue) return Responce<string>.Fail(401, "Wrong date");

        if (dto.Name.Trim().Length > 150) return Responce<string>.Fail(400, "Name must have less 150 characters");

        var exist = await _context.Authors.FindAsync(id);

        if (exist == null) return Responce<string>.Fail(404, "Author to update not found");

        var noChange = exist.Name == dto.Name && exist.BirthDate == dto.BirthDate;
        if (noChange) return Responce<string>.Fail(401, "No changes were made");

        exist.Name = dto.Name;
        exist.BirthDate = dto.BirthDate;
        var result = await _context.SaveChangesAsync();
         return result == 0
               ? Responce<string>.Fail(500, "Not updated")
               : Responce<string>.Created("Updated successfuly");
    }
}
