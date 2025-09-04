using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AppUserDtos;

namespace LibraryManagement.Mappings
{
    public static class AppUserMappings
    {
        public static ApplicationUser ToApplicationUserModel(this UserRegistrationRequest userRegistration)
        {
            return new ApplicationUser
            {
                Id = Guid.NewGuid().ToString().Substring(0, 8),
                Email = userRegistration.Email,
                Role = userRegistration.Role
            };
        }

        public static UserRegistrationRequest ToRegistrationRequest(this AdminRegistrationDto adminRegistrationDto)
        {
            return new UserRegistrationRequest
            {
                Email = adminRegistrationDto.Email,
                Password = adminRegistrationDto.Password,
                Role = UserRoles.Admin.ToString()
            };
        }

        public static UserRegistrationRequest ToRegistrationRequest(this MemberRegistrationDto memberRegistrationDto)
        {
            return new UserRegistrationRequest
            {
                Email = memberRegistrationDto.Email,
                Password = memberRegistrationDto.Password,
                Role = UserRoles.Member.ToString()
            };
        }

        public static AppUserDto ToAppUserDto(this ApplicationUser appUser)
        {
            return new AppUserDto
            {
                Email = appUser.Email,
                Role = appUser.Role
            };
        }
    }
}