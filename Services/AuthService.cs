using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AppUserDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly LibraryDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(LibraryDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
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

        public async Task<string?> LoginAsync(LoginDto loginRequest)
        {
            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (appUser is null) return null;

            if (new PasswordHasher<ApplicationUser>()
                .VerifyHashedPassword(appUser, appUser.PasswordHash, loginRequest.Password) == PasswordVerificationResult.Failed)
                return null;

            var token = _tokenService.CreateToken(appUser);
            return token;
        }

    }
}