using System;
using Domain.DTOs.Books;
using Infrastructure.APIResponce;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/book")]
public class BookController
{
    private readonly IBookService _service;
    public BookController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<Responce<List<BookGetDto>>> GetItemsAsync()
    {
        return await _service.GetItemsAsync();
    }
    [HttpPost]
    public async Task<Responce<string>> CreateItemAsync([FromForm] BookCreateDto dto)
    {
        return await _service.CreateItemAsync(dto);

    }
    [HttpPut("{id:int}")]
    public async Task<Responce<string>> UpdateItemAsync(int id, [FromForm] BookUpdateDto dto)
    {
        return await _service.UpdateItemAsync(id, dto);

    }
    [HttpDelete("{id:int}")]
    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        return await _service.DeleteItemAsync(id);

    }
    [HttpGet("{id:int}")]
    public async Task<Responce<BookGetDto>> GetItemByIdAsync(int id)
    {
        return await _service.GetItemByIdAsync(id);

    }
    [HttpGet("genre- {genre}")]
    public async Task<Responce<List<BookGetDto>>> GetItemByGenreAsync(string genre)
    {
        return await _service.GetItemByGenreAsync(genre);

    }
    [HttpGet("recent/{years:int}")]
    public async Task<Responce<List<BookGetDto>>> GetRecentlyPublishedBooks(int years)
    {
        return await _service.GetRecentlyPublishedBooks(years);

    }
}
