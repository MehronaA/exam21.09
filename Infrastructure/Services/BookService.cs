using System;
using System.Diagnostics;
using Domain.DTOs.Books;
using Domain.Entities;
using Infrastructure.APIResponce;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BookService : IBookService
{
    private readonly DataContext _context;
    public BookService(DataContext context)
    {
        _context = context;
    }
    public async Task<Responce<string>> CreateItemAsync(BookCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) return Responce<string>.Fail(409, "Title is required");
        if (string.IsNullOrWhiteSpace(dto.Genre)) return Responce<string>.Fail(409, "Genre is required");
        if (dto.PublishedYear < 0 || dto.PublishedYear > DateTime.Now.Year) return Responce<string>.Fail(409, "Wrong Published year");
        if (dto.AuthorId == 0) return Responce<string>.Fail(409, "Author Id is required");
        if (dto.Title.Trim().Length > 200) return Responce<string>.Fail(401, "Title must have less than 200 characters");
        if (dto.Genre.Trim().Length > 100) return Responce<string>.Fail(401, "Genre must have less than 100 characters");

        var exist = await _context.Books.FirstOrDefaultAsync(b => b.Title == dto.Title);
        if (exist != null) return Responce<string>.Fail(409, "Book is already exist");

        var newBook = new Book()
        {
            Title = dto.Title,
            Genre = dto.Genre,
            PublishedYear = dto.PublishedYear,
            AuthorId = dto.AuthorId
        };

        await _context.Books.AddAsync(newBook);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Something goes wrong")
                : Responce<string>.Created("Created successfuly");
    }

    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        var exist = await _context.Books.FindAsync(id);
        if (exist == null) return Responce<string>.Fail(404, "Author to delete not found");

        var hasBorrowReccord = await _context.BorrowRecords.FirstOrDefaultAsync(br => br.BookId == id);
        if (hasBorrowReccord != null) return Responce<string>.Fail(409, "Cannot delete book because of borrowing record");

        _context.Books.Remove(exist);
        var result = await _context.SaveChangesAsync();
        return result == 0
                ? Responce<string>.Fail(500, "Not deleted")
                : Responce<string>.Created("Deleted successfuly");
    }

    public async Task<Responce<List<BookGetDto>>> GetItemByGenreAsync(string genre)
    {
        var items = await _context.Books.Select(b => new BookGetDto()
        {
            Id = b.Id,
            Title = b.Title,
            Genre = b.Genre,
            PublishedYear = b.PublishedYear,
            AuthorName = b.Author.Name
        }).Where(b => b.Genre == genre).ToListAsync();
        return Responce<List<BookGetDto>>.Ok(items);

    }

    public async Task<Responce<BookGetDto>> GetItemByIdAsync(int id)
    {
        var exist = await _context.Books.FindAsync(id);
        if (exist == null) return Responce<BookGetDto>.Fail(404, $"Book with this id: {id} not found");

        var newBook = new BookGetDto()
        {
            Id = exist.Id,
            Title = exist.Title,
            Genre = exist.Genre,
            PublishedYear = exist.PublishedYear,
            AuthorName = exist.Author.Name
        };

        return Responce<BookGetDto>.Ok(newBook);


    }

    public async Task<Responce<List<BookGetDto>>> GetItemsAsync()
    {
        var items = await _context.Books.Select(b => new BookGetDto()
        {
            Id = b.Id,
            Title = b.Title,
            Genre = b.Genre,
            PublishedYear = b.PublishedYear,
            AuthorName = b.Author.Name
        }).ToListAsync();

        return Responce<List<BookGetDto>>.Ok(items);
    }

    public async Task<Responce<List<BookGetDto>>> GetRecentlyPublishedBooks(int years)
    {

        
        var items = await _context.Books
            .Where(b => b.PublishedYear >= DateTime.Now.Year - years)
            .Select(b => new BookGetDto
            {
                Id = b.Id,
                Title = b.Title,
                Genre = b.Genre,
                PublishedYear = b.PublishedYear,
                AuthorName = b.Author.Name
            })
            .ToListAsync();
            //if(items==null) return Responce<List<BookGetDto>>.Fail(404,"Genre not exist or no book with this genre");
        return Responce<List<BookGetDto>>.Ok(items);
    }

    public async Task<Responce<string>> UpdateItemAsync(int id, BookUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) return Responce<string>.Fail(409, "Title is required");
        if (string.IsNullOrWhiteSpace(dto.Genre)) return Responce<string>.Fail(409, "Genre is required");
        if (dto.PublishedYear < 0 || dto.PublishedYear > DateTime.Now.Year) return Responce<string>.Fail(409, "Wrong Published year");
        if (dto.AuthorId == 0) return Responce<string>.Fail(409, "Author Id is required");
        if (dto.Title.Trim().Length > 200) return Responce<string>.Fail(401, "Title must have less than 200 characters");
        if (dto.Genre.Trim().Length > 100) return Responce<string>.Fail(401, "Genre must have less than 100 characters");

        var exist = await _context.Books.FindAsync(id);
        if (exist == null) return Responce<string>.Fail(409, "Book to update doesnt exist");

        var noChange = exist.Title == dto.Title && exist.Genre == dto.Genre && exist.PublishedYear == dto.PublishedYear && exist.AuthorId == dto.AuthorId;
        if (noChange) return Responce<string>.Fail(400, "No changes were made");

        exist.Title = dto.Title;
        exist.Genre = dto.Genre;
        exist.PublishedYear = dto.PublishedYear;
        exist.AuthorId = dto.AuthorId;

        var result = await _context.SaveChangesAsync();
         return result == 0
               ? Responce<string>.Fail(500, "Not updated")
               : Responce<string>.Created("Updated successfuly");


    }
}
