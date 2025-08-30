using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BorrowerDtos;

namespace LibraryManagement.Mappings;

public static class BorrowerMappings
{
    public static Member ToMemberModel(this CreateMemberDto CreateMemberDto)
    {
        return new Member
        {
            Name = CreateMemberDto.Name,
            Email = CreateMemberDto.Email,
            Phone = CreateMemberDto.Phone
        };
    }

    public static MemberDto ToMemberDto(this Member MemberModel)
    {
        return new MemberDto
        {
            Id = MemberModel.Id,
            Name = MemberModel.Name,
            Email = MemberModel.Email,
            Phone = MemberModel.Phone
        };
    }

    public static void MapUpdateMember(this Member memberModel, UpdateMemberDto UpdateMemberDto)
    {
        memberModel.Name = UpdateMemberDto.Name;
        memberModel.Email = UpdateMemberDto.Email;
        memberModel.Phone = UpdateMemberDto.Phone;
    }
}
