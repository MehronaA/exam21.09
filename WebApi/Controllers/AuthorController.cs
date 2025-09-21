using System;
using Domain.DTOs.Authors;
using Infrastructure.APIResponce;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController
{
    private readonly IAuthorService _service;
    public AuthorController(IAuthorService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<Responce<List<AuthorGetDto>>> GetItemsAsync()
    {
        return await _service.GetItemsAsync();
    }
    [HttpPost]
    public async Task<Responce<string>> CreateItemAsync([FromForm]AuthorCreateDto dto)
    {
        return await _service.CreateItemAsync(dto);

    }
    [HttpPut("{id:int}")]
    public async Task<Responce<string>> UpdateItemAsync(int id, [FromForm] AuthorUpdateDto dto)
    {
        return await _service.UpdateItemAsync(id, dto);

    }
    [HttpDelete("{id:int}")]
    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        return await _service.DeleteItemAsync(id);

    }
    [HttpGet("{id}")]
    public async Task<Responce<AuthorGetDto>> GetItemByIdAsync(int id)
    {
        return await _service.GetItemByIdAsync(id);

    }
    [HttpGet("/most-books")]
    public async Task<Responce<List<AuthorGetDto>>> GetAuthorsWithMostBooks()
    {
        return await _service.GetAuthorsWithMostBooks();

    }

}
