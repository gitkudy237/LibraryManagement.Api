using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AppUserDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// TODO:   Create MemberRegisterDto and AdminRegisterDto
// TODO: Create TokenService
// TODO: Add corresponding settings in appsettings.json
namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AccountsController(LibraryDbContext context)
        {
            _context = context;
        }

        private static ApplicationUser _user = new();

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegistrationDto registrationRequest)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registrationRequest.Email))
                return BadRequest();

            _user = registrationRequest.ToApplicationUserModel();

            var hashedPassword = new PasswordHasher<ApplicationUser>()
                .HashPassword(_user, registrationRequest.Password);
            _user.PasswordHash = hashedPassword;

            await _context.Users.AddAsync(_user);
            await _context.SaveChangesAsync();

            return Ok(_user);
        }
    }


}