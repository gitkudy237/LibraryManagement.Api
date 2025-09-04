using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AppUserDtos;

namespace LibraryManagement.Core.Abstractions
{
    public interface IAuthService
    {
        Task<ApplicationUser?> RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<string?> LoginAsync(LoginDto loginRequest);
    }
}