using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AppUserDtos;
using LibraryManagement.Dtos.BorrowerDtos;

namespace LibraryManagement.Mappings;

public static class BorrowerMappings
{
    public static Member ToMemberModel(this CreateMemberDto CreateMemberDto)
    {
        return new Member
        {
            Name = CreateMemberDto.Name,
            Phone = CreateMemberDto.Phone
        };
    }

    public static MemberDto ToMemberDto(this Member MemberModel)
    {
        return new MemberDto
        {
            Id = MemberModel.Id,
            Name = MemberModel.Name,
            Phone = MemberModel.Phone
        };
    }

    public static void MapUpdateMember(this Member memberModel, UpdateMemberDto UpdateMemberDto)
    {
        memberModel.Name = UpdateMemberDto.Name;
        memberModel.Phone = UpdateMemberDto.Phone;
    }

    public static ApplicationUser ToApplicationUserModel(this MemberRegistrationDto memberRegistrationDto)
    {
        return new ApplicationUser
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8),
            Email = memberRegistrationDto.Email,
            Role = UserRoles.Member.ToString()
        };
    }

    public static Member ToMemberModel(this MemberRegistrationDto memberRegistrationDto, string userId)
    {
        return new Member
        {
            Name = memberRegistrationDto.Name,
            Phone = memberRegistrationDto.Phone,
            UserId = userId
        };
    }
}
