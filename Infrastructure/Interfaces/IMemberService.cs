using System;
using Domain.DTOs.Members;
using Infrastructure.APIResponce;

namespace Infrastructure.Interfaces;

public interface IMemberService
{
    Task<Responce<List<MemberGetDto>>> GetItemsAsync();
    Task<Responce<string>> CreateItemAsync(MemberCreateDto dto);
    Task<Responce<string>> UpdateItemAsync(int id, MemberUpdateDto dto);
    Task<Responce<string>> DeleteItemAsync(int id);
    Task<Responce<MemberGetDto>> GetItemByIdAsync(int id);
    Task<Responce<List<MemberGetDto>>> GetMembersWithRecentBorrows(int days);
    Task<Responce<List<MemberGetDto>>> GetTopNMembersByBorrows(int n);
}
