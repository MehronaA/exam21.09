using System;
using Domain.DTOs.Members;
using Infrastructure.APIResponce;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/member")]
public class MemberController
{
    private readonly IMemberService _service;
    public MemberController(IMemberService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<Responce<List<MemberGetDto>>> GetItemsAsync()
    {
        return await _service.GetItemsAsync();
    }
    [HttpPost]
    public async Task<Responce<string>> CreateItemAsync([FromForm]MemberCreateDto dto)
    {
        return await _service.CreateItemAsync(dto);
    }
    [HttpPut("{id:int}")]
    public async Task<Responce<string>> UpdateItemAsync(int id, [FromForm]MemberUpdateDto dto)
    {
        return await _service.UpdateItemAsync(id, dto);
    }
    [HttpDelete("{id:int}")]
    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        return await _service.DeleteItemAsync(id);
    }
    [HttpGet("{id:int}")]
    public async Task<Responce<MemberGetDto>> GetItemByIdAsync(int id)
    {
        return await _service.GetItemByIdAsync(id);
    }
    [HttpGet("recent-borrows-{days:int}")]
    public async Task<Responce<List<MemberGetDto>>> GetMembersWithRecentBorrows(int days)
    {
        return await _service.GetMembersWithRecentBorrows(days);
    }
    [HttpGet("top-borrows-{n:int}")]

    public async Task<Responce<List<MemberGetDto>>> GetTopNMembersByBorrows(int n)
    {
        return await _service.GetTopNMembersByBorrows(n);
    }
}
