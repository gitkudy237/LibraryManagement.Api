using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser appUser);
    }
}