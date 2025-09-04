using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AppUserDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly LibraryDbContext _context;

        public AuthService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> RegisterAsync(UserRegistrationRequest registrationRequest)
        {
            if (_context.Users.Any(u => u.Email == registrationRequest.Email))
                return null;

            var newUser = registrationRequest.ToApplicationUserModel();
            var hashedPassword = new PasswordHasher<ApplicationUser>()
                .HashPassword(newUser, registrationRequest.Password);
            newUser.PasswordHash = hashedPassword;

            await _context.Users.AddAsync(newUser);

            return newUser;
        }


    }
}