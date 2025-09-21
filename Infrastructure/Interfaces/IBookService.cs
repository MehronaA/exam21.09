using System;
using Domain.DTOs.Books;
using Infrastructure.APIResponce;

namespace Infrastructure.Interfaces;

public interface IBookService
{
    Task<Responce<List<BookGetDto>>> GetItemsAsync();
    Task<Responce<string>> CreateItemAsync(BookCreateDto dto);
    Task<Responce<string>> UpdateItemAsync(int id, BookUpdateDto dto);
    Task<Responce<string>> DeleteItemAsync(int id);
    Task<Responce<BookGetDto>> GetItemByIdAsync(int id);
    Task<Responce<List<BookGetDto>>> GetItemByGenreAsync(string genre);
    Task<Responce<List<BookGetDto>>> GetRecentlyPublishedBooks(int years);
    
}
