using System;
using Domain.DTOs.Authors;
using Infrastructure.APIResponce;

namespace Infrastructure.Interfaces;

public interface IAuthorService
{
    Task<Responce<List<AuthorGetDto>>> GetItemsAsync();
    Task<Responce<string>> CreateItemAsync(AuthorCreateDto dto);
    Task<Responce<string>> UpdateItemAsync(int id, AuthorUpdateDto dto);
    Task<Responce<string>> DeleteItemAsync(int id);
    Task<Responce<AuthorGetDto>> GetItemByIdAsync(int id);
    Task<Responce<List<AuthorGetDto>>> GetAuthorsWithMostBooks();

    

}
